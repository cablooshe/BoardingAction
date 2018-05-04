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
        if (PlayerSelect.units != null) {
            if (PlayerSelect.units.Count > 0) {
                squadName.text = System.Convert.ToString(PlayerSelect.units[0].GetComponent<PUnit>().name);
                HP.text = System.Convert.ToString(PlayerSelect.units[0].GetComponent<PUnit>().currentHealth);
                Debug.Log(HP.text);
            } else {
                squadName.text = "";
            }
        }
	}
}
