using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [Header("Set in Inspector")]
    public int[] numObjectives; // Number of objectives per phase
	public int[] spawnsPerPhase; // These ints should sum to the length of spawns, and there should be as many of these as the number of objectives phases
	public GameObject[] spawns; // GameObjects to be spawned
    public bool isPrimary = true;

    [Header("Set Dynamically")]
    public int countComplete = 0;
	public int objectivePhase = 0;
	public int lastSpawn = 0;

    public void CompletedObjective()
    {
        countComplete++;
        if (countComplete == numObjectives[objectivePhase])
            FinishedObjectives();
    }

    private void FinishedObjectives()
    {
		objectivePhase++;
		if (objectivePhase == numObjectives.Length) {
			GameObject.Find("MainCamera").GetComponent<ObjectiveMaster>().decrement();
		} else {
			for (int i = 0; i < spawnsPerPhase [objectivePhase-1]; i++) {
				GameObject spawnedObjective = Instantiate (spawns [lastSpawn]);
				spawnedObjective.transform.SetParent (this.transform);
				lastSpawn++;
			}
			countComplete = 0;
		}
    }
}
