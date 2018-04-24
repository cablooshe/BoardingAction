using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour {

    public int totalHealth; //total health possible, should not change
    public int squadHealth; //current health
    public float squadSpeed;
    public string squadName;
    public int squadDamage;
    public SquadLeader leader;
    public List<GameObject> equipment;  //list of all equipment held by squad
    public SoldierSet soldiers;  //2 soldiers that come with the squad leader
    public int healthLimit;  //health limit goes down if you lose squad members, starts at totalHealth


    // Use this for initialization
    void Start () {
        //squadHealth = squadHealth + soldiers.health;
        //healthLimit = squadHealth;
        //squadDamage = squadDamage + soldiers.damage;
        //squadSpeed = squadSpeed * soldiers.speedMult;

    }
	
	// Update is called once per frame
	void Update () {
        if (squadHealth < 40) {  //squad does less damage with less soldiers
            healthLimit = 40;  //once a soldier dies, it cannot be revived so max health is lowered
            squadDamage -= (soldiers.damage / 2);
        } else if (squadHealth < 70) {
            healthLimit = 70;
            squadDamage = 70;
        }
    }
}
