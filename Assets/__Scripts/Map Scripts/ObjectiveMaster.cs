using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ObjectiveMaster : MonoBehaviour
{

    public Text numPrimeLeft;
    public Text numSecondLeft;
    public Text primeText;
    public Text secondText;

    [Header("Set in Inspector")]
    public float numPrimeObjectiveHandlers;
    public float numSecondObjectiveHandlers;


    public void decrement()
    {
        /*
        Debug.Log("DECREMENT PRIME");
        Debug.Log(System.Convert.ToString(numPrimeObjectiveHandlers));

        if (numPrimeObjectiveHandlers - 1 >= 0)
        {
            numPrimeLeft.text = System.Convert.ToString(numPrimeObjectiveHandlers);
        }
        */
        if (numPrimeObjectiveHandlers-- <= 0)
        {
            allDone();
        }
    }
    
    public void decrementSecond()
    {
        Debug.Log("DECREMENT SECONDARY");

        if (numSecondObjectiveHandlers-- <= 0)
        {
            partDone();
        }
    }

    public void FinishMission()
    {
        string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(sceneName);
    }
    public void allDone()
    {
        print("YOU WIN!!!!");

        PlayerInfo.currentSceneIndex++;
        FinishMission();
        PlayerInfo.currentSceneIndex++;

    }

    public void partDone()
    {
        print("Seconday Objectives complete!");
    }
    /*
    void Start()
    {
        this.numPrimeLeft.text = System.Convert.ToString(numPrimeObjectiveHandlers);
        this.numSecondLeft.text = System.Convert.ToString(numSecondObjectiveHandlers);
        this.primeText.text = "Kill Enemies";
        this.secondText.text = "Collect Samples";
    }
    void update()
    {
        this.numPrimeLeft.text = System.Convert.ToString(numPrimeObjectiveHandlers);
        this.numSecondLeft.text = System.Convert.ToString(numSecondObjectiveHandlers);
    }*/
}
