using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries



//mage is a subclass of PT_MonoBehavior
public class PUnit : Unit {

	[Header("PUnit: Associated Prefabs - Set in Inspector")]
    public GameObject tapIndicatorPrefab;
    public GameObject explosion;
  


    [Header("PUnit: Mouse Info")]
    public float mTapTime = 0.5f; //how long is considered a tap
    public float mDragDist = 10; //how long is considered a drag
	public float activeScreenWidth = 1; //the % of the screen to use
	public MPhase mPhase = MPhase.idle;
    public bool prepGrenade = false;
    public float grenadeCoolDown = 10f;
    public float grenadeRange = 15f;
   
    public bool grenadeReady = true;
    
    
    

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
        
    }
    new public void StopWalking() {
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        anim.SetBool("Walking", false);
    }

    public void dropCover() {
        Debug.Log("COVER DROPPED");
    }

    void updateAnimation() {
        if (isTargeting)
        {
            anim.SetBool("Attacking", true);

        }
        //else { anim.SetBool("Attacking", false); }
        if (walking)
        {
            anim.SetBool("Walking", true);
        }
        else
            anim.SetBool("Walking", false);
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

      
        if(timestamp <= Time.time && !grenadeReady)
        {
            
            print("grenade ready");
            grenadeReady = true;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
        }

        if (Input.GetKey(KeyCode.Q) && !prepGrenade)
        {
           if (timestamp <= Time.time)
            {
                readyGrenade();
            } else
            {
                print("grenade not ready");                
            }
        }

    }

    void readyGrenade()
    {
        prepGrenade = true;
        walking = false;
        halo.GetComponent<SelectionHalo>().mat.color = Color.red;
    }

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
		// We might need this later, but now it is USELESS (almost as much as Gordon)
    }
    override public void MouseDragUp()
    {
        // We might need this later, but now it is USELESS (almost as much as Gordon's comments)
    }

    void throwGrenade(Vector3 xTarget)
    {
        float dist = Vector3.Distance(transform.position, xTarget);
        int structure = 2;
        int door = 4;
        int layerM1 = 1 << structure;
        int layerM2 = 1 << door;
        int layerMask = layerM1 | layerM2;
        if (Physics.Raycast(transform.position, xTarget, dist,~(2 | 4)))
        {
            print("collide");
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
            return;
        }

        if (dist < grenadeRange)
        {
            GameObject exp;
            exp = Instantiate(explosion) as GameObject;
            exp.transform.position = xTarget;
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.blue;
            timestamp = Time.time + grenadeCoolDown;
            grenadeReady = false;
        }
        else
        {
            print("too far");
            prepGrenade = false;
            halo.GetComponent<SelectionHalo>().mat.color = Color.green;
        }
    }



   
    //show where the player tapped
    /*public void ShowTap(Vector3 loc) {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }*/

    void OnCollisionEnter (Collision c) {
        GameObject go = c.gameObject;      
        if (go.tag == "PUnit" && go.GetComponent<PUnit>().walking == false) {
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

}

