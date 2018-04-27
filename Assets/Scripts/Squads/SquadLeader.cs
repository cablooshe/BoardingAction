using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : MonoBehaviour {

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
            case "Idiot":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            case "Bully":
                data.speed = 3 + (1 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 20;  //TODO: how does weapon affect this?
                break;
            case "Generic":
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
