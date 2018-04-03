using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHalo : MonoBehaviour {
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;
    public Color haloColor = Color.yellow;

    [Header("Set Dynamically")]
    public bool visible = false;

    Material mat;

	// Use this for initialization
	void Start () {
        mat = GetComponent<Renderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
	}
}
