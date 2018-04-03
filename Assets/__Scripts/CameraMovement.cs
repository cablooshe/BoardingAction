using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public float speed = .5f;
	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 move = Quaternion.Euler(30, 0, 0) * new Vector3(0, -speed * Time.deltaTime, 0);
            print("down " + move);
            transform.Translate(move);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 move = Quaternion.Euler(30, 0, 0) * new Vector3(0, speed * Time.deltaTime, 0);
            print("up " + move);
            transform.Translate(move);
        }
    }
}
