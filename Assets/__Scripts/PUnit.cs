using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries



//mage is a subclass of PT_MonoBehavior
public class PUnit : Unit {
	public enum Ability { grenade, deployCover, enrage, heal, c4 };


	[Header("PUnit: Associated Prefabs - Set in Inspector")]
    public GameObject tapIndicatorPrefab;
    public GameObject explosion;
    public GameObject deployableCover;
    public GameObject c4prefab;

  
	[Header("PUnit: Abilities")]
	public Ability ability1 = Ability.grenade;
	public Ability ability2 = Ability.enrage;

    [Header("PUnit: Mouse Info")]
    public float mTapTime = 0.5f; //how long is considered a tap
    public float mDragDist = 10; //how long is considered a drag
	public float activeScreenWidth = 1; //the % of the screen to use
	public MPhase mPhase = MPhase.idle;
    public bool prepGrenade = false;
    public float grenadeCoolDown = 10f;
    public float coverCoolDown = 5f;
    public float grenadeRange = 15f;
    public float enrageModifier = 1.5f; 
    public float enrageCoolDown = 20f; //amount of time to be able to enrage again
    public float c4CoolDown = 20f;
    public bool readyAbility1 = true;
    public bool readyAbility2 = true;
    public bool enraged = false;
    public float enrageTime = 10f;//Amount of time a unit has the buff from enrage
    public float enrageTimer = 0f;
    public GameObject c4instance;
    public GameObject rangeCircle; //used to visualize the grenade range
    public int numHealsLeft = 3;
    public float percentHeal = 0.3f;
    public Color lastColor;
    public bool flash = false; //used to flash the circle red when ability not ready
    public float flashTimer = 0f;
    public float flashCoolDown = .05f;


    protected new void Awake() {
        base.Awake();
        anim = GetComponent<Animator>();
        /* S = this;
         this.selected = false;
         mPhase = MPhase.idle;
         //this.GetComponent<Rigidbody>().transform.position.z = 0;
         //find the characterTrans to rotate with Face()
         characterTrans = transform.Find("CharacterTrans");
         transforms.Add(characterTrans.Find("SquadLeader"));
         transforms.Add(characterTrans.Find("Member1"));
         transforms.Add(characterTrans.Find("Member2"));

         //viewCharacterTrans = characterTrans.Find("View_Character");*/
        mPhase = MPhase.idle;
        
        halo = Instantiate(haloPrefab) as GameObject;
        halo.transform.parent = this.transform;
        halo.transform.position = new Vector3(halo.transform.position.x-.23f, halo.transform.position.y+.05f, halo.transform.position.z);
        halo.GetComponent<Renderer>().enabled = false;
        halo.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);

        rangeCircle = Instantiate(haloPrefab) as GameObject;
        rangeCircle.GetComponent<Renderer>().enabled = false;
        rangeCircle.transform.parent = this.transform;
        rangeCircle.transform.position = new Vector3(halo.transform.position.x - .23f, halo.transform.position.y + .05f, halo.transform.position.z);
        
        rangeCircle.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);
        rangeCircle.transform.localScale += new Vector3(2*grenadeRange, 2*grenadeRange, 0);
        

    }
    public new void StopWalking() {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        anim.SetBool("Walking", false);
    }
    
    public void flashCircle()
    {
        if(halo.GetComponent<SelectionHalo>().mat.color != Color.red)
        {
            lastColor = halo.GetComponent<SelectionHalo>().mat.color;
        }
        halo.GetComponent<SelectionHalo>().mat.color = Color.red;
        flash = true;
        flashTimer = Time.time + flashCoolDown;
    }
    
    public void dropCover()
    {
        if (ability1 == Ability.deployCover)
        {
            if (!cooledDown(ability1timestamp))
            {
                //print("Cover not ready");
                flashCircle();
                return;
            }
        }
        else
        {
            if (!cooledDown(ability2timestamp))
            {
                //print("Cover not ready");
                flashCircle();
                return;
            }
        }
       
        Debug.Log("COVER DROPPED");
        Vector3 coverRot = characterTrans.rotation.eulerAngles;
        Vector3 coverPos = transform.position;
        GameObject go = Instantiate(deployableCover) as GameObject;
        go.transform.rotation = Quaternion.Euler(coverRot.x, coverRot.y, coverRot.z);
        go.transform.position = coverPos;
        if (ability1 == Ability.deployCover)
        {
            ability1timestamp = Time.time + coverCoolDown;
        }
        else
        {
            ability2timestamp = Time.time + coverCoolDown;
        }
        halo.GetComponent<SelectionHalo>().mat.color = Color.green;
    }

    public void throwGrenade() {
        Debug.Log("GRENADE THROWN");
    }

    public void placeCharge() {
        Debug.Log("CHARGE PLACED");
    }

    public void deployDrone() {
        Debug.Log("DRONE DEPLOYED");
    }

    public void deployTurret() {
        Debug.Log("TURRET DEPLOYED");
    }

    public void hackConsole() {
        Debug.Log("CONSOLE HACKED");
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
            rangeCircle.GetComponent<Renderer>().enabled = false;
            halo.GetComponent<Renderer>().enabled = false;
            unreadyGrenade();
        }
    }

    void updateAnimation() {

        if (!walking && this.inCover)
        {
            anim.SetBool("InCover", true);
        }
        else {
            anim.SetBool("InCover", false);
        }
        if (isAttacking)
        {
            anim.SetBool("Attacking", true);

        }
        else{
            anim.SetBool("Attacking", false);
        }
        //else { anim.SetBool("Attacking", false); }
        if (walking)
        {
            anim.SetBool("Walking", true);
            anim.SetFloat("WalkSpeed", this.gameObject.GetComponent<Rigidbody>().velocity.magnitude*0.5f);
        }
        else{
            anim.SetBool("Walking", false);
            anim.SetFloat("WalkSpeed", 0f);

        }
    }

    void Update() {
        toggleHalo();
        updateAnimation();

        if (!selected) return;
        //find whether the mouse button 0 was pressed or released this frame
        bool b0Down = Input.GetMouseButtonDown(0);
        bool b1Down = Input.GetMouseButtonDown(1);
        bool b1Up = Input.GetMouseButtonUp(1);
        bool b0Up = Input.GetMouseButtonUp(0);

        //handle all input here except for inventory button
        /*
         * there are only a few possible actions:
         * 1. tap on the ground to move to that point
         * 2. drag on the ground with no spell selected to move the mage
         * 3. drag on the ground with spell to case along the ground
         * 4. tap on an enemy to attack
         */
        //an expample of using < to return a bool value

        bool inActiveArea = (float)Input.mousePosition.x / Screen.width < activeScreenWidth;

        //this is handled as an if statement instead of switch because a tap can sometimes happen within a single frame
        if (mPhase == MPhase.idle) {
            if(b1Down && inActiveArea) {
                mouseInfos.Clear(); //clear the mouseInfos
                AddMouseInfo(); //and add a first mouseinfo

                //if the mouse was clicked on something, its aavalid mousedown
                if (mouseInfos[0].hit) {
                    MouseDown();
                    mPhase = MPhase.down;
                }
            }
        }

        if (mPhase == MPhase.down) {
            AddMouseInfo();
            if(b1Up) {
                RightClick();
                mPhase = MPhase.idle;
            } 
        }

        if(mPhase == MPhase.drag) {
            AddMouseInfo();
            if(b0Up) {
                MouseDragUp();
                mPhase = MPhase.idle;
            } else {
                MouseDrag(); //still dragging
            }
        }


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useAbility1();
          
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useAbility2();
        }

        if (enraged && enrageTimer < Time.time) //Unenrages the unit
        {
            unenrage();
        }  

        if(flash && flashTimer <= Time.time)
        {
            halo.GetComponent<SelectionHalo>().mat.color = lastColor;
            flash = false;
        }

    }

    protected override void FixedUpdate() {
        base.FixedUpdate();

    }

    public void useAbility1()
    {
        if (cooledDown(ability1timestamp))
        {
            switch (ability1)
            {
                case Ability.grenade:
                    readyGrenade();
                    break;
                case Ability.enrage:
                    enrage(1);
                    break;
                case Ability.deployCover:
                    anim.SetTrigger("DeployingCover"); //this is terrible be wary if making changes to this using cooldowns
                    break;
                case Ability.c4:
                    if (c4instance == null)
                    {
                        placeC4();
                    }
                    else
                    {
                        blowC4(2);
                    }
                    break;
                case Ability.heal:
                    heal();
                    break;
            }
        }
        else
        {
            //print("ability 1 not ready");
            flashCircle();
        }
    }

    public void useAbility2()
    {
        if (cooledDown(ability2timestamp))
        {
            switch (ability2)
            {
                case Ability.grenade:
                    readyGrenade();
                    break;
                case Ability.enrage:
                    enrage(2);
                    break;
                case Ability.deployCover:
                    anim.SetTrigger("DeployingCover"); //this is terrible be wary if making changes to this using cooldowns
                    break;
                case Ability.c4:
                    if (c4instance == null)
                    {
                        placeC4();
                    }
                    else
                    {
                        blowC4(2);
                    }
                    break;
                case Ability.heal:
                    heal();
                    break;
            }

        }
        else
        {
            //print("ability 2 not ready");
            flashCircle();
        }
    }

    public void placeC4()
    {
        print("placec4");
        c4instance = Instantiate(c4prefab) as GameObject;
        //c4instance.transform.position = this.transform.position;
        Vector3 c4Rot = characterTrans.rotation.eulerAngles;
        Vector3 c4Pos = transform.position;
        c4instance.transform.rotation = Quaternion.Euler(c4Rot.x, c4Rot.y, c4Rot.z);
        c4instance.transform.position = c4Pos;
    }

    public void blowC4(int abilityID)
    {
        GameObject exp;
        exp = Instantiate(explosion) as GameObject;
        exp.transform.position = c4instance.transform.position;
        Destroy(c4instance);
        c4instance = null;

        if (abilityID == 1)
        {
            ability1timestamp = Time.time + c4CoolDown;
        }
        else
        {
            ability2timestamp = Time.time + c4CoolDown;
        }
    }

    public void enrage(int abilityID)
    {
        this.updateDamage = this.updateDamage * this.enrageModifier;
        int i = 0;
        for (i = 0; i < this.transforms[0].childCount; i++)
        {
            this.transforms[1].GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
            this.transforms[2].GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
        }
        prepGrenade = false;
        if (abilityID == 1)
        {
            ability1timestamp = Time.time + enrageCoolDown;
        }
        else
        {
            ability2timestamp = Time.time + enrageCoolDown;
        }
        enrageTimer = Time.time + enrageTime;
        enraged = true;
    }



    public void unenrage()
    {
        if (enraged)
        {
            this.updateDamage = this.updateDamage / this.enrageModifier;

            for (int i = 0; i < this.transforms[0].childCount; i++)
            {
                this.transforms[1].GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
                this.transforms[2].GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.grey);
            }
            enraged = false;
        }
        else
        {
            print("already unenraged");
        }
    }

    void readyGrenade()
    {
        prepGrenade = true;
        walking = false;
        halo.GetComponent<SelectionHalo>().mat.color = Color.yellow;
        rangeCircle.GetComponent<Renderer>().material.color = Color.yellow;
        rangeCircle.GetComponent<SelectionHalo>().mat.mainTextureOffset = new Vector2(.2f, 0);
        rangeCircle.GetComponent<Renderer>().enabled = true;
    }

    void throwGrenade(Vector3 xTarget)
    {
        float dist = Vector3.Distance(transform.position, xTarget);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, xTarget - transform.position, dist, LayerMask.GetMask("Default")))
        {
            print("collide");
            Debug.DrawRay(transform.position, xTarget - transform.position, Color.red, 100000, true);
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
            rangeCircle.GetComponent<Renderer>().enabled = false;
            return;
        }

        if (dist < grenadeRange)
        {
            GameObject exp;
            exp = Instantiate(explosion) as GameObject;
            exp.transform.position = xTarget;
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
            if (ability1 == Ability.grenade)
            {
                ability1timestamp= Time.time + grenadeCoolDown;
            }
            else
            {
                ability2timestamp = Time.time + grenadeCoolDown;
            }
            rangeCircle.GetComponent<Renderer>().enabled = false;

        }
        else
        {
            print("too far");
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
            rangeCircle.GetComponent<Renderer>().enabled = false;

        }
    }

    void unreadyGrenade()
    {
        prepGrenade = false;
        halo.GetComponent<SelectionHalo>().mat.color = Color.green;
    
    }

    public void heal() {
        if (this.currentHealth == this.updateMaxHealth) {
            print("you're already full health!");
        } else if (numHealsLeft <= 0) {
            print("you've used up all your heals!");
        } else {
            numHealsLeft--;
            float newHealth = (this.updateMaxHealth * percentHeal) + this.currentHealth;
            if (newHealth > this.updateMaxHealth) {
                this.currentHealth = this.updateMaxHealth;
            } else {
                this.currentHealth = newHealth;
            }
            
        }
    }



 


    //check a timestamp to see if its passed
    bool cooledDown(float timestamp)
    {
        return timestamp <= Time.time;
    }








  

  
    //used to put unit in grenade throwing state
   

    //used if grenade is not thrown
 
    //Pulls inifo about the mouse, adds it to mouseInfos, and returns it
    MouseInfo AddMouseInfo() {
        MouseInfo mInfo = new MouseInfo();
        mInfo.screenLoc = Input.mousePosition;
        mInfo.loc = Utils.mouseLoc; //gets the position of the mouse at z=0
        mInfo.ray = Utils.mouseRay;//gets the ray from the main cam through the mouse pointer
        mInfo.time = Time.time;
        mInfo.Raycast(); //default is to raycast with no mask

        if(mouseInfos.Count == 0) {
            mouseInfos.Add(mInfo);
        } else {
            float lastTime = mouseInfos[mouseInfos.Count - 1].time;
            if(mInfo.time != lastTime) {
                //if time has passed since the last mouseinfo
                mouseInfos.Add(mInfo);
            }
            //this time test is needed because addmouseinfo could be called twice in one frame
           
        }
        return mInfo;
    }

    public MouseInfo lastMouseInfo {
        get {
            if (mouseInfos.Count == 0) return null;
            return (mouseInfos[mouseInfos.Count - 1]);
        }
    }
    
    override public void MouseDown() {
        if (DEBUG) print("Mage.MouseDown()");

        GameObject clickedGO = mouseInfos[0].hitInfo.collider.gameObject;
        //if mouse wasnt clicked on anything, this throuws an error, however mousedown only calls when clicked on something,, so we're safe
    }

    override public void RightClick()
    {
        if (DEBUG) print("Mage.RightClick()");

	

        if (!prepGrenade)
        {
            WalkTo(lastMouseInfo.loc);
        }
        else
        {
            throwGrenade(lastMouseInfo.loc);
        }
       
    }

    override public void MouseDrag()
    {
		// We might need this later, but now it is USELESS (almost as much as Gordon (double lol))
    }
    override public void MouseDragUp()
    {
        // We might need this later, but now it is USELESS (almost as much as Gordon's comments (lol))
    }

    



   
    //show where the player tapped
    /*public void ShowTap(Vector3 loc) {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }*/

    void OnCollisionEnter (Collision c) {
        print("Colliding");
        GameObject go = c.gameObject;
        if ((go.tag == "PUnit" && go.GetComponent<PUnit>().walking == false) || go.tag == "Structure") {
            print("OnCollisionEnter");
            StopWalking();
        }
		if (go.tag == "Door") {
			go.GetComponentInParent<DoubleDoor>().OpenDoors();
		}
		/*Room currentRoom = go.transform.parent.GetComponent<Room> (); 
		if (currentRoom != null) {
			currentRoom.makeVisible ();
		}*/
    }

	void OnTriggerEnter(Collider other) {
		GameObject go = other.gameObject;
		if (go.tag == "Pickup") {
			PickUp otherPickUp = go.GetComponent<PickUp> ();
			otherPickUp.GetPickedUp ();
		}
	}

    
    public void ClearInput() {
        mPhase = MPhase.idle;
    }

    public override void takeDamage(float damage, GameObject enemy)
    {
        base.takeDamage(damage, enemy);
    }

}

