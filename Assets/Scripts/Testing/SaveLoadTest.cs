using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Starting Gold: " + PlayerInfo.Gold);
        print("Increasing gold by 500 then saving...");
        //this will fail on load because it is pulling from Squads, which are not set directly on load (ask about this)
        Debug.Log("Squad Leaders in your Squads array: " + PlayerInfo.Squads[0].leader.data.leaderName + ", " + PlayerInfo.Squads[1].leader.data.leaderName + ", " + PlayerInfo.Squads[2].leader.data.leaderName + ", " + PlayerInfo.Squads[3].leader.data.leaderName + ", ");
	}
	
	// Update is called once per frame
	void Update () {
        PlayerInfo.Gold = (PlayerInfo.Gold + 1);
        if (PlayerInfo.Gold % 500 == 0) {
            print("Game Saved!");
            SaveLoad.Save();
        }
	}
}
