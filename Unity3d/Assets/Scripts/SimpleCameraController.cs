using UnityEngine;

namespace UnityTemplateProjects
{
    public class SimpleCameraController : MonoBehaviour
    {
        class CameraState
        {
            public float yaw;
            public float pitch;
            public float roll;
            public float x;
            public float y;
            public float z;

            public void SetFromTransform(Transform t)
            {
                pitch = t.eulerAngles.x;
                yaw = t.eulerAngles.y;
                roll = t.eulerAngles.z;
                x = t.position.x;
                y = t.position.y;
                z = t.position.z;
            }

            public void Translate(Vector3 translation)
            {
                Vector3 rotatedTranslation = Quaternion.Euler(pitch, yaw, roll) * translation;

                x += rotatedTranslation.x;
                y += rotatedTranslation.y;
                z += rotatedTranslation.z;
            }

            public void LerpTowards(CameraState target, float positionLerpPct, float rotationLerpPct)
            {
                yaw = Mathf.Lerp(yaw, target.yaw, rotationLerpPct);
                pitch = Mathf.Lerp(pitch, target.pitch, rotationLerpPct);
                roll = Mathf.Lerp(roll, target.roll, rotationLerpPct);
                
                x = Mathf.Lerp(x, target.x, positionLerpPct);
                y = Mathf.Lerp(y, target.y, positionLerpPct);
                z = Mathf.Lerp(z, target.z, positionLerpPct);
            }

            public void UpdateTransform(Transform t)
            {
                t.eulerAngles = new Vector3(pitch, yaw, roll);
                t.position = new Vector3(x, y, z);
            }
        }
        
        CameraState m_TargetCameraState = new CameraState();
        CameraState m_InterpolatingCameraState = new CameraState();

        // ---- Automation API -------------------------------------------------------
        // External tooling (GM / DSL commands) must drive the camera THROUGH this
        // controller, not by writing transform directly: Update() owns the transform and
        // rebuilds it from m_InterpolatingCameraState every frame, so a direct write is
        // reverted immediately. These methods update BOTH states so the requested pose is
        // applied at once without fighting the interpolation, and remain fully compatible
        // with normal mouse/keyboard input (which just updates m_TargetCameraState later).
        static void WriteState(CameraState s, Vector3 position, Vector3 eulerAngles)
        {
            s.x = position.x; s.y = position.y; s.z = position.z;
            s.pitch = eulerAngles.x; s.yaw = eulerAngles.y; s.roll = eulerAngles.z;
        }

        public void ReadPose(out Vector3 position, out Vector3 eulerAngles)
        {
            position = new Vector3(m_TargetCameraState.x, m_TargetCameraState.y, m_TargetCameraState.z);
            eulerAngles = new Vector3(m_TargetCameraState.pitch, m_TargetCameraState.yaw, m_TargetCameraState.roll);
        }

        public void ApplyPose(Vector3 position, Vector3 eulerAngles)
        {
            WriteState(m_TargetCameraState, position, eulerAngles);
            WriteState(m_InterpolatingCameraState, position, eulerAngles);
            m_InterpolatingCameraState.UpdateTransform(transform);
        }

        public void MoveBy(Vector3 delta, bool local)
        {
            Vector3 pos, euler;
            ReadPose(out pos, out euler);
            ApplyPose(pos + (local ? Quaternion.Euler(euler) * delta : delta), euler);
        }

        public void AimAt(Vector3 target)
        {
            Vector3 pos, euler;
            ReadPose(out pos, out euler);
            Vector3 dir = target - pos;
            if (dir.sqrMagnitude < 0.0001f)
                return;
            ApplyPose(pos, Quaternion.LookRotation(dir.normalized, Vector3.up).eulerAngles);
        }

        // Continuous, non-blocking sweep driven by this component's own Update().
        bool m_SweepOn;
        float m_SweepDegPerSec, m_SweepRadius, m_SweepAngle;
        Vector3 m_SweepCenter, m_SweepBasePos, m_SweepBaseEuler;

        public bool IsSweeping { get { return m_SweepOn; } }

        public void SetSweep(bool enable, float degPerSec, float radius, Vector3 center)
        {
            m_SweepOn = false;
            if (!enable)
                return;
            ReadPose(out m_SweepBasePos, out m_SweepBaseEuler);
            m_SweepDegPerSec = degPerSec;
            m_SweepRadius = radius;
            m_SweepCenter = center;
            m_SweepAngle = 0f;
            m_SweepOn = true;
        }

        void UpdateSweep()
        {
            m_SweepAngle += m_SweepDegPerSec * Time.deltaTime;
            if (m_SweepAngle > 360f) m_SweepAngle -= 360f;
            if (m_SweepRadius > 0f)
            {
                float a = m_SweepAngle * Mathf.Deg2Rad;
                Vector3 pos = new Vector3(
                    m_SweepCenter.x + Mathf.Sin(a) * m_SweepRadius,
                    m_SweepBasePos.y,
                    m_SweepCenter.z + Mathf.Cos(a) * m_SweepRadius);
                ApplyPose(pos, new Vector3(m_SweepBaseEuler.x,
                    m_SweepBaseEuler.y + m_SweepAngle, m_SweepBaseEuler.z));
                AimAt(m_SweepCenter);
            }
            else
            {
                ApplyPose(m_SweepBasePos, new Vector3(m_SweepBaseEuler.x,
                    m_SweepBaseEuler.y + m_SweepAngle, m_SweepBaseEuler.z));
            }
        }
        // ---- end Automation API --------------------------------------------------

        [Header("Movement Settings")]
        [Tooltip("Exponential boost factor on translation, controllable by mouse wheel.")]
        public float boost = 3.5f;

        [Tooltip("Time it takes to interpolate camera position 99% of the way to the target."), Range(0.001f, 1f)]
        public float positionLerpTime = 0.2f;

        [Header("Rotation Settings")]
        [Tooltip("X = Change in mouse position.\nY = Multiplicative factor for camera rotation.")]
        public AnimationCurve mouseSensitivityCurve = new AnimationCurve(new Keyframe(0f, 0.5f, 0f, 5f), new Keyframe(1f, 2.5f, 0f, 0f));

        [Tooltip("Time it takes to interpolate camera rotation 99% of the way to the target."), Range(0.001f, 1f)]
        public float rotationLerpTime = 0.01f;

        [Tooltip("Whether or not to invert our Y axis for mouse input to rotation.")]
        public bool invertY = false;

        void OnEnable()
        {
            m_TargetCameraState.SetFromTransform(transform);
            m_InterpolatingCameraState.SetFromTransform(transform);
        }

        Vector3 GetInputTranslationDirection()
        {
            Vector3 direction = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.back;
            }
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
            }
            if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                direction += Vector3.down;
            }
            if (Input.GetKey(KeyCode.E))
            {
                direction += Vector3.up;
            }
            return direction;
        }
        
        void Update()
        {
            // Automation sweep owns the pose while enabled; skip mouse/keyboard input so
            // the two drivers do not fight over m_TargetCameraState.
            if (m_SweepOn)
            {
                UpdateSweep();
                return;
            }

            // Hide and lock cursor when right mouse button pressed
            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Unlock and show cursor when right mouse button released
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            // Rotation
            if (Input.GetMouseButton(1))
            {
                var mouseMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y") * (invertY ? 1 : -1));
                
                var mouseSensitivityFactor = mouseSensitivityCurve.Evaluate(mouseMovement.magnitude);

                m_TargetCameraState.yaw += mouseMovement.x * mouseSensitivityFactor;
                m_TargetCameraState.pitch += mouseMovement.y * mouseSensitivityFactor;
            }
            
            // Translation
            var translation = GetInputTranslationDirection() * Time.deltaTime;

            // Speed up movement when shift key held
            if (Input.GetKey(KeyCode.LeftShift))
            {
                translation *= 10.0f;
            }
            
            // Modify movement by a boost factor (defined in Inspector and modified in play mode through the mouse scroll wheel)
            boost += Input.mouseScrollDelta.y * 0.2f;
            translation *= Mathf.Pow(2.0f, boost);

            m_TargetCameraState.Translate(translation);

            // Framerate-independent interpolation
            // Calculate the lerp amount, such that we get 99% of the way to our target in the specified time
            var positionLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / positionLerpTime) * Time.deltaTime);
            var rotationLerpPct = 1f - Mathf.Exp((Mathf.Log(1f - 0.99f) / rotationLerpTime) * Time.deltaTime);
            m_InterpolatingCameraState.LerpTowards(m_TargetCameraState, positionLerpPct, rotationLerpPct);

            m_InterpolatingCameraState.UpdateTransform(transform);
        }
    }

}