using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
 
[System.Serializable]
public class SaveGame {

    public int sceneIndex;
    public int gold;
    public IList<SquadLeader> leaders;
    public IList<GameObject> equipment;

    public SaveGame() {
        this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.gold = PlayerInfo.Gold;
        this.leaders = PlayerInfo.Leaders;
        this.equipment = PlayerInfo.Equipment;
    }

    public void UpdateSave() {
        this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.gold = PlayerInfo.Gold;
        this.leaders = PlayerInfo.Leaders;
        this.equipment = PlayerInfo.Equipment;
    }

}