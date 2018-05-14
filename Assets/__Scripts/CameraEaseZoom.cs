using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEaseZoom : MonoBehaviour {

    public float zoomSensitivity = 15.0f;
    public float zoomSpeed = 6.0f;
    public float zoomMin = 5.0f;
    public float zoomMax = 80.0f;

    private float zoom;

    private Camera cam;


    void Start() {
        cam = GetComponent<Camera>();
        zoom = cam.fieldOfView;
    }

    void Update() {
        zoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        zoom = Mathf.Clamp(zoom, zoomMin, zoomMax);
    }

    void LateUpdate() {
        cam = GetComponent<Camera>();
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoom, Time.deltaTime * zoomSpeed);
    }
}
