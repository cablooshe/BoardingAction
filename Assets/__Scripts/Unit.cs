using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries

public abstract class Unit : PT_MonoBehaviour {
    static public Unit S;

    static public bool DEBUG = true;

    public float speed = 2; //the speed at which unit walks
    public float health = 10;

    public GameObject haloPrefab; //selection halo prefab that will be used when this unit is selected

    public bool __________________________________;

    public GameObject halo;

    public bool _selected; //is this unit selected

    private Transform viewCharacterTrans;

    protected float lineZ = -0.1f;

    public List<MouseInfo> mouseInfos = new List<MouseInfo>();
    public string actionStartTag; //["mage", "Ground", "Enemy"]

    public bool walking = false;
    public Vector3 walkTarget;
    public Transform characterTrans;

    public List<Transform> transforms;

    public bool isTargeting = false;
    public GameObject targetSelected;
    public float attackRadius = 2;
	protected string enemyTag = "EnemyUnit";

    public bool selected
    {
        get { return _selected; }
        set { _selected = value; }
    }


    // Use this for initialization
    void Awake () {
        S = this;
        this.selected = false;
        //find the characterTrans to rotate with Face()
        characterTrans = transform.Find("CharacterTrans");
        transforms.Add(characterTrans.Find("SquadLeader"));
        transforms.Add(characterTrans.Find("Member1"));
        transforms.Add(characterTrans.Find("Member2"));

        //viewCharacterTrans = characterTrans.Find("View_Character");

        /*halo = Instantiate(haloPrefab) as GameObject;
        halo.transform.parent = this.transform;
        halo.transform.position = new Vector3(halo.transform.position.x - .23f, halo.transform.position.y + .05f, halo.transform.position.z);
        halo.GetComponent<Renderer>().enabled = false;
        halo.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);*/
    }
	

    //______________________________WALKING AND FACING METHODS______________________________________\\
    public void WalkTo(Vector3 xTarget)
    {
        walkTarget = xTarget; //set theh point to walk to
        walkTarget.z = 0; //force z=0
        walking = true;
        Face(walkTarget); //look in the direction of walktarget
    }

    public void StopWalking()
    {
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void Face(Vector3 poi)
    {
        Vector3 delta = poi - pos;
        //use atan2 to get the rotation around z that ponts the x axis of mage:charactertrans towards poi
        float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);
        //set the rotation of charactwertrans (doesnt rotate just yet)
        //characterTrans.rotation = Quaternion.Euler(0, 0, rZ);
        foreach (Transform t in transforms)
        {
            t.rotation = Quaternion.Euler(-rZ, 90, -90);
        }
    }

    void FixedUpdate()
    {//happens every physics step, 50 times per second


        if (walking)
        {
            if ((walkTarget - pos).magnitude < speed * Time.fixedDeltaTime)
            {
                //if mage is very close to walktarget, just stop
                pos = walkTarget;
                StopWalking();
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
        }

        if (!isTargeting){
            findTargetInRange();
        }
        if (isTargeting){
            attackTarget(targetSelected);
        }

    }

    void attackTarget(GameObject enemy){
        enemy.GetComponent<Unit>().health--;
    }
    void findTargetInRange(){
        Vector3 localPos = S.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(localPos, attackRadius);
        int i = 0;
        GameObject toAttack = null;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].gameObject != this.gameObject && hitColliders[i].tag == enemyTag ){
                if (toAttack == null || Vector3.Distance(toAttack.transform.position,localPos) > Vector3.Distance(hitColliders[i].transform.position,localPos)){
                    toAttack = hitColliders[i].gameObject;
                }
            }
            i++;
        }

        if (toAttack != null){
            isTargeting = true;
            targetSelected = toAttack;
        }
    }

    //______________________________________SELECTION RELEVANT METHODS___________________________________________________\\

    public void toggleHalo()
    {
        if (selected)
        {
            halo.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            halo.GetComponent<Renderer>().enabled = false;
        }
    }

    //______________________________________MOUSECLICK METHODS___________________________________________________\\

    //public abstract void LeftClick();
    public abstract void RightClick();
    public abstract void MouseDown();
    public abstract void MouseDrag();
    public abstract void MouseDragUp();
    

}
