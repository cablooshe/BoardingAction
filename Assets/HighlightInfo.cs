using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightInfo : MonoBehaviour {

    public Text squadName;
    public Text HP;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerSelect.unitsSelected.Count > 0) {
            squadName.text = PlayerSelect.unitsSelected[0].name;
            HP.text = PlayerSelect.unitsSelected[0].name;
            Debug.Log(HP.text);
        } else {
            squadName.text = "";
        }
	}
}
