using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : MonoBehaviour {

    //all of the Squad Leader's variables are saved in a separate serializable class, so the data can be saved
    public SquadLeaderData data;


    public SquadLeader (string leaderName, string squadClass) {
        data = new SquadLeaderData();
        data.leaderName = leaderName;
        data.squadClass = squadClass;
        switch (data.squadClass) {
            case "Scout":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            case "Heavy":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            case "Balanced":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            case "Sniper":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            default:
                break;
        } 
        this.data.damage = (int)(Random.value * 10);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
