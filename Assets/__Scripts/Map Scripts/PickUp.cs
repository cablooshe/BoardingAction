using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
	public enum PickUpType { primaryObjective, secondaryObjective };

	[Header("PickUp: Set in Inspector")]
	public PickUpType itemType;

	 
	public void GetPickedUp() {
		// This will do more later, but for now...
		Destroy(this.gameObject);
	}
}
