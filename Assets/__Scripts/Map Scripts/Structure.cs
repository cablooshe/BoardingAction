using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour {
    [Header("Set in Inspector")]
    public float durability = 50;
	public bool isCover = false;
    public bool isTemporary = false;
    public float tempLife = 5f;

    private float expireTime;

	// Use this for initialization
	void Start () {
        expireTime = Time.time + tempLife;
	}
	
	// Update is called once per frame
	void Update () {
		if (isTemporary)
        {
            if(expireTime < Time.time)
            {
                Destroy(this.gameObject);
            }
        }
	}
}
