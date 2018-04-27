using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries

public abstract class Unit : PT_MonoBehaviour {
    static public bool DEBUG = true;

    [Header("Set in Inspector")]
    public bool isObjective = false;

    [Header("Unit: Squad Characteristics")]
    public string name;
    public float speed = 2; //the speed at which unit walks
    public float maxHealth = 10;
    public float damage = 1;
	public float attackRadius = 2;
	public float coverRadius = 1;
	public float attackSpeed = 1f; //amount of seconds between attacks

    protected int unitCount = 3;
    public float death1;
    public float death2;
    public int numDeaths = 0;

	[Header("Unit: Associated Prefabs - Set in Inspector")]
    public GameObject haloPrefab; //selection halo prefab that will be used when this unit is selected
	public GameObject muzzlePrefab;
	public GameObject muzzleFlashFront;
	public GameObject halo;
	public GameObject corpse;

	[Header("Unit: Current Status")]
    public float currentHealth = 10;
    public float updateMaxHealth = 10;
    public bool _selected; //is this unit selected
	public bool walking = false;
	public bool isTargeting = false;
	public bool inCover = false;
	public Vector3 walkTarget;

	protected float updateAttack = 1;

    private Transform viewCharacterTrans;

    protected float lineZ = -0.1f;

    public List<MouseInfo> mouseInfos = new List<MouseInfo>();

	public Transform characterTrans;

	public List<Transform> transforms;

	[Header("Unit: Enemy Info")]
    public GameObject targetSelected;
    protected string enemyTag = "EnemyUnit";
    //public bool randomPatrol;


    [Header("Unit: Animation Info")]
    protected Animator anim;
    public bool selected
    {
        get { return _selected; }
        set { _selected = value; }
    }



    // Use this for initialization
    protected void Awake () {
        death1 = Random.Range(0.3f, 0.6f) * maxHealth;
        death2 = Random.Range(0.1f, 0.2f) * maxHealth;
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
        walking = true;
        walkTarget = xTarget; //set the point to walk to
        walkTarget.z = 0; //force z=0
        Face(walkTarget); //look in the direction of walkTarget
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
            //Mathf.Abs((walkTarget.x - pos.x) + (walkTarget.y - pos.y))
            if (Mathf.Abs((walkTarget.x - pos.x) + (walkTarget.y - pos.y)) < speed * Time.fixedDeltaTime)
            {
                // print("CLOSE");
                //if mage is very close to walktarget, just stop
                //pos = walkTarget;
                //Vector3 stopPos = new Vector3(walkTarget.x, walkTarget.y, pos.z);
                //pos = stopPos;
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

    public void attack() {
        if (isTargeting)
        {
            targetSelected.GetComponent<Unit>().takeDamage(this.damage);
            attackAnimation(targetSelected);
            targetSelected.GetComponent<Unit>().takeDamageAnimation();
        }
    }

    public void attackAnimation(GameObject target) {
        muzzleFlashFront = Instantiate(muzzlePrefab) as GameObject;
        muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z-1f);
        muzzleFlashFront.transform.rotation = this.gameObject.transform.rotation;
    }

    public void takeDamage(float damage) {
        if((inCover) && (Random.value > 0.5)) {
            return;
        }
        currentHealth-=damage;
        if ((numDeaths == 0 && death1 > currentHealth) || (numDeaths == 1 && death2 > currentHealth))
        {
            loseMember(numDeaths++);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void loseMember(int deathCount) {
        Instantiate(corpse, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.4f), Quaternion.Euler(Random.Range(0,360),0, Random.Range(0,360)));
        Destroy(this.transforms[this.transforms.Count - 1].gameObject);
        this.transforms.RemoveAt(transforms.Count - 1);

    }

    public void takeDamageAnimation() {

    }

    /*void doDamage(GameObject enemy){
		if ((enemy.GetComponent<Unit> ().inCover) && (Random.value > 0.5)) {
			return;
		}
		enemy.GetComponent<Unit> ().currentHealth--;
        if (numDeaths == 0 && enemy.GetComponent<Unit>().death1 > enemy.GetComponent<Unit>().currentHealth) {
            enemy.GetComponent<Unit>().loseMember(
        }
        if (enemy.GetComponent<Unit>().currentHealth <= 0)
        {
            enemy.GetComponent<Unit>().Die();
        }
    }*/

    public void Die()
    {
        print("Die");
        Instantiate(corpse, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1), gameObject.transform.rotation);
        if (isObjective)
        {
            print("Dead");
            GameObject.Find("Map").GetComponent<Map>().CompletedObjective();
        }
        Destroy(gameObject);
    }

    public void findTargetInRange(){
        Vector3 localPos = this.transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(localPos, attackRadius);
        int i = 0;
        GameObject toAttack = null;
		inCover = false;
        while (i < hitColliders.Length)
        {
			if (hitColliders [i].gameObject != this.gameObject && hitColliders [i].tag == enemyTag) {
				RaycastHit hit;
                /*if (!(Physics.Raycast (localPos, hitColliders [i].gameObject.transform.position - localPos, out hit, attackRadius - 0.1f) && hit.collider.gameObject != hitColliders [i].gameObject)) {
					if (toAttack == null || Vector3.Distance (toAttack.transform.position, localPos) > Vector3.Distance (hitColliders [i].transform.position, localPos)) {
                        print("DOING SOMETHING");
						toAttack = hitColliders [i].gameObject;
					}
				}*/
                toAttack = hitColliders[i].gameObject;
			} else if ((hitColliders [i].tag == "Structure")
			           && (hitColliders [i].GetComponent<Structure> ().isCover)
			           && (Vector3.Distance (hitColliders [i].transform.position, localPos) < coverRadius)) {
				inCover = true;
			}
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

    public bool targetInRange(GameObject target){
        if (target == null){
            return false;
        }
        Vector3 localPos = this.transform.position;
        RaycastHit hit;
        /*if (!(Physics.Raycast(localPos, target.transform.position - localPos, out hit, attackRadius - 0.1f) && hit.collider.gameObject != target))
        {
            if (attackRadius >= Vector3.Distance(target.transform.position, localPos))
            {
                return true;
            }
        }*/
        if (attackRadius >= Vector3.Distance(target.transform.position, localPos))
        {
            return true;
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
