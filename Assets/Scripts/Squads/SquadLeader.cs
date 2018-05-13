using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader {

    //all of the Squad Leader's variables are saved in a separate serializable class, so the data can be saved
    public SquadLeaderData data;


    //constructor for initialization, to avoid errors
    public SquadLeader() {
        data = new SquadLeaderData();
        data.leaderName = "";
        data.squadClass = "";
        data.level = 1;
        data.speed = 1;
        data.damage = 1;
        data.exp = 0;
    }


    public SquadLeader(string leaderName, string squadClass) {
        data = new SquadLeaderData();
        data.leaderName = leaderName;
        data.squadClass = squadClass;
        switch (data.squadClass) {
            case "Scout":
                data.speed = 3 + (1 * data.level);
                data.health = 85 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 15;  //TODO: how does weapon affect this?
                break;
            case "Heavy":
                data.speed = 1 + (1 * data.level);
                data.health = 60 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 25;  //TODO: how does weapon affect this?
                break;
            case "Balanced":
                data.speed = 2 + (1 * data.level);
                data.health = 75 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 15;  //TODO: how does weapon affect this?
                break;
            case "Sniper":
                data.speed = 2 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 30;  //TODO: how does weapon affect this?
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
