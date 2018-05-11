using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries

public abstract class Unit : PT_MonoBehaviour {
    static public bool DEBUG = false;

	[Header("Set in Inspector")]
	public bool isObjective = false;							// Is killing this unit an objective?

	[Header("Unit: Squad Characteristics")]						// Squad characteristics are traits that do not change, set in inspector or imported
	public string name;											// Squad leader's name
	public float speed = 2; 									// Movement speed
	public float maxHealth = 10;								// Starting health
	public float damage = 3;									// Damage per shot
	public float attackRadius = 2;								// Max distance to trigger aggro, also generally shoot from
	public float coverRadius = 1;								// Max distance away from cover while still considered in cover?
	public float attackSpeed = 1f; 								// Seconds between attacks
	public bool isBoss = false;									// For enemies, is this squad a boss?
	public float bossShootRadius = 1;							// If it is a boss, how far away can they shoot from (whereas attackRadius becomes aggro range)

	protected int unitCount = 3;								// Number units in squad
	public float death1;										// Health below which first unit dies
	public float death2;										// Health below which second unit dies
	public int numDeaths = 0;									// Number units died so far in squad

	[Header("Unit: Associated Prefabs - Set in Inspector")]		// Prefabs needed, needs to be set in inspector
	public GameObject haloPrefab; 								// Selection halo prefab that will be used when this squad is selected
	public GameObject muzzlePrefab;
	public GameObject muzzleFlashFront;
	public GameObject halo;
	public GameObject corpse;									// Corpse prefab that spawns when units die

	[Header("Unit: Current Status")]							// Current stats of the squad, should change dynamically
	public float currentHealth = 10;							// How much health does the squad currently have
	public float updateMaxHealth = 10;							// Current max health (for display purposes, should change when units die)
	public float updateDamage = 1;
	public bool _selected; 										// Is this squad selected?
	public bool walking = false;								// Is this squad currently walking?
	public bool isTargeting = false;							// Is this squad targeting anything?
	public bool inCover = false;								// Is this squad in cover?
	public bool isDead = false;									// Is this squad already dead?
	public Vector3 walkTarget;									// Place the squad is trying to walk toward
	public List<GameObject> coverList;							// List of GameObjects currently acting as cover for this squad

	protected float updateAttack = 1;							// Minimum time at which this squad can attack again

	private Transform viewCharacterTrans;						// USELESS

	protected float lineZ = -0.1f;								// USELESS

	public List<MouseInfo> mouseInfos = new List<MouseInfo>();	// For PUnit, mouse handling stuff

	public Transform characterTrans;							// Parent of each unit to make rotation easier

	public List<Transform> transforms;							// List of unit transforms to move each unit with squad
    public float timestamp; 									// used for cooldowns - really could use a better name

    [Header("Unit: Enemy Info")]								// Information about the squad's enemies
	public GameObject targetSelected;							// The squad's current target
	protected string enemyTag = "EnemyUnit";					// The tag of the squad's enemies (makes sure they don't attack friends)
	//public bool randomPatrol;


	[Header("Unit: Animation Info")]							// Just animation stuff
	protected Animator anim;
	public bool selected
	{
		get { return _selected; }
		set { _selected = value; }
	}



	// Use this for initialization
	protected void Awake () {
		this.selected = false; // Should never start selected
		//find the characterTrans to rotate with Face()
		characterTrans = transform.Find("CharacterTrans");
		transforms.Add(characterTrans.Find("SquadLeader"));
		transforms.Add(characterTrans.Find("Member1"));
		transforms.Add(characterTrans.Find("Member2"));
        timestamp = Time.time;

		updateMaxHealth = maxHealth;
		currentHealth = maxHealth;
		updateDamage = damage;

		//viewCharacterTrans = characterTrans.Find("View_Character");

		/*halo = Instantiate(haloPrefab) as GameObject;
        halo.transform.parent = this.transform;
        halo.transform.position = new Vector3(halo.transform.position.x - .23f, halo.transform.position.y + .05f, halo.transform.position.z);
        halo.GetComponent<Renderer>().enabled = false;
        halo.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);*/
	}

	protected void Start()
	{
		death1 = Mathf.Floor(Random.Range(0.3f, 0.6f) * maxHealth);
		death2 = Mathf.Floor(Random.Range(0.1f, 0.2f) * maxHealth);
	}


	//______________________________WALKING AND FACING METHODS______________________________________\\
	public void WalkTo(Vector3 xTarget) // Given a position, try to walk to that position
	{
		walking = true;
		walkTarget = xTarget; //set the point to walk to
		walkTarget.z = 0; //force z=0
		Face(walkTarget); //look in the direction of walkTarget
	}

	public void StopWalking() // Stop walking
	{
		walking = false;
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	public void Face(Vector3 poi) // Rotate the squad so they seem to be looking towards what they should be
	{
		Vector3 delta = poi - pos;
		//use atan2 to get the rotation around z that ponts the x axis of mage:charactertrans towards poi
		float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);
		//set the rotation of charactwertrans (doesnt rotate just yet)
		characterTrans.rotation = Quaternion.Euler(0, 0, rZ);
		/*foreach (Transform t in transforms)
		{
			t.rotation = Quaternion.Euler(-rZ, 90, -90);
		}*/
	}

	protected void FixedUpdate()
	{//happens every physics step, 50 times per second

		//keep muzzle flash with unit
		//if (muzzleFlashFront != null) {
		//	muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - 1f);
		//}

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

		if (!isTargeting || !targetInRange(targetSelected)) // If the squad is not targeting anything, or the target is no longer in range, try to find a new target
		{
			findTargetInRange();
		}

        findCover(); // Always find all cover in range

		//DO Attack based on attack speed
		if (Time.time >= updateAttack)
		{
			// Change the next update (current second+attackSpeed)
			updateAttack = Mathf.FloorToInt(Time.time) + attackSpeed;
			// Call your function
			attack();
		}

        //if the unit isnt moving, stop walking
        if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude < 0.09f)
        {
            this.StopWalking();
        }


    }


    //_________________________________________________Targeting and attack/damage methods__________________________________________________\\

    public void attack() {
		if (isTargeting)
		{
			if (isBoss) { // If this unit is a boss, it may be targeting even if not in shooting range. If so, don't shoot.
				if (Vector3.Distance (targetSelected.transform.position, transform.position) > bossShootRadius)
					return;
			}
			targetSelected.GetComponent<Unit>().takeDamage(this.updateDamage, this.gameObject);
			attackAnimation(targetSelected);
			targetSelected.GetComponent<Unit>().takeDamageAnimation();
		}
	}

	public void attackAnimation(GameObject target) {
        for(int i = 0; i < transforms.Count(); i++){
		    muzzleFlashFront = Instantiate(muzzlePrefab) as GameObject;
            if (i == 0) {
		        muzzleFlashFront.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z-0.5f);
            } else {
                muzzleFlashFront.transform.position = new Vector3(this.transforms[i-1].position.x, this.transforms[i - 1].position.y, this.transforms[i - 1].position.z - 0.5f);
            }
            muzzleFlashFront.transform.LookAt(target.GetComponent<Unit>().characterTrans.transform.position);
            muzzleFlashFront.transform.Rotate(new Vector3(Random.Range(-15,15), Random.Range(-15, 15), Random.Range(-3, 3)));
            muzzleFlashFront.GetComponent<Rigidbody>().velocity = muzzleFlashFront.transform.forward * 35;
            Destroy(muzzleFlashFront, 0.5f);
        }
	}

	public void takeDamage(float damage, GameObject enemy) {
		Vector3 enemyPosition = enemy.transform.position - this.transform.position;
		if((inCover) && (Random.value > 0.5)) { // Check if cover would block the damage before seeing if there is any cover between squad and enemy to save time
			foreach (GameObject c in coverList) {
				Vector3 coverPosition = c.transform.position - this.transform.position;
				if (Vector3.Angle (enemyPosition, coverPosition) < 30) // Check if cover is relatively between squad and enemy
					return;
			}
		}
        takeDamage(damage);
	}

    //handling types of direct damage, such as those from exsplosion
    public void takeDamage(string type)
    {
        switch (type)
        {
            case "explosion":
                takeDamage(Random.Range(100, 200));
                break;
            default:
                takeDamage(10.0f);
                break;
        }   
    }

    //used for taking damage from things that cover does not apply to, such as exsplosions
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if ((numDeaths == 0 && death1 > currentHealth) || (numDeaths == 1 && death2 > currentHealth))
        {
            loseMember(numDeaths++);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

	public void loseMember(int deathCount) { // Make a corpse and try not to mess with the rest of the squad
		if (numDeaths == 1) {
			updateMaxHealth = death1;
			updateDamage = Mathf.Ceil(2 * damage / 3);
		} else if (numDeaths == 2) {
			updateMaxHealth = death2;
			updateDamage = Mathf.Ceil(damage / 3);
		}
		Instantiate(corpse, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0.4f), Quaternion.Euler(Random.Range(0,360),0, Random.Range(0,360)));
		this.transforms[this.transforms.Count - 1].gameObject.SetActive(false);
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
		if (isDead) // Prevent this from running multiple times
			return;
		Instantiate(corpse, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z - 1), gameObject.transform.rotation);
		isDead = true;
		if (isObjective) // If killing this squad was an objective, score the objective
		{
			GameObject.Find("Map").GetComponent<Map>().CompletedObjective();
		}
		Destroy(gameObject);
	}

	public void findTargetInRange(){
		Vector3 localPos = this.characterTrans.position;
		Collider[] hitColliders = Physics.OverlapSphere(localPos, attackRadius);
		int i = 0;
        GameObject toAttack = null;
		while (i < hitColliders.Length) // Check every collider within a certain radius, if it is an enemy then target it
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
			}
			i++;
		}

		if (toAttack != null){ // If a target was found, target it
			isTargeting = true;
			targetSelected = toAttack;
		} else {
			isTargeting = false;
			targetSelected = null;
		}

    }

    public void findCover(){
        Vector3 localPos = this.characterTrans.position;
        inCover = false;
        Collider[] coverColliders = Physics.OverlapSphere(localPos, coverRadius);
        coverList.Clear();
        int i = 0;
        while (i < coverColliders.Length) // for all colliders in range, if cover, add to coverList
        {
            if ((coverColliders[i].tag == "Structure")
                && (coverColliders[i].GetComponent<Structure>().isCover))
            {
                //&& (Vector3.Distance(coverColliders[i].transform.position, localPos) < coverRadius)){

                inCover = true;
                coverList.Add(coverColliders[i].gameObject);
            }
            i++;
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
