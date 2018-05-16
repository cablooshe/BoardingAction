using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectiveMaster : MonoBehaviour {

    [Header("Set in Inspector")]
    public float numObjectiveHandlers;

    public void decrement()
    {
        if (numObjectiveHandlers-- <= 0)
        {
            allDone();
        }
    }

    public void StartMission()
    {
        string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(sceneName);
    }
    public void allDone()
    {
        print("YOU WIN!!!!");
        PlayerInfo.currentSceneIndex++;
        StartMission();
        
    }
}
