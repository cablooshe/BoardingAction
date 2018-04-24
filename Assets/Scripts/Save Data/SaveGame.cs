using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
 
[System.Serializable]
public class SaveGame {

    public int sceneIndex;
    public int gold;
    public IList<SquadLeaderData> leaders;
    //public IList<GameObject> equipment;

    public SaveGame() {
        leaders = new List<SquadLeaderData>();
        if (PlayerInfo.Leaders != null) {
            foreach (SquadLeader l in PlayerInfo.Leaders) {
                leaders.Add(l.data);
            }
            this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            this.gold = PlayerInfo.Gold;
            //this.equipment = PlayerInfo.Equipment;
        }
    }

    public void UpdateSave() {
        leaders = new List<SquadLeaderData>();
        this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.gold = PlayerInfo.Gold;
        foreach (SquadLeader l in PlayerInfo.Leaders) {
            leaders.Add(l.data);
        }
        //this.equipment = PlayerInfo.Equipment;
    }

}