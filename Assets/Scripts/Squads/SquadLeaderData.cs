using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SquadLeaderData {

    [Header("Set in Inspector")]
    public string leaderName;
    public string squadClass;   //squad leader's specialty

    public int health;
    //public GameObject weapon;
    public List<string> abilities; //TODO: Abilities
    public int damage;
    public float speed;
    public int level;
    public int exp;
    public RawImage icon;

}
