using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSet {
	public string soldierClass; //specialty of the SoldierSet
    public int health;  //float?
    public float speedMult;
    public int damage;
	public PUnit.Ability ability1;
	public PUnit.Ability ability2;

    //constructor for initialization, to avoid errors
    public SoldierSet() {
        soldierClass = "";
        health = 1;
        speedMult = 1;
        damage = 1;
    }

    public SoldierSet(string clazz, int health, float speed, int damage) {
        soldierClass = clazz;
        this.health = health;
        speedMult = speed;
        this.damage = damage;
		ability1 = PUnit.Ability.grenade;
		ability2 = PUnit.Ability.deployCover;
    }
}
