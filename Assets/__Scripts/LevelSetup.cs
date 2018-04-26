using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetup : MonoBehaviour {
    public List<Vector3> spawnLoc;
    public GameObject unitPrefab;
	// Use this for initialization
	void Start () {
        spawnLoc.Add(new Vector3(-9, 0, 0.65f));
        IList<Squad> squads = PlayerInfo.Squads;
        for(int i = 0; i < squads.Count;i++){
            Squad test = squads[i];
            SquadLeader leader = test.leader;
            string name = leader.data.leaderName;
            float hp = test.squadHealth;
            float dam = test.squadDamage;
            float mov = test.squadSpeed;
            GameObject unit;
            unit = Instantiate(unitPrefab) as GameObject;
            unit.transform.position = spawnLoc[i];
            unit.GetComponent<PUnit>().currentHealth = hp;
            unit.GetComponent<PUnit>().maxHealth = unit.GetComponent<PUnit>().updateMaxHealth = hp;
            unit.GetComponent<PUnit>().speed = mov;
            unit.GetComponent<PUnit>().name = name;
            unit.GetComponent<PUnit>().damage = dam;
        }
        
	}
	
}
