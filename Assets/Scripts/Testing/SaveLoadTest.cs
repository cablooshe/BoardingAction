using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print("Starting Gold: " + PlayerInfo.Gold);
        print("Increasing gold by 500 then saving...");
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
