using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class NewBehaviourScript : MonoBehaviour {

    public Text squadAbilities = null;
    public void changeAbilitiesList (string word)
    {
        squadAbilities.text = word;
    }
	
}
