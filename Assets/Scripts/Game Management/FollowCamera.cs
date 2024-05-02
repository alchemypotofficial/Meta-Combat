using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private const float CAMERA_RESIZE_STEP = 0.005f;

    private const float LEFT_BOUND_OUTER = 0.04f;
    private const float LEFT_BOUND_INNER = LEFT_BOUND_OUTER + 0.001f;
    private const float RIGHT_BOUND_OUTER = 1 - LEFT_BOUND_OUTER;
    private const float RIGHT_BOUND_INNER = RIGHT_BOUND_OUTER - 0.001f;

    [Header("Follow Targets")]
    public Transform transform1;
    public Transform transform2;

    [Header("Camera Bounds")]
    [SerializeField] private Transform _leftCollider;
    [SerializeField] private Transform _rightCollider;

    [Header("Options")]
    [SerializeField] private float _minSize = 2f;
    [SerializeField] private float _maxSize = 3.2f;
    [SerializeField] private float _yOffset = 0.8f;

    // Set once
    private Camera _camera;

    // dynamic
    private Vector2 _targetWorldPos;
    private Vector2 _leftWorldPos;
    private Vector2 _rightWorldPos;
    private Vector2 _leftViewPos;
    private Vector2 _rightViewPos;
    private float _yCurrOffset;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        InitCameraParams();
    }

    private void Start()
    {
        UpdatePositions();
        InitCameraPosition();
    }

    private void LateUpdate()
    {
        UpdatePositions();
        CenterCameraPosition();
        UpdateCameraBounds();
    }

    private void UpdatePositions()
    {
        if (transform1.position.x  < transform2.position.x)
        {
            // Player is on the left of opponent
            _leftWorldPos = transform1.position;
            _rightWorldPos = transform2.position;
        }
        else
        {
            // Player is on the right of opponent
            _leftWorldPos = transform2.position;
            _rightWorldPos = transform1.position;
        }

        if (_leftWorldPos.x < _leftCollider.position.x) { _leftWorldPos.x = _leftCollider.position.x; }
        if (_rightWorldPos.x > _rightCollider.position.x) { _rightWorldPos.x = _rightCollider.position.x; }

        _targetWorldPos = Vector3.Lerp(_leftWorldPos, _rightWorldPos, 0.5f);
        
        // Set view positions
        _leftViewPos = _camera.WorldToViewportPoint(_leftWorldPos);
        _rightViewPos = _camera.WorldToViewportPoint(_rightWorldPos);
    }

    private int UpdateCameraBounds()
    {
        //RETURN: -1 = shrink, 1 = grow, 0 = none
        // viewport starts bottom-left at (0,0) to top-right at (1,1)
        // Increase the size of the camera
        if (_leftViewPos.x < LEFT_BOUND_OUTER || _rightViewPos.x > RIGHT_BOUND_OUTER)
        {
            // some character is on the very left side of the screen
            if (_camera.orthographicSize < _maxSize)
            {
                _camera.orthographicSize += CAMERA_RESIZE_STEP;
                _yCurrOffset += CAMERA_RESIZE_STEP;
                return 1;
            }
        }
        // Decrease the size of the camera
        else if (_leftViewPos.x > LEFT_BOUND_INNER || _rightViewPos.x < RIGHT_BOUND_INNER)
        {
            if (_camera.orthographicSize > _minSize)
            {
                _camera.orthographicSize -= CAMERA_RESIZE_STEP;
                _yCurrOffset -= CAMERA_RESIZE_STEP;
                return -1;
            }
        }
        return 0;
    }

    private void InitCameraParams()
    {
        /* call first on load after camera init */

        _camera.orthographicSize = _minSize;
        _yCurrOffset = _yOffset;
    }

    private void InitCameraPosition()
    {
        /* assumes InitCameraParams has been called prior */

        Vector3 targetPosition = new Vector3(_targetWorldPos.x, _targetWorldPos.y + _yCurrOffset, this.transform.position.z);
        this.transform.position = targetPosition;
    }

    private void CenterCameraPosition()
    {
        // center camera on target
        Vector3 _targetPosition = new Vector3(_targetWorldPos.x, _targetWorldPos.y + _yCurrOffset, this.transform.position.z);
        if (_targetPosition.x < _leftCollider.position.x + _camera.orthographicSize) { _targetPosition.x = _leftCollider.position.x + _camera.orthographicSize; }
        else if (_targetPosition.x > _rightCollider.position.x - _camera.orthographicSize) { _targetPosition.x = _rightCollider.position.x - _camera.orthographicSize; }

        this.transform.position = _targetPosition;
    }
}
