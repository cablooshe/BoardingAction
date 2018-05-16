using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightInfo : MonoBehaviour {

    public Text squadName;
    public Text HP;
    public Text Action1;
    public Text Action2;




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
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
                Action1.text = abilityText1;
                Action2.text = abilityText2;


            } else {
                squadName.text = "";
            }
        }
	}
}
