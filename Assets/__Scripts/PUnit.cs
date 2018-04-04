using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //Enables LINQ queries

//The MPhase enum is used to track the phase of mouse interaction
public enum MPhase {
    idle,
    down,
    drag
}

//MouseInfor stores info about the mouse in each frame of interaction




//mage is a subclass of PT_MonoBehavior
public class PUnit : PT_MonoBehaviour {
    static public PUnit S;

    static public bool DEBUG = true;

    public GameObject tapIndicatorPrefab;

    public float mTapTime = 0.5f; //how long is considered a tap
    public float mDragDist = 10; //how long is considered a drag

    public float activeScreenWidth = 1; //the % of the screen to usee

    public float speed = 2; //the speed at which _Mage walks

    public GameObject haloPrefab;
    //these are the min and max distance between two line points

    public bool __________________________________;

    public GameObject halo;

    public bool _selected;

    private Transform viewCharacterTrans;

    protected float lineZ = -0.1f;

    public MPhase mPhase = MPhase.idle;
    public List<MouseInfo> mouseInfos = new List<MouseInfo>();
    public string actionStartTag; //["mage", "Ground", "Enemy"]

    public bool walking = false;
    public Vector3 walkTarget;
    public Transform characterTrans;

    void Awake() {
        S = this;
        this.selected = false;
        mPhase = MPhase.idle;
        //this.GetComponent<Rigidbody>().transform.position.z = 0;
        //find the characterTrans to rotate with Face()
        characterTrans = transform.Find("CharacterTrans");
        viewCharacterTrans = characterTrans.Find("View_Character");

        halo = Instantiate(haloPrefab) as GameObject;
        halo.transform.parent = this.transform;
        halo.GetComponent<Renderer>().enabled = false;
        halo.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);

    }

    public void toggleHalo() {
        if(selected) {
            halo.GetComponent<Renderer>().enabled = true;
        }
        else {
            halo.GetComponent<Renderer>().enabled = false;
        }
    }

    void Update() {
        toggleHalo();
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
    
    void MouseDown() {
        if (DEBUG) print("Mage.MouseDown()");

        GameObject clickedGO = mouseInfos[0].hitInfo.collider.gameObject;
        //if mouse wasnt clicked on anything, this throuws an error, however mousedown only calls when clicked on something,, so we're safe

        GameObject taggedParent = Utils.FindTaggedParent(clickedGO);
        if(taggedParent == null) {
            actionStartTag = "";
        } else {
            actionStartTag = taggedParent.tag;
            //this should be either ground, mage, or enemy
        }
    }

    void RightClick()
    {
        if (DEBUG) print("Mage.RightClick()");

        //now this cares what was tapped
        switch (actionStartTag) {
            case "Mage":
                break;
            case "Ground":
                WalkTo(lastMouseInfo.loc);
                //ShowTap(lastMouseInfo.loc);
                break;
            default:
                WalkTo(lastMouseInfo.loc);
                break;
        }
    }
    void MouseDrag()
    {
        if (DEBUG) print("Mage.MouseDrag()");
        //WalkTo(mouseInfos[mouseInfos.Count - 1].loc);

        //drag is meaningless unless the mouse started on teh ground
        if (actionStartTag != "Ground") return;

        //if there is no element selected, the player should follow the mouse
        WalkTo(mouseInfos[mouseInfos.Count - 1].loc);

    }
    void MouseDragUp()
    {
        if (DEBUG) print("Mage.MouseDragUp()");

        if (actionStartTag != "Ground") return;


        StopWalking();

        //Stop walking
    }

    public void WalkTo(Vector3 xTarget) {
        walkTarget = xTarget; //set theh point to walk to
        walkTarget.z = 0; //force z=0
        walking = true;
        Face(walkTarget); //look in the direction of walktarget
    }

    public void Face(Vector3 poi) {
        Vector3 delta = poi - pos;
        //use atan2 to get the rotation around z that ponts the x axis of mage:charactertrans towards poi
        float rZ = Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x);
        //set the rotation of charactwertrans (doesnt rotate just yet)
        characterTrans.rotation = Quaternion.Euler(0, 0, rZ);
    }

    public void StopWalking() {
        walking = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void FixedUpdate() {//happens every physics step, 50 times per second


        if(walking) {
            if((walkTarget-pos).magnitude<speed*Time.fixedDeltaTime) {
                //if mage is very close to walktarget, just stop
                pos = walkTarget;
                StopWalking();
            } else {
                //otherwise, walk                
                GetComponent<Rigidbody>().velocity = (walkTarget - pos).normalized * speed;
            }
        } else {
            //if not walking, velocity should be zero
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

    }

   
    //show where the player tapped
    /*public void ShowTap(Vector3 loc) {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }*/


    // LineRenderer Code ------------------------------------------------------------------------\\

    
    public void ClearInput() {
        mPhase = MPhase.idle;
    }

    public bool selected {
        get { return _selected; }
        set { _selected = value; }
    }
}

