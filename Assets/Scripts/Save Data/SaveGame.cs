using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
 
[System.Serializable]
public class SaveGame {

    public int sceneIndex;
    public int gold;
    public int exp;

    public SaveGame() {
        this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.gold = PlayerInfo.Gold;
        this.exp = PlayerInfo.Exp;
    }

    public void UpdateSave() {
        this.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.gold = PlayerInfo.Gold;
        this.exp = PlayerInfo.Exp;
    }

}