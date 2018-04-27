using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

    [Header("Set in Inspector: EnemyUnit")]
    public Vector3[] patrolPoints;

<<<<<<< HEAD
    //private bool onPatrol = false;
    private Vector3 patrolDest;
=======
    private bool onPatrol = false;
>>>>>>> parent of 84639b5... players will spawn into map 3.  Pickups added to map though not sure if there are benefits to picking them up yet.  Added colors to tell friend from foe.  Edited enemy stats on map3

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
    void Start () {
		enemyTag = "PUnit";
        Patrol();
        //WalkTo(patrolDest);
    }
	
	// Update is called once per frame
	protected void Update () {
        /*if (((patrolDest.x - GetComponent<Rigidbody>().position.x) + (GetComponent<Rigidbody>().position.y - pos.y)) < speed * Time.fixedDeltaTime)
        {
            Patrol();
        }*/
        /*if(GetComponent<Rigidbody>().velocity == Vector3.zero)
        {
            print("Stopped");
            Patrol();
        }*/
        /*if (onPatrol)
        {
            if (!walking)
            {
                //onPatrol = false;
                Patrol();
            }
        }
        else
        {
            Patrol();
<<<<<<< HEAD
        }*/
        if (!walking)
        {
            walking = true;
            patrolDest = patrolPoints[Random.Range(0, patrolPoints.Length)];
            WalkTo(patrolDest);
        }
    }
=======
        }
>>>>>>> parent of 84639b5... players will spawn into map 3.  Pickups added to map though not sure if there are benefits to picking them up yet.  Added colors to tell friend from foe.  Edited enemy stats on map3

    void Patrol()
    {
        //onPatrol = true;
        walking = true;
        patrolDest = patrolPoints[Random.Range(0, patrolPoints.Length)];
        WalkTo(patrolDest);
    }

    void OnCollisionEnter(Collision c)
    {
        //print("Colliding");
        GameObject go = c.gameObject;
        if (go.tag == "PUnit" && go.GetComponent<PUnit>().walking == false)
        {
            StopWalking();
        }
        if (go.tag == "Door")
        {
            go.GetComponentInParent<DoubleDoor>().OpenDoors();
        }
<<<<<<< HEAD
        /*Room currentRoom = go.transform.parent.GetComponent<Room> (); 
		if (currentRoom != null) {
			currentRoom.makeVisible ();
		}*/
=======
    }

    void Patrol()
    {
        onPatrol = true;
        int ndx = Random.Range(0, patrolPoints.Length);
        print(ndx);
        WalkTo(patrolPoints[ndx]);
>>>>>>> parent of 84639b5... players will spawn into map 3.  Pickups added to map though not sure if there are benefits to picking them up yet.  Added colors to tell friend from foe.  Edited enemy stats on map3
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
