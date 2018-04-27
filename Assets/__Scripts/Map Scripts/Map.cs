using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    [Header("Set in Inspector")]
    public int numObjectives;

    [Header("Set Dynamically")]
    public int countComplete = 0;

    public void CompletedObjective()
    {
        print("completed obj");
        countComplete++;
        if (countComplete == numObjectives)
            FinishedObjectives();
    }

    private void FinishedObjectives()
    {
        print("You Win!");
    }
}
