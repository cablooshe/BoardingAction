﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonTextSwitcher : MonoBehaviour {

    public Button Slot1;
    public Button Slot2;
    public Button Slot3;
    public Button Slot4;
	public Text squadAbilities;
    public int CurrentSlot;
    

    public GameObject Choice01;
    public GameObject Choice02;
    public GameObject Choice03;
    public GameObject Choice04;
    public GameObject Choice05;
    public GameObject Choice06;
    public GameObject Choice07;
    public GameObject Choice08;
    public int ChoiceMade;
    public bool isLeaderScript;
    public bool disableOption;
    public bool deactivateOption;
	public bool actuallyDoesWhatTheScriptNameSuggests;
	public bool onlyOneButton;

    public List<Button> slots;
    public List<GameObject> choices;


    public void SlotChoice01() {
        CurrentSlot = 1;
    }
    public void SlotChoice02() {
        CurrentSlot = 2;
    }
    public void SlotChoice03() {
        CurrentSlot = 3;
    }
    public void SlotChoice04() {
        CurrentSlot = 4;
    }

    public void StartMission() {
        string path = SceneUtility.GetScenePathByBuildIndex(PlayerInfo.currentSceneIndex);
        string sceneName = path.Substring(0, path.Length - 6).Substring(path.LastIndexOf('/') + 1);
        SceneManager.LoadScene(sceneName);
    }


    public void ChoiceOption01() {
        ChoiceMade = 1;
    }
    public void ChoiceOption02() {
        ChoiceMade = 2;
    }
    public void ChoiceOption03() {
        ChoiceMade = 3;
    }
    public void ChoiceOption04() {
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

    public void ChangeText(int chosenLeader) {
        
        Debug.Log("ChoiceMade: " + ChoiceMade);
		if (actuallyDoesWhatTheScriptNameSuggests)
			slots [CurrentSlot - 1].GetComponentInChildren<Text> ().text = choices [0].GetComponentInChildren<Text> ().text;
		else if (onlyOneButton)
			slots [0].GetComponentInChildren<Text> ().text = choices [chosenLeader - 1].GetComponentInChildren<Text> ().text;
		else
			slots[CurrentSlot - 1].GetComponentInChildren<Text>().text = choices[chosenLeader - 1].GetComponentInChildren<Text>().text;
        if (deactivateOption) {
            slots[CurrentSlot - 1].interactable = false;
        }
        if (disableOption == true) {
            choices[chosenLeader - 1].SetActive(false);
        }
		if (actuallyDoesWhatTheScriptNameSuggests)
			return;
        if (isLeaderScript) {
            PlayerInfo.Squads[CurrentSlot - 1].leader = PlayerInfo.Leaders[ChoiceMade - 1];
        } else {
            SoldierSet sol = new SoldierSet();
            squadAbilities.text = "";
            switch (ChoiceMade) {
                case 1:
                    sol = new SoldierSet("Sneaky", 10, 1.5f, 2);
                    squadAbilities.text = "Abilities: Healing, Take more Damage, and C4.";
                    break;
                case 2:
                    sol = new SoldierSet("Bombastic", 10, 1.0f, 4);
                    squadAbilities.text = "Abilities: Grenades, C4, and Enrage Mode.";
                    break;
                case 3:
                    sol = new SoldierSet("Nerdy", 20, 1.25f, 2);
                    squadAbilities.text = "Abilities: Deployable Cover, Take more Damage, and Heal.";
                    break;
                case 4:
                    sol = new SoldierSet("Tanky", 40, 1.0f, 1);
                    squadAbilities.text = "Abiltiies: Take more Damage, Deployable Cover, and Grenades.";
                    break;
                case 5:
                    sol = new SoldierSet("Assault-y", 10, 1.25f, 3);
                    squadAbilities.text = "Abilities: Enrage Mode, Deployable Cover, and Heal.";
                    break;
                default:
                    squadAbilities.text = "Please choose a squad.";
                    break;

            }
            PlayerInfo.Squads[CurrentSlot - 1].soldiers = sol;
			print (CurrentSlot);
        }
    }

    private void Start() {
        if (isLeaderScript) {
            Choice01.GetComponentInChildren<Text>().text = PlayerInfo.Leaders[0].data.leaderName;
            Choice02.GetComponentInChildren<Text>().text = PlayerInfo.Leaders[1].data.leaderName;
            Choice03.GetComponentInChildren<Text>().text = PlayerInfo.Leaders[2].data.leaderName;
            Choice04.GetComponentInChildren<Text>().text = PlayerInfo.Leaders[3].data.leaderName;
            
        }

        /*
        slots.Add(Slot1);
        slots.Add(Slot2);
        slots.Add(Slot3);
        slots.Add(Slot4);

        choices.Add(Choice01);
        choices.Add(Choice02);
        choices.Add(Choice03);
        choices.Add(Choice04);
        choices.Add(Choice05);
        choices.Add(Choice06);
        choices.Add(Choice07);
        choices.Add(Choice08);
        */

    }

    // Update is called once per frame
    void Update () {
        if (CurrentSlot != 0 && ChoiceMade != 0) {
            ChangeText(ChoiceMade);
            CurrentSlot = 0;
            ChoiceMade = 0;
        }
    }
}
