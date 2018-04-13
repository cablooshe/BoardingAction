using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	[Header("Set Dynamically: Room")]
	// Bounds for room position
	public float				leftMax;
	public float				rightMax;
	public float 				upMax;
	public float				downMax;

	[SerializeField]
	protected bool				_oxygen;						// Is there oxygen in the room
	[SerializeField]
	protected bool				visible;						// Is the room visible to the player

	public bool oxygen {
		get { return _oxygen; }
		set { _oxygen = value; }
	}

	void Awake() {
		oxygen = true;
		visible = false;
		leftMax = rightMax = transform.GetChild (0).position.x;
		upMax = downMax = transform.GetChild (0).position.y;
		for (int i = 1; i < transform.childCount; i++) {
			if (transform.GetChild (i).position.x < leftMax)
				leftMax = transform.GetChild (i).position.x;
			if (transform.GetChild (i).position.x > rightMax)
				rightMax = transform.GetChild (i).position.x;
			if (transform.GetChild (i).position.y > upMax)
				upMax = transform.GetChild (i).position.y;
			if (transform.GetChild (i).position.y < downMax)
				downMax = transform.GetChild (i).position.y;
		}
	}

	public bool isInRoom(Vector3 pos) {
		if ((leftMax < pos.x) && (pos.x < rightMax) && (downMax < pos.y) && (pos.y < upMax))
			return true;
		return false;
	}

	public void makeVisible() {
		if (visible)
			return;

		// Do whatever makes the room visible

		visible = true;
	}
}
