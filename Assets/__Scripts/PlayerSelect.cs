using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {

    public GameObject unitSelected;
    
	// Use this for initialization
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "PUnit")
                {
                    if(unitSelected != null) {
                        unitSelected.GetComponent<PUnit>().selected = false;
                    unitSelected.GetComponent<Renderer>().material.color = Color.white;

                }

                unitSelected = hitInfo.collider.gameObject;
                    unitSelected.GetComponent<PUnit>().selected = true;
                unitSelected.GetComponent<Renderer>().material.color= Color.blue;
                }
        }

    }
}

