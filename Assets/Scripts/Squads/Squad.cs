using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad {

    public int squadHealth; //squad's health
    public float squadSpeed;
    public string squadName;
    public int squadDamage;
    public SquadLeader leader;
    //public List<GameObject> equipment;  //list of all equipment held by squad
    public SoldierSet soldiers;  //2 soldiers that come with the squad leader

    //constructor for initialization, to avoid errors
    public Squad() {
        squadHealth = 0;
        squadSpeed = 1;
        squadName = "";
        squadDamage = 0;
        leader = new SquadLeader();
        soldiers = new SoldierSet();
    }

    //TO DO- THIS DO IT
    public Squad(SquadLeader leader, SoldierSet set) {

    }
}
