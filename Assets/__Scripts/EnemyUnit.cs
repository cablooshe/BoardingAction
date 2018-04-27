using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit {

    [Header("Set in Inspector: EnemyUnit")]
    public Vector3[] patrolPoints;

    private bool onPatrol = false;

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
	}
	
	// Update is called once per frame
	protected new void FixedUpdate () {
        //keep muzzle flash with unit
        if (muzzleFlashFront != null)
        {
            muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 1f);
        }
        //print("HERE1:" + onPatrol);
        if (walking)
        {
            if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime)
            {
                //if mage is very close to walktarget, just stop
                pos = walkTarget;
                StopWalking();
                onPatrol = false;
            }
            else
            {
                //otherwise, walk                
                GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
            }
        }
        else
        {
            //if not walking, velocity should be zero
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            onPatrol = false;
        }
        //print("Here:" + onPatrol);
        if (!onPatrol)
        {
            Patrol();
        }

        if (!isTargeting || !targetInRange(targetSelected))
        {
            findTargetInRange();
        }

        //DO Attack based on attack speed
        if (Time.time >= updateAttack)
        {
            // Change the next update (current second+attackSpeed)
            updateAttack = Mathf.FloorToInt(Time.time) + attackSpeed;
            // Call your function
            attack();
        }
    }

    void Patrol()
    {
        onPatrol = true;
        int ndx = Random.Range(0, patrolPoints.Length);
        print(ndx);
        WalkTo(patrolPoints[ndx]);
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
