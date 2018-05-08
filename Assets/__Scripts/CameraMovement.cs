using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float cameraSpeed = 10f;
    public float zoomSpeed = 30f;
    public float minZoom = 1f;
    public float maxZoom = 50f;
    

	// Use this for initialization
	void Start () {
       
 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(cameraSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-cameraSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, -cameraSpeed * Time.deltaTime, 0);
            transform.Translate(move);
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, cameraSpeed * Time.deltaTime, 0);
            transform.Translate(move);
        }

        var d = Input.GetAxis("Mouse ScrollWheel");

        if(d > 0f || Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
        {
            //Scroll Up
       
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, 0, zoomSpeed * Time.deltaTime);
            transform.transform.Translate(move);

            if(transform.position.z > -1 * minZoom)  
            {
                move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, 0, -zoomSpeed * Time.deltaTime);
                transform.transform.Translate(move);
            }
        }
        else if (d < 0f || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) ) {
            //Scroll Down
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, 0, -zoomSpeed * Time.deltaTime);
            transform.transform.Translate(move);
            if(transform.position.z < -1 * maxZoom)
            {
                move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, 0, zoomSpeed * Time.deltaTime);
                transform.transform.Translate(move);
            }
        }


    }
}
