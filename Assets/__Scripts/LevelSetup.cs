using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
    public List<Vector3> spawnLoc;
    public GameObject unitPrefab;   
	// Use this for initialization
	void Start () {
        /*spawnLoc.Add(new Vector3(32, 8, 0.65f));
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
        }
        
	}
	
}
