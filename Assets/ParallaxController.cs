using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    [SerializeField] private float parallax = 0.1f;

    private GameObject aspectCamera;
    private float startPosition;

    private void Start()
    {
        aspectCamera = GameObject.Find("Camera");
        startPosition = transform.position.x;
    }

    private void Update()
    {
        float distance = aspectCamera.transform.position.x * parallax;
        transform.position = new Vector3(startPosition + distance, transform.position.y, transform.position.z);
    }

}
