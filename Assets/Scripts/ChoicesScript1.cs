using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicesScript1 : MonoBehaviour {

    [Header("Set in Inspector")]
    public string changedText;
    //Only for changing text of a button

    public Button buttonclicked;
    public Text textTo;
    public int ChoiceMade;

    public GameObject Result01;


    public void changeTheText()
    {
        ChoiceMade = 1;

    }

   
    // Update is called once per frame
    void Update () {
        if (ChoiceMade == 1)
        {
            buttonclicked = GetComponent<Button>();
            foreach (Transform t in transform)
            { // might not be obvious
                textTo = t.GetComponent<Text>();
            }
            buttonclicked.interactable = false;
            //buttonclicked = "Hello!";
        }

        ChoiceMade = 0;
    }
}
