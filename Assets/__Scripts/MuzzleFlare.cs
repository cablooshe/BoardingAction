using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlare : MonoBehaviour {

    public GameObject S;
    private float deathTime = 0.5f;
    private float startTime;
	// Use this for initialization
	void Start () {
        S = this.gameObject;
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time - startTime >= deathTime){
            Destroy(S);
        }
	}
}
