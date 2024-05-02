using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _leftBounds;
    [SerializeField] private Transform _rightBounds;


    private void Update()
    {
        float cameraLeftBound = _camera.ViewportToWorldPoint(new Vector3(0, 0.5f, 0)).x;
        float cameraRightBound = _camera.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0)).x;

        _leftBounds.position = new Vector2(cameraLeftBound - (_leftBounds.localScale.x / 2), _leftBounds.position.y);
        _rightBounds.position = new Vector2(cameraRightBound + (_rightBounds.localScale.x / 2), _rightBounds.position.y);
    }
}
