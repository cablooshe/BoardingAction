using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
 
[System.Serializable]
public class SaveGame {

    public static SaveGame current;
    public int sceneIndex;
    public int gold;
    public int exp;
    public GameObject[] soldiers;

    public SaveGame() {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        gold = PlayerInfo.Gold;
        exp = PlayerInfo.Exp;
        soldiers = PlayerInfo.Soldiers;
    }

}