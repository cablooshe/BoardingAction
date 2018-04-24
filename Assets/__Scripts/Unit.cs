using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries

public abstract class Unit : PT_MonoBehaviour {
    static public bool DEBUG = true;

    public float speed = 2; //the speed at which unit walks
    public float health = 10;

    public GameObject haloPrefab; //selection halo prefab that will be used when this unit is selected
    public GameObject muzzlePrefab;

    public bool __________________________________;

    public GameObject halo;

    private int attackSpeed = 1; //amount of seconds between attacks
    private int updateAttack = 1;
    public GameObject muzzleFlashFront;


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
	public bool inCover = false;

    public bool selected
    {
        get { return _selected; }
        set { _selected = value; }
    }

    [Header("Set in Inspector")]
    public GameObject corpse;


    // Use this for initialization
    protected void Awake () {
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

    protected void FixedUpdate()
    {//happens every physics step, 50 times per second
        //keep muzzle flash with unit
        if (muzzleFlashFront != null) {
            muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 1f);
        }

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

    //_________________________________________________Targeting and attack/damage methods__________________________________________________\\

    void attack() {
        if (isTargeting)
        {
            doDamage(targetSelected);
            attackAnimation(targetSelected);
            targetSelected.GetComponent<Unit>().takeDamageAnimation();
        }
    }

    void attackAnimation(GameObject target) {
        muzzleFlashFront = Instantiate(muzzlePrefab) as GameObject;
        muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z-1f);
        muzzleFlashFront.transform.rotation = this.gameObject.transform.rotation;
    }

    void takeDamageAnimation() {

    }

    void doDamage(GameObject enemy){
//<<<<<<< HEAD
        enemy.GetComponent<Unit>().health--;
        if (enemy.GetComponent<Unit>().health <= 0)
        {
            enemy.GetComponent<Unit>().Die();
        }
//=======
		if ((enemy.GetComponent<Unit> ().inCover) && (Random.value > 0.5)) {
			return;
		}
		enemy.GetComponent<Unit> ().health--;
//>>>>>>> 5d6d2c8da489278b943a300831b8da131b3191b3
    }

    public void Die()
    {
        Instantiate(corpse, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1), gameObject.transform.rotation);
        Destroy(gameObject);
    }

    void findTargetInRange(){
        Vector3 localPos = this.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(localPos, attackRadius);
        int i = 0;
        GameObject toAttack = null;
		inCover = false;
        while (i < hitColliders.Length)
        {
			if (hitColliders [i].gameObject != this.gameObject && hitColliders [i].tag == enemyTag) {
				RaycastHit hit;
				if (!(Physics.Raycast (localPos, hitColliders [i].gameObject.transform.position - localPos, out hit, attackRadius - 0.1f) && hit.collider.gameObject != hitColliders [i].gameObject)) {
					if (toAttack == null || Vector3.Distance (toAttack.transform.position, localPos) > Vector3.Distance (hitColliders [i].transform.position, localPos)) {
						toAttack = hitColliders [i].gameObject;
					}
				}
			} else if (hitColliders [i].tag == "Structure")
				inCover = true;
            i++;
        }

        if (toAttack != null){
            isTargeting = true;
            targetSelected = toAttack;
        } else {
            isTargeting = false;
            targetSelected = null;
        }
    }

    bool targetInRange(GameObject target){
        if (target == null){
            return false;
        }
        Vector3 localPos = this.transform.position;
        RaycastHit hit;
        if (!(Physics.Raycast(localPos, target.transform.position - localPos, out hit, attackRadius - 0.1f) && hit.collider.gameObject != target))
        {
            if (attackRadius >= Vector3.Distance(target.transform.position, localPos))
            {
                return true;
            }
        }
        return false;
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
