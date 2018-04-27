using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame() {
        PlayerInfo.CurrentSceneIndex = 2;
        PlayerInfo.Squads = new List<Squad>() {
            new Squad(),
            new Squad(),
            new Squad(),
            new Squad()
        };
        PlayerInfo.Leaders = new List<SquadLeader> {
            (new SquadLeader("Speedy", "Scout")),
            (new SquadLeader("B I G B O I", "Heavy")),
            (new SquadLeader("Average Joe", "Balanced")),
            (new SquadLeader("1v1meRust", "Sniper"))
        };
        for (int i = 0; i < PlayerInfo.Squads.Count; i++) {
            PlayerInfo.Squads[i].soldiers = new SoldierSet("Type " + i, 20, 2, 1);
            PlayerInfo.Leaders[i].data.level = 1;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadGame() {
        Debug.Log("LOADING");
        SaveLoad.Load();
        //trasnsfer all data from the save game into the PlayerInfo static script
        PlayerInfo.currentSceneIndex = SaveLoad.SavedGame.sceneIndex;
        PlayerInfo.gold = SaveLoad.SavedGame.gold;
        PlayerInfo.Leaders = new List<SquadLeader>();
        foreach (SquadLeaderData l in SaveLoad.SavedGame.leaders) {
            PlayerInfo.Leaders.Add(new SquadLeader(l.leaderName, l.squadClass));
        }
        //string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        //string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(1);
        PlayerInfo.Squads = new List<Squad>();
    }

}
