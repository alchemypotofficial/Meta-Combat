using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHandler : Singleton<CameraHandler>
{
    private const float CAMERA_RESIZE_STEP = 0.005f;

    private const float LEFT_BOUND_OUTER = 0.04f;
    private const float LEFT_BOUND_INNER = LEFT_BOUND_OUTER + 0.001f;
    private const float RIGHT_BOUND_OUTER = 1 - LEFT_BOUND_OUTER;
    private const float RIGHT_BOUND_INNER = RIGHT_BOUND_OUTER - 0.001f;

    [SerializeField] public Color letterBoxColor = new Color(0, 0, 0, 1);
    [SerializeField] public Vector2 aspectRatio;
    [SerializeField] public float maxSize = 3.2f;
    [SerializeField] public float minSize = 2f;
    [SerializeField] public bool followCamera;
    [SerializeField] public bool sizeCamera;

    public Transform transform1;
    public Transform transform2;

    private Camera aspectCamera;
    private Camera letterBoxCamera;

    private Vector3 targetPosition;
    private Vector3 leftPosition;
    private Vector3 rightPosition;

    private void Start()
    {
        aspectCamera = GetComponent<Camera>();
        CreateLetterBoxCamera();

        if (sizeCamera)
        {
            PerformSizing();
        }
    }

    private void Update()
    {
        if (sizeCamera)
        {
            PerformSizing();
        }

        UpdateCameraPosition();
    }

    private void OnValidate()
    {
        float x = Mathf.Max(1, aspectRatio.x);
        float y = Mathf.Max(1, aspectRatio.y);

        aspectRatio = new Vector2(x, y);
    }

    private void CreateLetterBoxCamera()
    {
        Camera[] cameras = FindObjectsOfType<Camera>();
        foreach (Camera camera in cameras)
        {
            if (camera.depth == -100)
            {
                Log.Warning("Found \"" + camera.name + "\" with a depth of \"-100\". Will cause letter boxing issues. Please increase its depth.");
            }
        }

        letterBoxCamera = new GameObject("Letter Box").AddComponent<Camera>();
        letterBoxCamera.transform.parent = aspectCamera.transform;
        letterBoxCamera.backgroundColor = letterBoxColor;
        letterBoxCamera.cullingMask = 0;
        letterBoxCamera.depth = -100;
        letterBoxCamera.farClipPlane = 1;
        letterBoxCamera.useOcclusionCulling = false;
        letterBoxCamera.allowHDR = false;
        letterBoxCamera.allowMSAA = false;
        letterBoxCamera.clearFlags = CameraClearFlags.Color;
    }

    private void PerformSizing()
    {
        float targetRatio = aspectRatio.x / aspectRatio.y;
        float windowaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = windowaspect / targetRatio;

        if (scaleheight < 1.0f)
        {
            Rect rect = aspectCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            aspectCamera.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = aspectCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            aspectCamera.rect = rect;
        }
    }

    private void UpdateCameraPosition()
    {
        if (transform1 != null && transform2 != null)
        {
            if (transform1.position.x < transform2.position.x)
            {
                leftPosition = transform1.position;
                rightPosition = transform2.position;
            }
            else
            {
                leftPosition = transform2.position;
                rightPosition = transform1.position;
            }

            float height = 2f * aspectCamera.orthographicSize;
            float width = height * aspectCamera.aspect;

            targetPosition.x = (leftPosition.x + rightPosition.x) / 2;
            targetPosition.y = ((leftPosition.y + rightPosition.y) / 2) + 0.75f;

            if (targetPosition.x - (width / 2) < -8.5f)
            {
                targetPosition.x = -8.5f + (width / 2);
            }
            else if (targetPosition.x + (width / 2) > 8.5f)
            {
                targetPosition.x = 8.5f - (width / 2);
            }

            if (targetPosition.y - (height / 2) < -5f)
            {
                targetPosition.y = -5f + (height / 2);
            }
            else if (targetPosition.y + (height / 2) > 5f)
            {
                targetPosition.y = 5f - (height / 2);
            }

            Vector3 position = new Vector3(targetPosition.x, targetPosition.y, this.transform.position.z);
            this.transform.position = position;
        }
        else
        {
            Vector3 position = new Vector3(0f, 0f, this.transform.position.z);
            this.transform.position = position;
        }
    }

    public static float GetLeftBorder()
    {
        float height = 2f * instance.aspectCamera.orthographicSize;
        float width = height * instance.aspectCamera.aspect;

        return instance.targetPosition.x - (width / 2);
    }

    public static float GetRightBorder()
    {
        float height = 2f * instance.aspectCamera.orthographicSize;
        float width = height * instance.aspectCamera.aspect;

        return instance.targetPosition.x + (width / 2);
    }
}