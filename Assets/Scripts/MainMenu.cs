using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame() {
        PlayerInfo.squads = new List<Squad>() {
            new Squad(),
            new Squad(),
            new Squad(),
            new Squad()
        };
        PlayerInfo.Leaders = new List<SquadLeader> {
            (new SquadLeader("Speedy", "Scout")),
            (new SquadLeader("Gordon't", "Idiot")),
            (new SquadLeader("Jake", "Bully")),
            (new SquadLeader("Leader4", "Generic"))
        };
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadGame() {
        Debug.Log("LOADING");
        SaveLoad.Load();
        PlayerInfo.currentSceneIndex = SaveLoad.SavedGame.sceneIndex;
        PlayerInfo.gold = SaveLoad.SavedGame.gold;
        PlayerInfo.Leaders = new List<SquadLeader>();
        foreach (SquadLeaderData l in SaveLoad.SavedGame.leaders) {
            PlayerInfo.Leaders.Add(new SquadLeader(l.leaderName, l.squadClass));
        }
        string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(sceneName);
    }

}
