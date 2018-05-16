using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightInfo : MonoBehaviour {

    public Text squadName;
    public Text HP;
	public Text ability1;
	public Text ability2;
	public Button button1;
	public Button button2;




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
		ColorBlock cb = button1.colors;
		cb.normalColor = Color.gray;
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
				ability1.text = System.Convert.ToString (leaderOne.ability1);
				if (leaderOne.ability1timestamp > Time.time) {
					cb = button1.colors;
					cb.normalColor = Color.red;
					button1.colors = cb;
				}
				ability2.text = System.Convert.ToString (leaderOne.ability2);
				if (leaderOne.ability2timestamp > Time.time) {
					cb = button2.colors;
					cb.normalColor = Color.red;
					button2.colors = cb;
				}
            } else {
                squadName.text = "No squads selected";
				HP.text = "0";
				ability1.text = "Ability 1";
				ability2.text = "Ability 2";
            }
        }
	}
}
