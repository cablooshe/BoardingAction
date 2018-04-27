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
                data.speed = 20 + (10 * data.level);
                data.health = 80 + (10 * data.level);
                //TODO: Equip Weapon
                data.damage = 50;  //TODO: how does weapon affect this?
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
