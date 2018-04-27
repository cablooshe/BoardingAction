using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

    [Header("Set in Inspector: EnemyUnit")]
    public Vector3[] patrolPoints;

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
        }*/
        if (!walking)
        {
            walking = true;
            patrolDest = patrolPoints[Random.Range(0, patrolPoints.Length)];
            WalkTo(patrolDest);
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
        /*Room currentRoom = go.transform.parent.GetComponent<Room> (); 
		if (currentRoom != null) {
			currentRoom.makeVisible ();
		}*/
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
