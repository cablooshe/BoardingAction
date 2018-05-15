using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetup : MonoBehaviour {
    public List<Vector3> spawnLoc;
    public GameObject unitPrefab;
    public List<GameObject> units;

    public int squadPressed = 0;

    //To update the squadinfo panel when highlighting
    public Text squadName;
    public Text HP;

    //squad # name
    public Text squad1N;
    public Text squad2N;
    public Text squad3N;
    public Text squad4N;

    //squad # population (i.e. number alive)
    public Text squad1P;
    public Text squad2P;
    public Text squad3P;
    public Text squad4P;

    //squad # total hp
    public Text squad1H;
    public Text squad2H;
    public Text squad3H;
    public Text squad4H;
    // Use this for initialization

    public GameObject squad2Panel;
    public GameObject squad3Panel;
    public GameObject squad4Panel;

    public void highlightSquad1() {
        this.GetComponent<PlayerSelect>().usingUI = true;
        this.GetComponent<PlayerSelect>().SelectAndCenter(units[0]);
        squadName.text = units[0].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad2() {
        this.GetComponent<PlayerSelect>().usingUI = true;
        this.GetComponent<PlayerSelect>().SelectAndCenter(units[1]);
        squadName.text = units[1].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad3() {
        this.GetComponent<PlayerSelect>().usingUI = true;
        this.GetComponent<PlayerSelect>().SelectAndCenter(units[2]);
        squadName.text = units[2].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad4() {
        this.GetComponent<PlayerSelect>().usingUI = true;
        this.GetComponent<PlayerSelect>().SelectAndCenter(units[3]);
        squadName.text = units[3].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[3].GetComponent<PUnit>().currentHealth);
    }


    void Start () {
        
        spawnLoc.Add(new Vector3(32, 8, 0.65f));
        spawnLoc.Add(new Vector3(32, 8, 0.65f));
        spawnLoc.Add(new Vector3(29, 11, 0.65f));
        spawnLoc.Add(new Vector3(24, 12, 0.65f));
        spawnLoc.Add(new Vector3(28, 10, 0.65f));
        IList<Squad> squads = PlayerInfo.Squads;
        if(squads == null) {
            return;
        }
        for(int i = 0; i < squads.Count;i++){
            Squad test = squads[i];
            if (squads[i].leader.data.leaderName.Length ==0)
                continue;
            SquadLeader leader = test.leader;
            string name = leader.data.leaderName;
			print (name + " " + test.soldiers.soldierClass);
			float hp = leader.data.health + 2 * test.soldiers.health;
			float dam = leader.data.damage + 2 * test.soldiers.damage;
			float mov = leader.data.speed * test.soldiers.speedMult;
            GameObject unit;
            unit = Instantiate(unitPrefab) as GameObject;
            unit.transform.position = spawnLoc[i];
            /*unit.GetComponent<PUnit>().currentHealth = unit.GetComponent<PUnit>().maxHealth = unit.GetComponent<PUnit>().updateMaxHealth = 500;
            unit.GetComponent<PUnit>().speed = 3;
            unit.GetComponent<PUnit>().name = name;
        	unit.GetComponent<PUnit>().damage = Random.Range(-10,10) + 30;*/
			unit.GetComponent<PUnit> ().currentHealth = unit.GetComponent<PUnit> ().maxHealth = unit.GetComponent<PUnit> ().updateMaxHealth = hp;
			unit.GetComponent<PUnit> ().speed = mov;
			unit.GetComponent<PUnit> ().name = name;
			unit.GetComponent<PUnit> ().damage = dam;
            unit.GetComponent<PUnit>().attackRadius = 6;
            units.Add(unit);
        }
        switch(units.Count) {
            case 0:
                break;
            case 1:
                squad1N.text = units[0].GetComponent<PUnit>().name;
                squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                squad2Panel.SetActive(false);
                squad3Panel.SetActive(false);
                squad4Panel.SetActive(false);
                break;
            case 2:
                squad1N.text = units[0].GetComponent<PUnit>().name;
                squad2N.text = units[1].GetComponent<PUnit>().name;
                squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                squad3Panel.SetActive(false);
                squad4Panel.SetActive(false);
                break;
            case 3:
                squad1N.text = units[0].GetComponent<PUnit>().name;
                squad2N.text = units[1].GetComponent<PUnit>().name;
                squad3N.text = units[2].GetComponent<PUnit>().name;
                squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                squad3P.text = System.Convert.ToString(3 - units[2].GetComponent<PUnit>().numDeaths);
                squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                squad3H.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
                squad4Panel.SetActive(false);
                break;
            case 4:
                squad1N.text = units[0].GetComponent<PUnit>().name;
                squad2N.text = units[1].GetComponent<PUnit>().name;
                squad3N.text = units[2].GetComponent<PUnit>().name;
                squad4N.text = units[3].GetComponent<PUnit>().name;
                squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                squad3P.text = System.Convert.ToString(3 - units[2].GetComponent<PUnit>().numDeaths);
                squad4P.text = System.Convert.ToString(3 - units[3].GetComponent<PUnit>().numDeaths);
                squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                squad3H.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
                squad4H.text = System.Convert.ToString(units[3].GetComponent<PUnit>().currentHealth);
                break;

        }

    }
    

    private void Update() {
        switch (units.Count) {
            case 0:
                break;
            case 1:
                if (units[0] != null) {
                    squad1N.text = units[0].GetComponent<PUnit>().name;
                    squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                    squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                } else {
                    squad1P.text = "0";
                    squad1H.text = "0";
                }

                squad2Panel.SetActive(false);
                squad3Panel.SetActive(false);
                squad4Panel.SetActive(false);
                break;
            case 2:

                if (units[0] != null) {
                    squad1N.text = units[0].GetComponent<PUnit>().name;
                    squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                    squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                } else {
                    squad1P.text = "0";
                    squad1H.text = "0";
                }
                if (units[1] != null) {
                    squad2N.text = units[1].GetComponent<PUnit>().name;
                    squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                    squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                } else {
                    squad2P.text = "0";
                    squad2H.text = "0";
                }



                squad3Panel.SetActive(false);
                squad4Panel.SetActive(false);
                break;
            case 3:

                if (units[0] != null) {
                    squad1N.text = units[0].GetComponent<PUnit>().name;
                    squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                    squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                } else {
                    squad1P.text = "0";
                    squad1H.text = "0";
                }

                if (units[1] != null) {
                    squad2N.text = units[1].GetComponent<PUnit>().name;
                    squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                    squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                } else {
                    squad2P.text = "0";
                    squad2H.text = "0";
                }

                if (units[2] != null) {
                    squad3N.text = units[2].GetComponent<PUnit>().name;
                    squad3P.text = System.Convert.ToString(3 - units[2].GetComponent<PUnit>().numDeaths);
                    squad3H.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
                } else {
                    squad3P.text = "0";
                    squad3H.text = "0";
                }

                squad4Panel.SetActive(false);
                break;
            case 4:
                if (units[0] != null) {
                    squad1N.text = units[0].GetComponent<PUnit>().name;
                    squad1P.text = System.Convert.ToString(3 - units[0].GetComponent<PUnit>().numDeaths);
                    squad1H.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
                } else {
                    squad1P.text = "0";
                    squad1H.text = "0";
                }

                if (units[1] != null) {
                    squad2N.text = units[1].GetComponent<PUnit>().name;
                    squad2P.text = System.Convert.ToString(3 - units[1].GetComponent<PUnit>().numDeaths);
                    squad2H.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
                } else {
                    squad2P.text = "0";
                    squad2H.text = "0";
                }

                if (units[2] != null) {
                    squad3N.text = units[2].GetComponent<PUnit>().name;
                    squad3P.text = System.Convert.ToString(3 - units[2].GetComponent<PUnit>().numDeaths);
                    squad3H.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
                } else {
                    squad3P.text = "0";
                    squad3H.text = "0";
                }

                if (units[3] != null) {
                    squad4N.text = units[3].GetComponent<PUnit>().name;
                    squad4P.text = System.Convert.ToString(3 - units[3].GetComponent<PUnit>().numDeaths);
                    squad4H.text = System.Convert.ToString(units[3].GetComponent<PUnit>().currentHealth);
                } else {
                    squad4P.text = "0";
                    squad4H.text = "0";
                }
                break;

        }
    }

}
