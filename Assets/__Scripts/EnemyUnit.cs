﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {
	public enum behavior { standard, charge, passive };

    [Header("Set in Inspector: EnemyUnit")]
    public Vector3[] patrolPoints;
	public behavior unitBehavior = behavior.standard;

    //private bool onPatrol = false;
    private Vector3 patrolDest;

    // Use this for initialization
    /*protected new void Awake()
    {
        death1 = Random.Range(0.3f, 0.6f) * maxHealth;
        death2 = Random.Range(0.1f, 0.2f) * maxHealth;
        this.selected = false;
        //find the characterTrans to rotate with Face()
        characterTrans = transform.Find("CharacterTrans");
        transforms.Add(characterTrans.Find("SquadLeader"));
        transforms.Add(characterTrans.Find("Member1"));
        transforms.Add(characterTrans.Find("Member2"));

        onPatrol = false;
    }*/

    // Use this for initialization
    new void Start () {
		enemyTag = "PUnit";
        Patrol();
		base.Start ();
        //WalkTo(patrolDest);
    }
	
	// Update is called once per frame
	protected void Update () {
		if (isTargeting) {
            if (unitBehavior == behavior.charge)
                WalkTo(targetSelected.transform.position);
            else if (unitBehavior == behavior.passive)
                WalkAway(targetSelected.transform.position);
			else
				StopWalking ();
			return;
		}
        if (!walking)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        //onPatrol = true;
        walking = true;
        patrolDest = patrolPoints[Random.Range(0, patrolPoints.Length)];
        WalkTo(patrolDest);
    }

    void OnCollisionEnter(Collision c)
    {        
        GameObject go = c.gameObject;
        if (go.tag == "PUnit" && go.GetComponent<PUnit>().walking == false)
        {
            StopWalking();
        }
        if (go.tag == "Door")
        {
            go.GetComponentInParent<DoubleDoor>().OpenDoors();
            Patrol();
        }
       
    }

    public void WalkAway(Vector3 xTarget) // Given a position, try to walk to that position
    {
        Vector3 DirectionAway = (xTarget - this.gameObject.transform.position);
        DirectionAway.Normalize();
        walking = true;
        walkTarget = xTarget; //set the point to walk to
        walkTarget.z = 0; //force z=0
        Face(walkTarget); //look in the direction of walkTarget
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
}
