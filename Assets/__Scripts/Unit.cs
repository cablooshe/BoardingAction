using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : PT_MonoBehaviour {
    static public Unit S;

    static public bool DEBUG = true;

    public float speed = 2; //the speed at which unit walks

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

    public bool selected
    {
        get { return _selected; }
        set { _selected = value; }
    }


    // Use this for initialization
    void Start () {
        S = this;
        this.selected = false;
        //find the characterTrans to rotate with Face()
        characterTrans = transform.Find("CharacterTrans");
        transforms.Add(characterTrans.Find("SquadLeader"));
        transforms.Add(characterTrans.Find("Member1"));
        transforms.Add(characterTrans.Find("Member2"));

        //viewCharacterTrans = characterTrans.Find("View_Character");

        halo = Instantiate(haloPrefab) as GameObject;
        halo.transform.parent = this.transform;
        halo.transform.position = new Vector3(halo.transform.position.x - .23f, halo.transform.position.y + .05f, halo.transform.position.z);
        halo.GetComponent<Renderer>().enabled = false;
        halo.transform.position = new Vector3(this.pos.x, this.pos.y, this.pos.z - 0.15f);
    }
	
	// Update is called once per frame
	void Update () {
		
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


}
