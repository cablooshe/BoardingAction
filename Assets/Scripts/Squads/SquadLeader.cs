﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquadLeader {

    //all of the Squad Leader's variables are saved in a separate serializable class, so the data can be saved
    public SquadLeaderData data;


    //constructor for initialization, to avoid errors
    public SquadLeader() {
        data = new SquadLeaderData();
        data.leaderName = "";
        data.squadClass = "";
        data.level = 1;
        data.speed = 1.0f;
        data.damage = 1;
        data.exp = 0;
    }


    public SquadLeader(string leaderName, string squadClass) {
        data = new SquadLeaderData();
        data.leaderName = leaderName;
        data.squadClass = squadClass;
      //  data.icon = icon;
        switch (data.squadClass) {
        case "Scout":
			data.speed = 1.8f + (data.level / 2);
            data.health = 85 + (10 * data.level);
            //TODO: Equip Weapon
            data.damage = 15;  //TODO: how does weapon affect this?
            break;
        case "Heavy":
			data.speed = 1.0f + (data.level / 10);
            data.health = 100 + (10 * data.level);
            //TODO: Equip Weapon
            data.damage = 45;  //TODO: how does weapon affect this?
            break;
        case "Balanced":
			data.speed = 1.4f + (data.level / 4);
            data.health = 75 + (10 * data.level);
            //TODO: Equip Weapon
            data.damage = 15;  //TODO: how does weapon affect this?
            break;
		case "Sniper":
			data.speed = 1.4f + (data.level / 4);
            data.health = 80 + (10 * data.level);
            //TODO: Equip Weapon
            data.damage = 30;  //TODO: how does weapon affect this?
            break;
		case "Recruit":
			data.speed = 1.3f + (data.level / 4);
			data.health = 60 + (10 * data.level);
			data.damage = 14;
			break;
		case "Pacifist":
			data.speed = 1f + (data.level / 10);
			data.health = 500 + (100 * data.level);
			data.damage = 3;
			break;
		case "Gamer":
			data.speed = 1.75f + (data.level / 2);
			data.health = 50 + (5 * data.level);
			data.damage = 10;
			break;
		case "Public Speaker":
			data.speed = 1f;
			data.health = 50 + (5 * data.level);
			data.damage = Random.Range(1,30);
			break;
        default:
            break;
        }
        this.data.level = 1;
        //this.data.damage = (int)(Random.value * 10);
    }


    // Update is called once per frame
    void Update() {

    }
}
