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
    public GameObject Choice05;
    public GameObject Choice06;
    public GameObject Choice07;
    public GameObject Choice08;
    public GameObject Choice09;
    public GameObject Choice10;
    public GameObject Choice11;
    public GameObject Choice12;
    public GameObject Choice13;
    public GameObject Choice14;
    public GameObject Choice15;
    public GameObject Choice16;
    public GameObject BackButton;
    public int ChoiceMade;

    public GameObject Result01;
    public GameObject Result02;
    public GameObject Result03;
    public GameObject Result04;
    public GameObject Result05;
    public GameObject Result06;
    public GameObject Result07;
    public GameObject Result08;
    public GameObject Result09;
    public GameObject Result10;
    public GameObject Result11;
    public GameObject Result12;
    public GameObject Result13;
    public GameObject Result14;
    public GameObject Result15;
    public GameObject Result16;
    public GameObject Start;



    public void ChoiceOption01 () {
        ChoiceMade = 1;
    }

    public void ChoiceOption02 () {
        ChoiceMade = 2;
    }

    public void ChoiceOption03 () {
        ChoiceMade = 3;
    }

    public void ChoiceOption04 () {
        ChoiceMade = 4;
    }

    public void ChoiceOption05() {
        ChoiceMade = 5;
    }

    public void ChoiceOption06() {
        ChoiceMade = 6;
    }

    public void ChoiceOption07() {
        ChoiceMade = 7;
    }

    public void ChoiceOption08() {
        ChoiceMade = 8;
    }

    public void ChoiceOption09() {
        ChoiceMade = 9;
    }

    public void ChoiceOption10() {
        ChoiceMade = 10;
    }

    public void ChoiceOption11() {
        ChoiceMade = 11;
    }

    public void ChoiceOption12() {
        ChoiceMade = 12;
    }

    public void ChoiceOption13() {
        ChoiceMade = 13;
    }

    public void ChoiceOption14() {
        ChoiceMade = 14;
    }

    public void ChoiceOption15() {
        ChoiceMade = 15;
    }

    public void ChoiceOption16() {
        ChoiceMade = 16;
    }

    public void ChoiceOptionBack() {
        ChoiceMade = 17;
    }

    // Update is called once per frame
    void Update () {
		if (ChoiceMade >= 1) {
            ChoiceList.SetActive(false);
        }
        if (ChoiceMade == 1) {
            Result01.SetActive(true);

        }
        if (ChoiceMade == 2) {
            Result02.SetActive(true);

        }
        if (ChoiceMade == 3) {
            Result03.SetActive(true);

        }
        if (ChoiceMade == 4) {
            Result04.SetActive(true);

        }
        if (ChoiceMade == 5) {
            Result05.SetActive(true);

        }
        if (ChoiceMade == 6) {
            Result06.SetActive(true);

        }
        if (ChoiceMade == 7) {
            Result07.SetActive(true);

        }
        if (ChoiceMade == 8) {
            Result08.SetActive(true);

        }
        if (ChoiceMade == 9) {
            Result09.SetActive(true);

        }
        if (ChoiceMade == 10) {
            Result10.SetActive(true);

        }
        if (ChoiceMade == 11) {
            Result11.SetActive(true);

        }
        if (ChoiceMade == 12) {
            Result12.SetActive(true);


        }
        if (ChoiceMade == 13) {
            Result13.SetActive(true);


        }
        if (ChoiceMade == 14) {
            Result14.SetActive(true);


        }
        if (ChoiceMade == 15) {
            Result15.SetActive(true);


        }
        if (ChoiceMade == 16) {
            Result16.SetActive(true);


        }

        if (ChoiceMade == 17) {
            Start.SetActive(true);
        }

        ChoiceMade = 0;
    }
}
