using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

    [Header("Set in Inspector")]
    public GameObject corpse;

	// Use this for initialization
	void Start () {
		enemyTag = "PUnit";
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0) {
            Die();
        }
	}
    override public void MouseDown()
    {

    }

    override public void RightClick()
    {
    }
    override public void MouseDrag()
    {

    }
    override public void MouseDragUp()
    {
 
    }
    public void Die() {
        Instantiate(corpse,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z-1), gameObject.transform.rotation);
        Destroy(gameObject);
    }
}
