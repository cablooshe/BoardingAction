using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : MonoBehaviour {

    [Header("Set in Inspector")]
    public string leaderName;
    public string squadClass;   //squad leader's specialty

    public int health;
    public GameObject weapon;
    public List<string> abilities; //TODO: Abilities
    public int damage;
    public float speed;
    public int level;
    public int exp;

    public SquadLeader(string leaderName, string squadClass) {
        switch (squadClass) {
            case "Scout":
                speed = 20 + (10 * level);
                health = 80 + (10 * level);
                //TODO: Equip Weapon
                damage = 50;  //TODO: how does weapon affect this?
                break;
            default:
                break;
        }
        this.leaderName = leaderName;
        this.squadClass = squadClass;
        this.damage = (int)(Random.value * 10);
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
