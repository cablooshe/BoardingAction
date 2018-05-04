using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public int sceneNum;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

   
    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void SaveGame()
    {
        Debug.Log("SAVING");
        SaveLoad.Save();
       
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(sceneNum);
    }
}
