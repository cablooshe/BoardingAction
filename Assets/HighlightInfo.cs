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
        /*List<GameObject> selected = this.gameObject.GetComponent<PlayerSelect>().unitsSelected;
        if (selected != null) {
            Debug.Log(selected.Count);
            Debug.Log("spot 1");
            if (selected.Count > 0) {
                Debug.Log("spot 2");

                Debug.Log(System.Convert.ToString(selected[0].GetComponent<PUnit>().name));
                Debug.Log(System.Convert.ToString(selected[0].GetComponent<PUnit>().currentHealth));

                squadName.text = System.Convert.ToString(selected[0].GetComponent<PUnit>().name);
                HP.text = System.Convert.ToString(selected[0].GetComponent<PUnit>().currentHealth);

            } else {
                squadName.text = "";
            }
        }*/
	}
}
