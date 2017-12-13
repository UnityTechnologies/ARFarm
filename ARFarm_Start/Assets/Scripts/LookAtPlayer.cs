//This script is used to make UI elements face the player's camera

using UnityEngine;

public class LookAtPlayer : MonoBehaviour 
{
	public Transform player;	//The player's transform

	//Update after all other updates have run
	void LateUpdate()
	{
		//Apply the rotation needed to look at the player. Note, since pointing a UI text element
		//at the player makes it appear backwards, we are actually pointing this object
		//directly *away* from the player.
		transform.rotation = Quaternion.LookRotation (transform.position - player.position);
	}
}
