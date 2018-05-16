using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void allDone()
    {
        print("YOU WIN!!!!");
    }
}
