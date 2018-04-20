using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour {

	[Header("Set in Inspector")]
	public bool				isVertical = true;

	[Header("Set Dynamically")]
	public bool				isOpen = false;		// Is the door currently open?

	public GameObject 		door1; 				// Lower x/y-positioned door
	public GameObject 		door2; 				// Higher x/y-positioned door
	public float			closeTime;			// Time to close the doors

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
		} else {
			if (door1.transform.position.x < temp.transform.position.x)
				door2 = temp;
			else {
				door2 = door1;
				door1 = temp;
			}
		}
		closeTime = Time.time;
	}

	void Update() {
		if (isOpen && (Time.time > closeTime)) {
			CloseDoors ();
		}
	}		

	public void OpenDoors() {
		if (isOpen)
			return;
		if (isVertical) {
			door1.transform.position = new Vector3 (door1.transform.position.x, door1.transform.position.y - 1, door1.transform.position.z);
			door2.transform.position = new Vector3 (door2.transform.position.x, door2.transform.position.y + 1, door2.transform.position.z);
		} else {
			door1.transform.position = new Vector3 (door1.transform.position.x - 1, door1.transform.position.y, door1.transform.position.z);
			door2.transform.position = new Vector3 (door2.transform.position.x + 1, door2.transform.position.y, door2.transform.position.z);
		}
		isOpen = true;
		closeTime = Time.time + 5;
	}

	public void CloseDoors() {
		if (!isOpen)
			return;
		if (isVertical) {
			door1.transform.position = new Vector3 (door1.transform.position.x, door1.transform.position.y + 1, door1.transform.position.z);
			door2.transform.position = new Vector3 (door2.transform.position.x, door2.transform.position.y - 1, door2.transform.position.z);
		} else {
			door1.transform.position = new Vector3 (door1.transform.position.x + 1, door1.transform.position.y, door1.transform.position.z);
			door2.transform.position = new Vector3 (door2.transform.position.x - 1, door2.transform.position.y, door2.transform.position.z);
		}
		isOpen = false;
	}
}
