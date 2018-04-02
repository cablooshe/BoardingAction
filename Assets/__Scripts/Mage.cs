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

//the elementtype enum
public enum ElementType {
    earth,
    water,
    air,
    fire,
    aether,
    none
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
public class Mage : PT_MonoBehaviour {
    static public Mage S;

    static public bool DEBUG = true;

    public float mTapTime = 0.1f; //how long is considered a tap

    public GameObject tapIndicatorPrefab; //prefab of the tap indicator

    public float mDragDist = 5; //how long is considered a drag

    public float activeScreenWidth = 1; //the % of the screen to usee

    public float speed = 2; //the speed at which _Mage walks

    public GameObject[] elementPrefabs; //the Element_Sphere prefabs
    public float elementRotDist = 0.5f;
    public float elementRotSpeed = 0.5f;
    public int maxNumSelectedElements = 1;
    public Color[] elementColors;

    //these are the min and max distance between two line points
    public float lineMinDelta = 0.1f;
    public float lineMaxDelta = 0.5f;
    public float lineMaxLength = 8f;

    public GameObject fireGroundSpellPrefab;

    public float health = 4;
    public float damageTime = -100;
    public float knockbackDist = 1;
    public float knockbackDur = 0.5f;
    public float invincibleDur = 0.5f;
    public int invTimesToBlink = 4;

    public bool __________________________________;

    private bool invincibleBool = false;
    private bool knockbackBool = false;
    private Vector3 knockbackDir;
    private Transform viewCharacterTrans;

    protected Transform spellAnchor; //the parent transform for all spells

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

    public List<Element> selectedElements = new List<Element>();

    void Awake() {
        S = this;
        mPhase = MPhase.idle;
        //this.GetComponent<Rigidbody>().transform.position.z = 0;
        //find the characterTrans to rotate with Face()
        characterTrans = transform.Find("CharacterTrans");
        viewCharacterTrans = characterTrans.Find("View_Character");

        //get the linRenderer component and disable it
        liner = GetComponent<LineRenderer>();
        liner.enabled = false;

        GameObject saGO = new GameObject("Spell Anchor");
        //creates an empty gameObject names "spell anchor".  when youcreate a new gameobject this way, its at P: [0,0,0] S: [1,1,1]
        spellAnchor = saGO.transform; //get its transform


    }

    void Update() {
        //find whether the mouse button 0 was pressed or released this frame
        bool b0Down = Input.GetMouseButtonDown(0);
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
            if(b0Down && inActiveArea) {
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
            if(b0Up) {
                MouseTap();
                mPhase = MPhase.idle;
            } else if (Time.time - mouseInfos[0].time > mTapTime) {
                //if its been down longer for a tap, it may be a drag, but to be a drag, it must also have moved a certain number of pixels on screen
                float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;
                if(dragDist >=mDragDist) {
                    mPhase = MPhase.drag;
                }

                //however, drag will immediately start after mTapTime if there are no elements selected
                if (selectedElements.Count == 0) {
                    mPhase = MPhase.drag;
                }
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

        OrbitSelectedElements();
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
    void MouseTap()
    {
        if (DEBUG) print("Mage.MouseTap()");

        //now this cares what was tapped
        switch (actionStartTag) {
            case "Mage":
                break;
            case "Ground":
                WalkTo(lastMouseInfo.loc);
                ShowTap(lastMouseInfo.loc);
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
        if (selectedElements.Count == 0) {
            WalkTo(mouseInfos[mouseInfos.Count - 1].loc);
        } else {
            AddPointToLiner(mouseInfos[mouseInfos.Count - 1].loc);

        }
    }
    void MouseDragUp()
    {
        if (DEBUG) print("Mage.MouseDragUp()");

        if (actionStartTag != "Ground") return;

        //if there is no element selected, the player should follow the mouse
        if (selectedElements.Count == 0)
        {
            StopWalking();
        } else {

            CastGroundSpell();
            //clear the liner
            ClearLiner();
        }

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
        if (invincibleBool) {
            float blinkU = (Time.time - damageTime) / invincibleDur;
            blinkU *= invTimesToBlink;
            blinkU %= 1.0f;
            bool visible = (blinkU > 0.5f);
            if (Time.time - damageTime > invincibleDur) {
                invincibleBool = false;
                visible = true;
            }

            viewCharacterTrans.gameObject.SetActive(visible);
        }

        if(knockbackBool) {
            if (Time.time - damageTime > knockbackDur) {
                knockbackBool = false;
            }
            float knockbackSpeed = knockbackDist / knockbackDur;
            vel = knockbackDir * knockbackSpeed;
            return;
        }


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

    void OnCollisionEnter(Collision coll) {
        GameObject otherGo = coll.gameObject;
        //oclliding with a wall can also stop waling
        Tile ti = otherGo.GetComponent<Tile>();
        if(ti != null) {
            if(ti.height > 0) {
                StopWalking();
            }
        }

        EnemyBug bug = coll.gameObject.GetComponent<EnemyBug>();
        if (bug != null) CollisionDamage(bug);
        //if (bug != null) CollisionDamage(otherGo);
    }

    void OnTriggerEnter(Collider other) {
        EnemySpiker spiker = other.GetComponent<EnemySpiker>();
        if (spiker != null) {
            CollisionDamage(spiker);
            //CollisionDamage(other.gameObject);
        }
    }

    void CollisionDamage(Enemy enemy) {
        if (invincibleBool) return;
        StopWalking();
        ClearInput();

        health -= enemy.touchDamage;
        if (health <= 0) {
            Die();
            return;
        }

        damageTime = Time.time;
        knockbackBool = true;
        knockbackDir = (pos - enemy.pos).normalized;
        invincibleBool = true;
    }

    //mage dies
    void Die() {
        Application.LoadLevel(0);
    }

    //show where the player tapped
    public void ShowTap(Vector3 loc) {
        GameObject go = Instantiate(tapIndicatorPrefab) as GameObject;
        go.transform.position = loc;
    }

    public void SelectElement(ElementType elType) {
        if(elType == ElementType.none) {
            ClearElements();
            return;
        }

        if(maxNumSelectedElements == 1) {
            //if only one element allowed, clear the current one
            ClearElements();
        }

        //cant select more than maxNumSelectedElements simultaneously
        if (selectedElements.Count >= maxNumSelectedElements) return;

        //its okay to add this element
        GameObject go = Instantiate(elementPrefabs[(int)elType]) as GameObject;
        //Note the typecast from elementtype to int in the line above
        Element el = go.GetComponent<Element>();
        el.transform.parent = this.transform;
        el.type = elType;

        selectedElements.Add(el); //add el to thte list of elements
    }

    //clears all elements form selected elements and destroys theeir gameobjects
    public void ClearElements() {
        foreach(Element el in selectedElements) {
            //destroy each gameobject in the list
            Destroy(el.gameObject);
        }
        selectedElements.Clear();
    }

    //valled every update to orbit the elements around
    void OrbitSelectedElements() {
        //if there are none sellected, just return
        if (selectedElements.Count == 0) return;

        Element el;
        Vector3 vec;
        float theta0, theta;
        float tau = Mathf.PI * 2;
        //difive thee circle nto the number of elements that are orbiting
        float rotPerElement = tau / selectedElements.Count;

        //the base rotation angle is set based on time
        theta0 = elementRotSpeed * Time.time * tau;

        for (int i = 0; i < selectedElements.Count;i++) {
            //determine the rotation angle for each element
            theta = theta0 + i * rotPerElement;
            el = selectedElements[i];
            //use simple trig to turun the angle into a unit vector
            vec = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0);
            //,ultiply the vec by the elementRotDist
            vec *= elementRotDist;
            //raise the element to waist height
            vec.z = -0.5f;
            el.lPos = vec;//set the position of hte element sphere
        }
    }

    // LineRenderer Code ------------------------------------------------------------------------\\

    //add a new point to the line
    void AddPointToLiner(Vector3 pt) {
        pt.z = lineZ;

        //linePts.Add(pt);
        //UpdateLiner();

        //add the point if linePts is empty
        if (linePts.Count == 0) {
            linePts.Add(pt);
            totalLineLength = 0;
            return; //but wait a bit to update the linerenderer
        }

        if (totalLineLength > lineMaxLength) return;

        Vector3 pt0 = linePts[linePts.Count - 1]; //get the last point in linePts
        Vector3 dir = pt - pt0;
        float delta = dir.magnitude;
        dir.Normalize();

        totalLineLength += delta;

        //if less than mnii distance
        if (delta < lineMinDelta) {
            //then dont add it
            return;
        }

        //if its further than the max distance then extra points
        if (delta > lineMaxDelta) {
            //add extra points
            float numToAdd = Mathf.Ceil(delta / lineMaxDelta);
            float midDelta = delta / numToAdd;
            Vector3 ptMid;
            for (int i = 1; i < numToAdd; i++) {
                ptMid = pt0 + (dir * midDelta * i);
                linePts.Add(ptMid);
            }
        }

        linePts.Add(pt);
        UpdateLiner();
    }

    //update the linerenderer with the new points
    public void UpdateLiner() {
        //get the type of selectedElement
        int el = (int)selectedElements[0].type;

        //set the line color based on that type
        //liner.SetColors(elementColors[el], elementColors[el]);
        liner.startColor = elementColors[el];
        liner.endColor = elementColors[el];

        //Update the representation of the ground spell about to be cast
        liner.positionCount = linePts.Count;
        for(int i = 0; i < linePts.Count; i++) {
            liner.SetPosition(i, linePts[i]);
        }
        liner.enabled = true;
    }

    public void ClearLiner() {
        liner.enabled = false;
        linePts.Clear();
    }
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    void CastGroundSpell() {
        if (selectedElements.Count == 0) return;

        //because this version of the prototype only allows a single element to be selected, we can use that 0th element to pick the espell
        switch (selectedElements[0].type) {
            case ElementType.fire:
                GameObject fireGO;
                foreach(Vector3 pt in linePts) {
                    fireGO = Instantiate(fireGroundSpellPrefab) as GameObject;
                    fireGO.transform.parent = spellAnchor;
                    fireGO.transform.position = pt;
                }
                break;

                //TODO: add other element types later

        }
        ClearElements();
    }

    public void ClearInput() {
        mPhase = MPhase.idle;
    }
}

