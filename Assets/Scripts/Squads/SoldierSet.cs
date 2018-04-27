using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSet : MonoBehaviour {

    public string soldierClass; //specialty of the SoldierSet
    public int health;  //float?
    public float speedMult;
    public int damage;

    public SoldierSet (string clazz, int health, float speed, int damage) {
        soldierClass = clazz;
        this.health = health;
        speedMult = speed;
        this.damage = damage;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
