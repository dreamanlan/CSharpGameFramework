using UnityEngine;
using UnityEngine.UI;

public class AutoFitSceneViewer : MonoBehaviour
{
    public Transform target;
    public Camera captureCamera;
    public RenderTexture renderTexture;

    public int imgLeft = 10;
    public int imgTop = 50;
    public int rtWidth = 256;
    public int rtHeight = 256;
    public float refreshInterval = 0.1f;

    public float distToTarget = 100f;
    public Vector3 direction = new Vector3(0, -1, 1);

    private RawImage rawImage;
    private float lastRefreshTime;

    public void Setup(int left, int top, int width, int height, float interval)
    {
        if (rtWidth != width || rtHeight != height) {
            rtWidth = width;
            rtHeight = height;

            if (null != renderTexture) {
                renderTexture.Release();
                renderTexture = new RenderTexture(rtWidth, rtHeight, 16, RenderTextureFormat.ARGB32);
                renderTexture.Create();
            }
            if (null != captureCamera) {
                captureCamera.targetTexture = renderTexture;
            }
            if (null != rawImage) {
                rawImage.texture = renderTexture;

                RectTransform rt = rawImage.rectTransform;
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.sizeDelta = new Vector2(rtWidth, rtHeight);
                rt.anchoredPosition = new Vector2(imgLeft, -1 * imgTop);
            }
        }
        else if (left != imgLeft || top != imgTop) {
            imgLeft = left;
            imgTop = top;
            if (null != rawImage) {
                rawImage.texture = renderTexture;

                RectTransform rt = rawImage.rectTransform;
                rt.anchorMin = new Vector2(0, 1);
                rt.anchorMax = new Vector2(0, 1);
                rt.pivot = new Vector2(0, 1);
                rt.sizeDelta = new Vector2(rtWidth, rtHeight);
                rt.anchoredPosition = new Vector2(imgLeft, -1 * imgTop);
            }
        }
        refreshInterval = interval;
    }
    public void Refresh()
    {
        if (null == target || null == captureCamera) {
            return;
        }
        CameraLookAt(captureCamera, distToTarget, direction, target.transform.position);
    }

    void Start()
    {
        if (captureCamera == null) {
            GameObject camGO = gameObject;
            captureCamera = camGO.AddComponent<Camera>();
        }

        renderTexture = new RenderTexture(rtWidth, rtHeight, 16, RenderTextureFormat.ARGB32);
        renderTexture.Create();

        captureCamera.targetTexture = renderTexture;

        SetupUI();
        lastRefreshTime = Time.time;
    }

    void Update()
    {
        float curTime = Time.time;
        if (curTime - lastRefreshTime > refreshInterval) {
            Refresh();
            lastRefreshTime = curTime;
        }
    }

    void CameraLookAt(Camera cam, float distance, Vector3 direction, Vector3 to)
    {
        cam.transform.position = to - direction * distance;
        cam.transform.LookAt(to);

        float near = distance * 0.1f;
        float far = distance * 5f;
        cam.nearClipPlane = near < 0.1f ? near : 0.1f;
        cam.farClipPlane = far > 3000f ? far : 3000f;
    }

    void SetupUI()
    {
        Canvas canvas = null;
        var gmObj = GameObject.Find("GmScript");
        if (null != gmObj) {
            canvas = gmObj.GetComponentInChildren<Canvas>();
        }
        if (canvas == null) {
            GameObject canvasGO = new GameObject("SceneViewCanvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        GameObject rawImageGO = new GameObject("SceneCaptureRawImage");
        rawImageGO.transform.SetParent(canvas.transform, false);

        rawImage = rawImageGO.AddComponent<RawImage>();
        rawImage.texture = renderTexture;

        RectTransform rt = rawImage.rectTransform;
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.sizeDelta = new Vector2(rtWidth, rtHeight);
        rt.anchoredPosition = new Vector2(imgLeft, -1 * imgTop);
    }

    void OnDestroy()
    {
        if (renderTexture != null) {
            renderTexture.Release();
            Destroy(renderTexture);
        }
        if (captureCamera != null) {
            Destroy(captureCamera.gameObject);
        }
        if (rawImage != null) {
            Destroy(rawImage.gameObject);
        }
    }
}