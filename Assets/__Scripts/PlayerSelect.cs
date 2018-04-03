using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour {

    public List<GameObject> unitsSelected;

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hitInfo = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "PUnit")
                {
                    if(unitsSelected.Count != 0) {
                        foreach (GameObject u in unitsSelected) {
                            u.GetComponent<PUnit>().selected = false;
                            u.GetComponent<Renderer>().material.color = Color.white;
                            foreach (Renderer r in u.GetComponentsInChildren<Renderer>()) {
                               // r.material.color = Color.white; //Set material.
                        }
                    }
                }

                unitsSelected.Clear();
                GameObject uSelected = hitInfo.collider.gameObject;
                uSelected.GetComponent<PUnit>().selected = true;
                uSelected.GetComponent<Renderer>().material.color= Color.blue;
                foreach (Renderer r in uSelected.GetComponentsInChildren<Renderer>()) {
                    //r.material.color = Color.blue; //Set material.
                }
                unitsSelected.Add(uSelected);
            }
        }

    }
}

