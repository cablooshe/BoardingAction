using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C4 : MonoBehaviour {
    private GameObject lightSource;
    private float time = 1f;
    private float timeInc = 1f;
    private bool isActive = false;
	// Use this for initialization
	void Start () {
        lightSource = transform.Find("Spotlight").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > time) {
            Flip();
            time += timeInc;
        }
	}
    void Flip() {
        lightSource.SetActive(!isActive);
        isActive = !isActive;
    }
}
