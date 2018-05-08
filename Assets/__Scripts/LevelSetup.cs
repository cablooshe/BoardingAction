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

    public void highlightSquad1() {
        units[0].GetComponent<PUnit>()._selected = true;
        units[1].GetComponent<PUnit>()._selected = false;
        units[2].GetComponent<PUnit>()._selected = false;
        units[3].GetComponent<PUnit>()._selected = false;
        squadName.text = units[0].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[0].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad2() {
        units[0].GetComponent<PUnit>()._selected = false;
        units[1].GetComponent<PUnit>()._selected = true;
        units[2].GetComponent<PUnit>()._selected = false;
        units[3].GetComponent<PUnit>()._selected = false;
        squadName.text = units[1].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[1].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad3() {
        units[0].GetComponent<PUnit>()._selected = false;
        units[1].GetComponent<PUnit>()._selected = false;
        units[2].GetComponent<PUnit>()._selected = true;
        units[3].GetComponent<PUnit>()._selected = false;
        squadName.text = units[2].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[2].GetComponent<PUnit>().currentHealth);
    }

    public void highlightSquad4() {
        units[0].GetComponent<PUnit>()._selected = false;
        units[1].GetComponent<PUnit>()._selected = false;
        units[2].GetComponent<PUnit>()._selected = false;
        units[3].GetComponent<PUnit>()._selected = true;
        squadName.text = units[3].GetComponent<PUnit>().name;
        HP.text = System.Convert.ToString(units[3].GetComponent<PUnit>().currentHealth);
    }


    void Start () {
        /**
        spawnLoc.Add(new Vector3(32, 8, 0.65f));
        spawnLoc.Add(new Vector3(32, 8, 0.65f));
        spawnLoc.Add(new Vector3(29, 11, 0.65f));
        spawnLoc.Add(new Vector3(24, 12, 0.65f));
        spawnLoc.Add(new Vector3(28, 10, 0.65f));*/
        IList<Squad> squads = PlayerInfo.Squads;
        print(squads.Count);
        if(squads == null) {
            return;
        }
        for(int i = 0; i < squads.Count;i++){
            Squad test = squads[i];
            if (squads[i].leader.data.leaderName.Length ==0)
                continue;
            SquadLeader leader = test.leader;
            string name = leader.data.leaderName;
            float hp = test.squadHealth;
            float dam = test.squadDamage;
            float mov = test.squadSpeed;
            GameObject unit;
            unit = Instantiate(unitPrefab) as GameObject;
            unit.transform.position = spawnLoc[i];
            unit.GetComponent<PUnit>().currentHealth = unit.GetComponent<PUnit>().maxHealth = unit.GetComponent<PUnit>().updateMaxHealth = 500;
            unit.GetComponent<PUnit>().speed = 3;
            unit.GetComponent<PUnit>().name = name;
            unit.GetComponent<PUnit>().damage = Random.Range(-10,10) + 30;
            unit.GetComponent<PUnit>().attackRadius = 6;
            units.Add(unit);
        }
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


    }
    

    private void Update() {
        
    }

}
