using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The MPhase enum is used to track the phase of mouse interaction
public enum MPhase
{
    idle,
    down,
    drag
}

//MouseInfo stores info about the mouse in each frame of interaction
public class MouseInfo
{
    public Vector3 loc; //location of the mouse near z=0
    public Vector3 screenLoc; //screen position of the mouse
    public Ray ray;//ray from the mouse into 3d space
    public float time;//time this mouseInfo was recorded 
    public RaycastHit hitInfo;//info aobut what was hit by the ray
    public bool hit; //whether the mouse was over any collider


    //these methods see if the mouseRay hits anything
    public RaycastHit Raycast()
    {
        hit = Physics.Raycast(ray, out hitInfo);
        return hitInfo;
    }

    public RaycastHit Raycaset(int mask)
    {
        hit = Physics.Raycast(ray, out hitInfo, mask);
        return hitInfo;
    }

}

public class PlayerSelect : MonoBehaviour {

    public List<GameObject> unitsSelected;
    public static List<GameObject> units;
    public bool isSelecting = false;
    public Vector3 mousePosition1;


    //Mouse selection related stuff
    public float mDragDist = 2;

    public MPhase mPhase = MPhase.idle;

    public List<MouseInfo> mouseInfos = new List<MouseInfo>();

    MouseInfo AddMouseInfo()
    {
        MouseInfo mInfo = new MouseInfo();
        mInfo.screenLoc = Input.mousePosition;
        mInfo.loc = Utils.mouseLoc; //gets the position of the mouse at z=0
        mInfo.ray = Utils.mouseRay;//gets the ray from the main cam through the mouse pointer
        mInfo.time = Time.time;
        mInfo.Raycast(); //default is to raycast with no mask

        if (mouseInfos.Count == 0)
        {
            mouseInfos.Add(mInfo);
        }
        else
        {
            float lastTime = mouseInfos[mouseInfos.Count - 1].time;
            if (mInfo.time != lastTime)
            {
                //if time has passed since the last mouseinfo
                mouseInfos.Add(mInfo);
            }
            //this time test is needed because addmouseinfo could be called twice in one frame

        }
        return mInfo;
    }

    public MouseInfo lastMouseInfo
    {
        get
        {
            if (mouseInfos.Count == 0) return null;
            return (mouseInfos[mouseInfos.Count - 1]);
        }
    }


    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds =
            Utils.GetViewportBounds(camera, mousePosition1, Input.mousePosition);

        return viewportBounds.Contains(
            camera.WorldToViewportPoint(gameObject.transform.position));
    }

    // Update is called once per frame
    void Update () {

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


        //this is handled as an if statement instead of switch because a tap can sometimes happen within a single frame
        if (mPhase == MPhase.idle)
        {
            //mouseInfos.Clear(); //clear the mouseInfos
            if(b0Down) {
                mouseInfos.Clear();
                AddMouseInfo(); //and add a first mouseinfo

            //if the mouse was clicked on something, its aavalid mousedown
                MouseDown();
                mPhase = MPhase.down;
            }
            
        }

        if (mPhase == MPhase.down)
        {
            AddMouseInfo();
            if (b0Up)
            {
                MouseTap();
                mPhase = MPhase.idle;
            }
            else
            {
                float dragDist = (lastMouseInfo.screenLoc - mouseInfos[0].screenLoc).magnitude;
                if (dragDist >= mDragDist)
                {
                    mPhase = MPhase.drag;
                }

            }
        }

        if (mPhase == MPhase.drag)
        {
            AddMouseInfo();
            if (b0Up)
            {
                MouseDragUp();
                mouseInfos.Clear();
                mPhase = MPhase.idle;
            }
            else
            {
                MouseDrag(); //still dragging
            }
        }





        if (Input.GetMouseButtonDown(0))
        {

            mPhase = MPhase.down;

            isSelecting = true;
            mousePosition1 = Input.mousePosition;
        }
        // If we let go of the left mouse button, end selection
        if (Input.GetMouseButtonUp(0)) {
            if(isSelecting) {
                foreach(GameObject u in unitsSelected) {
                    u.GetComponent<PUnit>().selected = false;
                }
                unitsSelected.Clear();
                foreach (GameObject unit in GameObject.FindGameObjectsWithTag("PUnit")) {
                    if (IsWithinSelectionBounds(unit)){
                        unitsSelected.Add(unit);
                        unit.GetComponent<PUnit>().selected = true;
                    }
                }
            }
            isSelecting = false;
        }  
    }

    public void MouseUp() {

    }

    public void MouseDown() {
        isSelecting = true;
        mousePosition1 = Input.mousePosition;
    }

    public void MouseTap() {

        if (unitsSelected.Count != 0)
        {
            foreach (GameObject u in unitsSelected)
            {
                u.GetComponent<PUnit>().selected = false;

            }
            unitsSelected.Clear();
        }
        isSelecting = false;
        RaycastHit hitInfo = new RaycastHit();
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo) && hitInfo.transform.tag == "PUnit")
        {
            GameObject uSelected = hitInfo.collider.gameObject;
            uSelected.GetComponent<PUnit>().selected = true;
            unitsSelected.Add(uSelected);
        }
    }

    public void MouseDrag() {
        //mousePosition1 = Input.mousePosition;
    }

    public void MouseDragUp() {
        if (isSelecting)
        {
            foreach (GameObject u in unitsSelected)
            {
                if (u != null)
                    u.GetComponent<PUnit>().selected = false;
            }
            unitsSelected.Clear();
            foreach (GameObject unit in GameObject.FindGameObjectsWithTag("PUnit"))
            {
                if (IsWithinSelectionBounds(unit))
                {
                    unitsSelected.Add(unit);
                    unit.GetComponent<PUnit>().selected = true;
                }
            }
        }
        isSelecting = false;
    }

    public void Select(GameObject unit) {
        foreach (GameObject u in unitsSelected)
        {
            if (u != null)
                u.GetComponent<PUnit>().selected = false;
        }
        unitsSelected.Clear();
        unit.GetComponent<PUnit>().selected = true;
        unitsSelected.Add(unit);
    }
}

