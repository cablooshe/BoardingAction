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
[System.Serializable]
public class MouseInfo {
    public Vector3 loc; //location of the mouse near z=0
    public Vector3 screenLoc; //screen position of the mouse
    public Ray ray;//ray from the mouse into 3d space
    public float time;//time this mouseInfo was recorded 
    public RaycastHit hitInfo;//info aobut what was hit by the ray
    public bool hit; //whether the mouse was over any collider


    //these methods see if the mouseRay hits anything
    public RaycastHit Raycast() {
        hit = Physics.Raycast(ray, out hitInfo);
        return hitInfo;
    }

    public RaycastHit Raycaset(int mask) {
        hit = Physics.Raycast(ray, out hitInfo, mask);
        return hitInfo;
    }
}



//mage is a subclass of PT_MonoBehavior
public class PUnit : PT_MonoBehaviour {
    static public PUnit S;

    static public bool DEBUG = true;

    public GameObject tapIndicatorPrefab;

    public float mTapTime = 0.1f; //how long is considered a tap
    public float mDragDist = 5; //how long is considered a drag

    public float activeScreenWidth = 1; //the % of the screen to usee

    public float speed = 2; //the speed at which _Mage walks

    //these are the min and max distance between two line points

    public bool __________________________________;

    public bool _selected;



    private Transform viewCharacterTrans;

    public float totalLineLength;

    public List<Vector3> linePts;
    protected LineRenderer liner;
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

        //get the linRenderer component and disable it
        liner = GetComponent<LineRenderer>();
        liner.enabled = false;

        //creates an empty gameObject names "spell anchor".  when youcreate a new gameobject this way, its at P: [0,0,0] S: [1,1,1]


    }

    void Update() {

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
            } else if (Time.time - mouseInfos[0].time > mTapTime) {
                //if its been down longer for a tap, it may be a drag, but to be a drag, it must also have moved a certain number of pixels on screen
                float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;
                if(dragDist >=mDragDist) {
                    mPhase = MPhase.drag;
                }

                //however, drag will immediately start after mTapTime if there are no elements selected
                mPhase = MPhase.drag;
                
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

