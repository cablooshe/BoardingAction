using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {
	public enum PickUpType { primaryObjective, secondaryObjectiveCouncil, secondaryObjectiveKai };

	[Header("PickUp: Set in Inspector")]
	public PickUpType itemType;
	public int posFavor = 1;
    public int negFavor = 0;
    public float rotationsPerSecond = 0.1f;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }


    public void GetPickedUp() {
        // This will do more later, but for now...
		if (itemType == PickUpType.secondaryObjectiveCouncil) {
			PlayerInfo.CouncilFavor = PlayerInfo.CouncilFavor + posFavor;
			PlayerInfo.KaiFavor = PlayerInfo.KaiFavor - negFavor;
			print ("The Council is Pleased.");
		} else if (itemType == PickUpType.secondaryObjectiveKai) {
			PlayerInfo.KaiFavor = PlayerInfo.KaiFavor + posFavor;
			PlayerInfo.CouncilFavor = PlayerInfo.CouncilFavor - negFavor;
			print ("Admiral Kai is pleased.");
		} else
        	this.GetComponentInParent<Map>().CompletedObjective();
		Destroy(this.gameObject);
	}
}
