using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour {

	[Header("Set in Inspector")]
	public bool				isVertical = true;

	[Header("Set Dynamically")]
	public bool				isOpen = false;		// Is the door currently open?
	public bool				opening = false; 	// Is the door in the middle of opening?
	public bool				closing = false;	// Is the door in the middle of closing?

	public GameObject 		door1; 				// Lower x/y-positioned door
	public GameObject 		door2; 				// Higher x/y-positioned door

	public float			startOpen;			// When the doors started opening
	public float			finishOpen;			// Time for the doors to be completely open
	public float			startClose;			// When the doors started closing
	public float 			finishClose; 		// Time for the doors to be completely closed
	public float			closeTime;			// Time to start closing the doors

	public float			startPos1;			// Starting position for door1
	public float			startPos2;			// Starting position for door2

	void Awake() {
		door1 = transform.GetChild (0).gameObject;
		GameObject temp = transform.GetChild (1).gameObject;
		if (isVertical) {
			if (door1.transform.position.y < temp.transform.position.y)
				door2 = temp;
			else {
				door2 = door1;
				door1 = temp;
			}
			startPos1 = door1.transform.position.y;
			startPos2 = door2.transform.position.y;
		} else {
			if (door1.transform.position.x < temp.transform.position.x)
				door2 = temp;
			else {
				door2 = door1;
				door1 = temp;
			}
			startPos1 = door1.transform.position.x;
			startPos2 = door2.transform.position.x;
		}
		closeTime = Time.time;
	}

	void Update() {
		if (opening) {
			float openStatus = Mathf.Min (Time.time - startOpen, 1);
			if (isVertical) {
				door1.transform.position = new Vector3 (door1.transform.position.x, startPos1 - openStatus, door1.transform.position.z);
				door2.transform.position = new Vector3 (door2.transform.position.x, startPos2 + openStatus, door2.transform.position.z);
			} else {
				door1.transform.position = new Vector3 (startPos1 - openStatus, door1.transform.position.y, door1.transform.position.z);
				door2.transform.position = new Vector3 (startPos2 + openStatus, door2.transform.position.y, door2.transform.position.z);
			}
			if (Time.time > finishOpen) {
				opening = false;
				closeTime = Time.time + 5;
			}
		} else if (closing) {
			float closeStatus = Mathf.Max (finishClose - Time.time, 0);
			if (isVertical) {
				door1.transform.position = new Vector3 (door1.transform.position.x, startPos1 - closeStatus, door1.transform.position.z);
				door2.transform.position = new Vector3 (door2.transform.position.x, startPos2 + closeStatus, door2.transform.position.z);
			} else {
				door1.transform.position = new Vector3 (startPos1 - closeStatus, door1.transform.position.y, door1.transform.position.z);
				door2.transform.position = new Vector3 (startPos2 + closeStatus, door2.transform.position.y, door2.transform.position.z);
			}
			if (Time.time > finishClose) {
				closing = false;
			}
		}
		if (isOpen && (Time.time > closeTime)) {
			CloseDoors ();
		}
	}		

	public void OpenDoors() {
		if (isOpen || opening || closing)
			return;
		isOpen = true;
		opening = true;
		startOpen = Time.time;
		finishOpen = Time.time + 1;
	}

	public void CloseDoors() {
		if (!isOpen || closing || opening)
			return;
		isOpen = false;
		closing = true;
		startClose = Time.time;
		finishClose = Time.time + 1;
	}
}
