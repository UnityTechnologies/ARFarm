//This script is used to teleport the player around the VR scene

using UnityEngine;

public class PlayerMover : MonoBehaviour 
{
	[SerializeField] float playerHeight = 2.5f;		//How high off the ground should the player be
	[SerializeField] ReticuleMover reticle;			//A reticle object used to identify where the player is looking
	[SerializeField] ParticleSystem teleportEffect;	//A particle effect to play when the player teleports
	[SerializeField] LayerMask whatIsGround;		//A layer mask defining what layers constitute the ground

	Vector3 startingPosition;	//The original position of the player
	UnityEngine.AI.NavMeshHit navHitInfo;		//Where on a navmesh the player is looking
	bool isValid;				//Is the player currently looking at a valid place to teleport?

	void Start ()
	{
		//Cache the original position for use later
		startingPosition = transform.parent.position;
		//startingPosition = transform.position;
	}

	void Update ()
	{
		UpdateReticule ();
		MovePosition ();
	}

	void UpdateReticule()
	{
		//Assume we don't have a valid position
		isValid = false;

		//Generate a ray at the camera facing directly forward
		Ray ray = new Ray(transform.position, transform.forward);
		RaycastHit hit;

		//If this ray hits something on the ground layer...
		if (Physics.Raycast(ray, out hit, 1000, whatIsGround))
		{
			//...look at the navmesh to determine if the ray is within 5 units of it. If it is...
			if (UnityEngine.AI.NavMesh.SamplePosition (hit.point, out navHitInfo, 5, UnityEngine.AI.NavMesh.AllAreas)) 
			{
				//...we have a valid position
				isValid = true;

				//If we have a reticle, move it to the point where the ray meets the navmesh
				if (reticle != null)
					reticle.SetTarget (navHitInfo.position);
			} 
		}
	}

	void MovePosition()
	{
		//If our current looking position isn't valid, leave
		if (!isValid)
			return;

		//If we press "Fire1"...
		if ( Input.GetButtonDown ("Jump")) 
		{
			//...move our position to the point on the navmesh plus a little extra height (so it doesn't
			//feel like our heads are laying directly on the ground)
			transform.parent.position = navHitInfo.position + new Vector3 (0f, playerHeight, 0f);
			//If we have a teleport particle effect, play it
			if(teleportEffect != null)
				teleportEffect.Play ();	
		}

		//If we press escape or a back button...
		if(Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.Escape))
		{
			//...move back to the original position
			transform.parent.position = startingPosition;

			//If we have a teleport particle effect, play it
			if(teleportEffect != null)
				teleportEffect.Play ();
		}
	}
}
