﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighlightInfo : MonoBehaviour {

    public static int SquadCount  = 0;

    public Text squadName;
    public Text HP;

	public Text ability1;
	public Text ability2;
	public Button button1;
	public Button button2;

    // Use this for initialization
    void Awake() {
        //SquadCount = PlayerInfo.Squads.Count;
       // print(SquadCount);
    }

    // Update is called once per frame
    void Update() {
        if (SquadCount == 0) {
            SceneManager.LoadScene("LoseScreen");
            return;
        }

        List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
        ColorBlock cb = button1.colors;
        cb.normalColor = Color.white;
        button1.colors = cb;
        button2.colors = cb;
        if (selected != null && selected.Count > 0 && selected[0] != null) {
            PUnit leaderOne = selected[0].GetComponent<PUnit>();
            //  Debug.Log(selected.Count);
            // Debug.Log("spot 1");
            if (selected.Count > 0) {
                //    Debug.Log("spot 2");

                // Debug.Log(System.Convert.ToString(selected[0].GetComponent<PUnit>().name));
                //Debug.Log(System.Convert.ToString(selected[0].GetComponent<PUnit>().currentHealth));

                squadName.text = System.Convert.ToString(leaderOne.name);
                HP.text = System.Convert.ToString(leaderOne.currentHealth);

                string abilityText1 = System.Convert.ToString(leaderOne.ability1);
                string abilityText2 = System.Convert.ToString(leaderOne.ability2);
                if (abilityText1 == "heal") {
                    abilityText1 += ("\n\ncharges left: " + System.Convert.ToString(leaderOne.numHealsLeft));
                }
                if (abilityText2 == "heal") {
                    abilityText2 += ("\n\ncharges left: " + System.Convert.ToString(leaderOne.numHealsLeft));
                }
				ability1.text = abilityText1;
				if (leaderOne.ability1timestamp > Time.time) {
					cb = button1.colors;
					cb.normalColor = Color.red;
					button1.colors = cb;
				}
				ability2.text = abilityText2;
				if (leaderOne.ability2timestamp > Time.time) {
					cb = button2.colors;
					cb.normalColor = Color.red;
					button2.colors = cb;
				}
                //button1.onClick.AddListener(leaderOne.useAbility1);
                //button2.onClick.AddListener(leaderOne.useAbility2);
            } else {
                squadName.text = "No squads selected";
                HP.text = "0";
                ability1.text = "Ability 1";
                ability2.text = "Ability 2";
            }
        }
    }

    public void DoAbility1() {
        List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
        PUnit leaderOne = selected[0].GetComponent<PUnit>();
        leaderOne.useAbility1();
    }

    public void DoAbility2() {
        List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
        PUnit leaderOne = selected[0].GetComponent<PUnit>();
        leaderOne.useAbility2();
    }
}
