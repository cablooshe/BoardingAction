using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : MonoBehaviour {
    private GameObject lightSource;
    private float time = 0.4f;
    private bool isActive = false;
	// Use this for initialization
	void Start () {
        lightSource = transform.Find("Spotlight").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > time) {
            Flip();
            time += 2;
        }
	}
    void Flip() {
        lightSource.SetActive(!isActive);
        isActive = !isActive;
    }
}
