using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float cameraSpeed = 10f;
    public float zoomSpeed = 30f;
    public float minZoom = 1f;
    public float maxZoom = 20f;
    

	// Use this for initialization
	void Start () {
       
 
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(cameraSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-cameraSpeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, -cameraSpeed * Time.deltaTime, 0);
            transform.Translate(move);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 move = Quaternion.Euler(-transform.rotation.eulerAngles.x, 0, 0) * new Vector3(0, cameraSpeed * Time.deltaTime, 0);
            transform.Translate(move);
        }

        var d = Input.GetAxis("Mouse ScrollWheel");

        if(d > 0f)
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
        else if (d < 0f) {
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
