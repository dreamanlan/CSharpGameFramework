using UnityEngine;
using UnityEditor;

namespace Unity.MemoryProfilerForExtension.Editor.UI
{
    internal class ZoomArea
    {
        // Global state
        private static Vector2 m_WorldMouseDownPosition = new Vector2(-1000000, -1000000); // in transformed space
        private static int zoomableAreaHash = "ZoomableArea".GetHashCode();


        public class Styles
        {
            public GUIStyle background = "OL Box";
        }

        public Styles m_Styles;
        public Styles styles
        {
            get
            {
                if (m_Styles == null)
                    m_Styles = new Styles();
                return m_Styles;
            }
        }

        public bool mbHasViewSpaceRect = false;
        public Rect m_ViewSpace = new Rect(float.NegativeInfinity, float.NegativeInfinity, float.PositiveInfinity, float.PositiveInfinity);//space in render window in pixel
        public Rect m_WorldSpace = new Rect(-1, -1, 1, 1);

        // initial transformation to center the world into the view space
        public Vector2 m_WorldToViewTranslation;
        public Vector2 m_WorldToViewScale;

        // Effective tranformation of initial and user transformation
        public Vector2 m_WorldToViewScale_Effective = new Vector2(1, 1);
        public Vector2 m_WorldToViewTranslation_Effective = new Vector2(0, 0);

        // Current user-defined translation
        public Vector2 m_Scale = new Vector2(1, 1);
        public Vector2 m_Translation = new Vector2(0, 0);

        public bool mbAnimated = false;
        public System.Diagnostics.Stopwatch mAnimTimer;
        public enum AnimType
        {
            Anim_2Point,
            Anim_3Point,
        }
        AnimType mAnimType;
        public float mAnimDuration;
        public Vector2 m_Scale_Anim_Start = new Vector2(1, 1);
        public Vector2 m_Scale_Anim_Delta = new Vector2(1, 1);
        public Vector2 m_Scale_Anim_Mid = new Vector2(1, 1);
        public Vector2 m_Scale_Anim_Mid_Delta = new Vector2(1, 1);
        public Vector2 m_Scale_Anim_Target = new Vector2(1, 1);
        public Vector2 m_Translation_Anim_Start = new Vector2(0, 0);
        public Vector2 m_Translation_Anim_Delta = new Vector2(0, 0);
        public Vector2 m_Translation_Anim_Mid = new Vector2(0, 0);
        public Vector2 m_Translation_Anim_Mid_Delta = new Vector2(0, 0);
        public Vector2 m_Translation_Anim_Target = new Vector2(0, 0);
        public delegate void AnimCallback();
        public AnimCallback mAnimMidCB;

        public void FocusTo(float targetScale, Vector2 targetWorldPosition, AnimCallback animMidCB, bool force2PointAnim)
        {
            mAnimMidCB = animMidCB;

            var effectiveTargetScale = m_WorldToViewScale * targetScale;
            var effectiveTargetTranslation = m_WorldToViewTranslation + (-targetWorldPosition * effectiveTargetScale);
            if (force2PointAnim || ViewInWorldSpace.Contains(targetWorldPosition))
            {
                mAnimType = AnimType.Anim_2Point;
                mAnimDuration = 2;
                m_Scale_Anim_Start = m_Scale;
                m_Scale_Anim_Target = effectiveTargetScale / m_WorldToViewScale;
                m_Scale_Anim_Delta = m_Scale_Anim_Target - m_Scale_Anim_Start;
                m_Translation_Anim_Start = m_Translation;
                m_Translation_Anim_Target = effectiveTargetTranslation - m_WorldToViewTranslation;
                m_Translation_Anim_Delta = m_Translation_Anim_Target - m_Translation_Anim_Start;
            }
            else
            {
                var currentWorldPos = -(m_WorldToViewTranslation_Effective - m_WorldToViewTranslation) / m_WorldToViewScale_Effective;
                var midWorldPos = 0.5f * (currentWorldPos + targetWorldPosition);
                float marginPercent = 0.01f;
                var w = Mathf.Abs(targetWorldPosition.x - currentWorldPos.x) + marginPercent * m_WorldSpace.width;
                var h = Mathf.Abs(targetWorldPosition.y - currentWorldPos.y) + marginPercent * m_WorldSpace.height;
                var zw = m_WorldSpace.width / w;
                var zh = m_WorldSpace.height / h;
                var z = Mathf.Min(zw, zh);
                z = Mathf.Min(m_Scale.x, z);
                var midScale = new Vector2(z, z);//0.5f * (Vector2.one + m_Scale);

                var effectiveMidScale = m_WorldToViewScale * midScale;
                var effectiveMidTranslation = m_WorldToViewTranslation + (-midWorldPos * effectiveMidScale);


                mAnimType = AnimType.Anim_3Point;
                m_Scale_Anim_Start = m_Scale;
                m_Scale_Anim_Mid = midScale;
                m_Scale_Anim_Target = effectiveTargetScale / m_WorldToViewScale;

                m_Scale_Anim_Delta = m_Scale_Anim_Mid - m_Scale_Anim_Start;
                m_Scale_Anim_Mid_Delta = m_Scale_Anim_Target - m_Scale_Anim_Mid;

                m_Translation_Anim_Start = m_Translation;
                m_Translation_Anim_Mid = effectiveMidTranslation - m_WorldToViewTranslation;
                m_Translation_Anim_Target = effectiveTargetTranslation - m_WorldToViewTranslation;

                m_Translation_Anim_Delta = m_Translation_Anim_Mid - m_Translation_Anim_Start;
                m_Translation_Anim_Mid_Delta = m_Translation_Anim_Target - m_Translation_Anim_Mid;

                mAnimDuration = 4;
            }
            mbAnimated = true;
            mAnimTimer = new System.Diagnostics.Stopwatch();
            mAnimTimer.Start();
        }

        private float EaseInOutCubic(float time, float startValue, float deltaValue, float duration)
        {
            time /= duration / 2;
            if (time < 1) return deltaValue * 0.5f * time * time * time + startValue;
            time -= 2;
            return deltaValue * 0.5f * (time * time * time + 2) + startValue;
        }

        private float EaseInQuad(float time, float startValue, float deltaValue, float duration)
        {
            time /= duration;
            return deltaValue * time * time + startValue;
        }

        private float EaseOutQuad(float time, float startValue, float deltaValue, float duration)
        {
            time /= duration;
            return -deltaValue * time * (time - 2) + startValue;
        }

        private void AnimStep()
        {
            if (mbAnimated)
            {
                if (mAnimTimer == null)
                {
                    mbAnimated = false;
                    return;
                }
                var animTime = mAnimTimer.Elapsed.TotalSeconds;

                if (animTime >= mAnimDuration * 0.5f)
                {
                    if (mAnimMidCB != null)
                    {
                        mAnimMidCB();
                        mAnimMidCB = null;
                    }
                }
                if (animTime >= mAnimDuration)
                {
                    m_Scale = m_Scale_Anim_Target;
                    m_Translation = m_Translation_Anim_Target;
                    mAnimTimer = null;
                }
                else
                {
                    //float t = EaseInOutCubic((float)animTime, 0, 1, duration);
                    Vector2 scaleMin;
                    Vector2 scaleDelta;
                    Vector2 translationMin;
                    Vector2 translationDelta;
                    float t;
                    switch (mAnimType)
                    {
                        default:
                        case AnimType.Anim_2Point:

                            t = EaseInOutCubic((float)animTime, 0, 1, mAnimDuration);
                            scaleMin = m_Scale_Anim_Start;
                            scaleDelta = m_Scale_Anim_Delta;
                            translationMin = m_Translation_Anim_Start;
                            translationDelta = m_Translation_Anim_Delta;

                            break;
                        case AnimType.Anim_3Point:

                            if (animTime < mAnimDuration * 0.5f)
                            {
                                //scale out
                                t = EaseInOutCubic((float)animTime, 0, 1, mAnimDuration * 0.5f);
                                scaleMin = m_Scale_Anim_Start;
                                scaleDelta = m_Scale_Anim_Delta;
                                translationMin = m_Translation_Anim_Start;
                                translationDelta = m_Translation_Anim_Delta;
                            }
                            else
                            {
                                //scale in
                                t = EaseInOutCubic((float)animTime - mAnimDuration * 0.5f, 0, 1, mAnimDuration * 0.5f);
                                scaleMin = m_Scale_Anim_Mid;
                                scaleDelta = m_Scale_Anim_Mid_Delta;
                                translationMin = m_Translation_Anim_Mid;
                                translationDelta = m_Translation_Anim_Mid_Delta;
                            }
                            break;
                    }

                    m_Scale = scaleMin + t * scaleDelta;
                    m_Translation = translationMin + t * translationDelta;
                }
                EnforceScaleAndRange();
            }
        }

        public Matrix4x4 worldToViewMatrix
        {
            get;
            private set;
        }
        public Rect WorldInViewSpace
        {
            get
            {
                var min = WorldToViewTransformPoint(m_WorldSpace.min);
                var max = WorldToViewTransformPoint(m_WorldSpace.max);
                return Rect.MinMaxRect(Mathf.Min(min.x, max.x), Mathf.Min(min.y, max.y), Mathf.Max(min.x, max.x), Mathf.Max(min.y, max.y));
            }
        }
        public Rect ViewInWorldSpace
        {
            get
            {
                var min = ViewToWorldTransformPoint(Vector2.zero);
                var max = ViewToWorldTransformPoint(new Vector2(m_ViewSpace.width, m_ViewSpace.height));
                return Rect.MinMaxRect(Mathf.Min(min.x, max.x), Mathf.Min(min.y, max.y), Mathf.Max(min.x, max.x), Mathf.Max(min.y, max.y));
            }
        }
        public Vector2 WorldToViewTransformPoint(Vector2 lhs)
        { return new Vector2(lhs.x * m_WorldToViewScale_Effective.x + m_WorldToViewTranslation_Effective.x, lhs.y * m_WorldToViewScale_Effective.y + m_WorldToViewTranslation_Effective.y); }
        public Vector3 WorldToViewTransformPoint(Vector3 lhs)
        { return new Vector3(lhs.x * m_WorldToViewScale_Effective.x + m_WorldToViewTranslation_Effective.x, lhs.y * m_WorldToViewScale_Effective.y + m_WorldToViewTranslation_Effective.y, 0); }

        public Vector2 ViewToWorldTransformPoint(Vector2 lhs)
        { return new Vector2((lhs.x - m_WorldToViewTranslation_Effective.x) / m_WorldToViewScale_Effective.x, (lhs.y - m_WorldToViewTranslation_Effective.y) / m_WorldToViewScale_Effective.y); }
        public Vector3 ViewToWorldTransformPoint(Vector3 lhs)
        { return new Vector3((lhs.x - m_WorldToViewTranslation_Effective.x) / m_WorldToViewScale_Effective.x, (lhs.y - m_WorldToViewTranslation_Effective.y) / m_WorldToViewScale_Effective.y, 0); }


        public Vector2 WorldToViewTransformVector(Vector2 lhs)
        { return new Vector2(lhs.x * m_WorldToViewScale_Effective.x, lhs.y * m_WorldToViewScale_Effective.y); }
        public Vector3 WorldToViewTransformVector(Vector3 lhs)
        { return new Vector3(lhs.x * m_WorldToViewScale_Effective.x, lhs.y * m_WorldToViewScale_Effective.y, 0); }

        public Vector2 ViewToWorldTransformVector(Vector2 lhs)
        { return new Vector2(lhs.x / m_WorldToViewScale_Effective.x, lhs.y / m_WorldToViewScale_Effective.y); }
        public Vector3 ViewToWorldTransformVector(Vector3 lhs)
        { return new Vector3(lhs.x / m_WorldToViewScale_Effective.x, lhs.y / m_WorldToViewScale_Effective.y, 0); }


        public Vector2 ViewInitialToWorldTransformPoint(Vector2 lhs)
        { return new Vector2((lhs.x - m_WorldToViewTranslation.x) / m_WorldToViewScale.x, (lhs.y - m_WorldToViewTranslation.y) / m_WorldToViewScale.y); }
        public Vector3 ViewInitialToWorldTransformPoint(Vector3 lhs)
        { return new Vector3((lhs.x - m_WorldToViewTranslation.x) / m_WorldToViewScale.x, (lhs.y - m_WorldToViewTranslation.y) / m_WorldToViewScale.y, 0); }

        public Vector2 mousePositionInWorld
        {
            get { return ViewToWorldTransformPoint(Event.current.mousePosition); }
        }
        public Vector2 mousePositionInView
        {
            get { return Event.current.mousePosition; }
        }
        public Vector2 worldPixelSize
        {
            get { return new Vector2(m_WorldSpace.width / m_ViewSpace.width / m_Scale.x, m_WorldSpace.height / m_ViewSpace.height / m_Scale.y); }
        }

        // Utility mouse event functions

        private bool IsResetEvent()
        {
            return (
                (Event.current.button == 2 && Event.current.control)
            );
        }

        private bool IsZoomEvent()
        {
            return (
                (Event.current.button == 1 && Event.current.alt) // right+alt drag
                                                                 //|| (Event.current.button == 0 && Event.current.command) // left+commend drag
                                                                 //|| (Event.current.button == 2 && Event.current.command) // middle+command drag

            );
        }

        private bool IsPanEvent()
        {
            return (
                (Event.current.button == 0 && Event.current.alt) // left+alt drag
                || (Event.current.button == 2 && !Event.current.command) // middle drag
            );
        }

        public ZoomArea()
        {
        }

        public void resizeView(Rect r)
        {
            mbHasViewSpaceRect = true;
            m_ViewSpace = r;
            UpdateInitialTransformation();
        }

        public void resizeWorld(Rect r)
        {
            m_WorldSpace = r;
            UpdateInitialTransformation();
        }

        private void UpdateInitialTransformation()
        {
            float aspectRatio = m_ViewSpace.width / m_ViewSpace.height;
            aspectRatio = 1;
            m_WorldToViewScale = new Vector2(m_ViewSpace.width / m_WorldSpace.width, -m_ViewSpace.height / m_WorldSpace.height * aspectRatio);
            m_WorldToViewTranslation = new Vector2(m_ViewSpace.width * 0.5f, m_ViewSpace.height * 0.5f);

            EnforceScaleAndRange();
        }

        public void BeginViewGUI()
        {
            AnimStep();
            var boxRect = new Rect(m_ViewSpace.x - 1, m_ViewSpace.y - 1, m_ViewSpace.width + 1, m_ViewSpace.height + 1);
            GUILayout.BeginArea(boxRect, styles.background);
            HandleZoomAndPanEvents(m_ViewSpace);
            GUILayout.EndArea();
        }

        public void HandleZoomAndPanEvents(Rect area)
        {
            area.x = 0;
            area.y = 0;
            int id = GUIUtility.GetControlID(zoomableAreaHash, FocusType.Passive, area);
            var miw = mousePositionInWorld;

            switch (Event.current.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    if (area.Contains(Event.current.mousePosition))
                    {
                        // Catch keyboard control when clicked inside zoomable area
                        // (used to restrict scrollwheel)
                        GUIUtility.keyboardControl = id;

                        if (IsResetEvent())
                        {
                            GUIUtility.hotControl = id;
                            Reset();
                            Event.current.Use();
                        }

                        if (IsZoomEvent() || IsPanEvent())
                        {
                            GUIUtility.hotControl = id;
                            m_WorldMouseDownPosition = mousePositionInWorld;

                            Event.current.Use();
                        }
                    }
                    break;
                case EventType.MouseUp:
                    //Debug.Log("mouse-up!");
                    if (GUIUtility.hotControl == id)
                    {
                        GUIUtility.hotControl = 0;

                        // If we got the mousedown, the mouseup is ours as well
                        // (no matter if the click was in the area or not)
                        m_WorldMouseDownPosition = new Vector2(-1000000, -1000000);
                        //Event.current.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl != id) break;

                    if (IsZoomEvent())
                    {
                        // Zoom in around mouse down position
                        Zoom(m_WorldMouseDownPosition, false);
                        Event.current.Use();
                    }
                    else if (IsPanEvent())
                    {
                        // Pan view
                        Pan();
                        Event.current.Use();
                    }
                    break;
                case EventType.ScrollWheel:
                    if (!area.Contains(Event.current.mousePosition))
                        break;

                    // Zoom in around cursor position
                    Zoom(mousePositionInWorld, true);
                    Event.current.Use();
                    break;
            }
        }

        public void EndViewGUI()
        {
        }

        public void Reset()
        {
            m_Scale = new Vector2(1, 1);
            m_Translation = new Vector2(0, 0);
            EnforceScaleAndRange();
        }

        private void Pan()
        {
            m_Translation.x += Event.current.delta.x;
            m_Translation.y += Event.current.delta.y;

            EnforceScaleAndRange();
        }

        private void Zoom(Vector2 zoomAround, bool scrollwhell)
        {
            // Get delta (from scroll wheel or mouse pad)
            // Add x and y delta to cover all cases
            // (scrool view has only y or only x when shift is pressed,
            // while mouse pad has both x and y at all times)
            float delta = Event.current.delta.x + Event.current.delta.y;

            if (scrollwhell)
                delta = -delta;

            // Scale multiplier. Don't allow scale of zero or below!
            float scale = Mathf.Max(0.01F, 1 + delta * 0.01F);

            if (!Event.current.shift)
            {
                // Offset to make zoom centered around cursor position
                m_Translation.x -= zoomAround.x * (scale - 1) * m_WorldToViewScale_Effective.x;

                // Apply zooming
                m_Scale.x *= scale;
            }
            if (!EditorGUI.actionKey)
            {
                // Offset to make zoom centered around cursor position
                m_Translation.y -= zoomAround.y * (scale - 1) * m_WorldToViewScale_Effective.y;

                // Apply zooming
                m_Scale.y *= scale;
            }

            EnforceScaleAndRange();
        }

        public void EnforceScaleAndRange()
        {
            m_Scale.x = Mathf.Max(m_Scale.x, 1);
            m_Scale.y = Mathf.Max(m_Scale.y, 1);
            float xScaleLimit = m_Scale.x - 1;
            //if (xScaleLimit > 0)
            {
                float xTransMax = m_ViewSpace.width * xScaleLimit * 0.5f;
                m_Translation.x = Mathf.Clamp(m_Translation.x, -xTransMax, xTransMax);
            }
            float yScaleLimit = m_Scale.y - 1;
            //if (yScaleLimit > 0)
            {
                float yTransMax = m_ViewSpace.height * yScaleLimit * 0.5f;
                m_Translation.y = Mathf.Clamp(m_Translation.y, -yTransMax, yTransMax);
            }

            //m_ViewToWorldScale_Effective = new Vector2(m_ViewToWorldScale.x / m_Scale.x, m_ViewToWorldScale.y / m_Scale.y);
            m_WorldToViewScale_Effective = new Vector2(m_WorldToViewScale.x * m_Scale.x, m_WorldToViewScale.y * m_Scale.y);
            m_WorldToViewTranslation_Effective = new Vector2(m_WorldToViewTranslation.x + m_Translation.x, m_WorldToViewTranslation.y + m_Translation.y);
            //m_ViewToWorldTranslation_Effective = new Vector2(m_ViewToWorldTranslation.x - m_Translation.x, m_ViewToWorldTranslation.y - m_Translation.y);

            var s = new Vector3(m_WorldToViewScale_Effective.x
                , m_WorldToViewScale_Effective.y
                , 1);
            worldToViewMatrix = Matrix4x4.TRS(m_WorldToViewTranslation_Effective, Quaternion.identity, s);
        }
    }
}
