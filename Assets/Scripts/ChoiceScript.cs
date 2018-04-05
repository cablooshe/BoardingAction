using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceScript : MonoBehaviour {


    public GameObject ChoiceList;
    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public GameObject Choice04;
    public int ChoiceMade;

    public GameObject Result1;
    public GameObject Result2;
    public GameObject Result3;
    public GameObject Result4;



    public void ChoiceOption1 () {
        ChoiceMade = 1;
    }

    public void ChoiceOption2 () {
        ChoiceMade = 2;
    }

    public void ChoiceOption3 () {
        ChoiceMade = 3;
    }

    public void ChoiceOption4 () {
        ChoiceMade = 4;
    }



    // Update is called once per frame
    void Update () {
		if (ChoiceMade >= 1) {
            ChoiceList.SetActive(false);
        }
        if (ChoiceMade == 1) {
            Result1.SetActive(true);

        }
        if (ChoiceMade == 2) {
            Result2.SetActive(true);

        }
        if (ChoiceMade == 3) {
            Result3.SetActive(true);

        }
        if (ChoiceMade == 4) {
            Result4.SetActive(true);

        }

        ChoiceMade = 0;
    }
}
