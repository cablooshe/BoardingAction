using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {

    public GameObject unitSelected;

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
                    foreach (Renderer r in unitSelected.GetComponentsInChildren<Renderer>()) {
                    r.material.color = Color.white; //Set material.
                    }
                }

                unitSelected = hitInfo.collider.gameObject;
                unitSelected.GetComponent<PUnit>().selected = true;
                unitSelected.GetComponent<Renderer>().material.color= Color.blue;
                foreach (Renderer r in unitSelected.GetComponentsInChildren<Renderer>()) {
                    r.material.color = Color.blue; //Set material.
                }
            }
        }

    }
}

