using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadGame() {
        Debug.Log("LOADING");
        SaveLoad.Load();
        PlayerInfo.currentSceneIndex = SaveLoad.savedGame.sceneIndex;
        PlayerInfo.gold = SaveLoad.savedGame.gold;
        PlayerInfo.exp = SaveLoad.savedGame.exp;
        string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(sceneName);
    }

}
