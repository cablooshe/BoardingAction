using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour {

    public Text numPrimeLeft;

    [Header("Set in Inspector")]
    public int[] numObjectives; // Number of objectives per phase
	public int[] spawnsPerPhase; // These ints should sum to the length of spawns, and there should be as many of these as the number of objectives phases
	public GameObject[] spawns; // GameObjects to be spawned
    public bool isPrimary = true;
    public Text missionObjective;   

    [Header("Set Dynamically")]
    public int countComplete = 0;
	public int objectivePhase = 0;
	public int lastSpawn = 0;

    public void CompletedObjective()
    {
        countComplete++;
        numPrimeLeft.text = System.Convert.ToString(System.Math.Abs(System.Convert.ToInt16(numPrimeLeft.text) - 1));
        Debug.Log("countComplete: " + countComplete);
        if (objectivePhase < numObjectives.Length){
            if (countComplete == numObjectives[objectivePhase]){
                Debug.Log("FinishedObjectives()");
                FinishedObjectives();
            }
        }
    }

    private void FinishedObjectives()
    {
        if (isPrimary)
        {
            objectivePhase++;

            if (objectivePhase == numObjectives.Length)
            {
                GameObject.Find("Main Camera").GetComponent<ObjectiveMaster>().decrement();
            }
            else
            {
                for (int i = 0; i < spawnsPerPhase[objectivePhase - 1]; i++)
                {
                    GameObject spawnedObjective = Instantiate(spawns[lastSpawn]);
                    spawnedObjective.transform.SetParent(this.transform);
                    lastSpawn++;
                }
                countComplete = 0;
            }
        }
         
    }
    void Start()
    {
        this.numPrimeLeft.text = System.Convert.ToString(System.Math.Abs(numObjectives[objectivePhase]));
    }
    void update()
    {
        //this.numPrimeLeft.text = System.Convert.ToString(numObjectives[objectivePhase] - countComplete);
    }
}
