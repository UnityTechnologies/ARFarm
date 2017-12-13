using UnityEngine;
using System.Collections.Generic;

//[AddComponentMenu("Shepherd/Sheer O Matic")]
public class SheerOMatic : MonoBehaviour 
{
	// Helpers for positioning the sheep for animations
	public Transform entrancePoint;
	public Transform exitPoint;

	// The 'ejected' sheep to use after processing
	public GameObject nakedSheepPrefab;
	
	// Animation hints for when to move or change the sheep
	public float suctionTime = 1f;
	public float sheerTime = 1f;
	public float ejectTime = 1f;

	// Animator to control what the machine itself is doing
	public Animator animator;

	private List<Sheep> sheepToProcess = new List<Sheep>();
	private GameObject activeObject = null;

	private enum SheerPhase
	{
		idle = 0,
		suction = 1,
		backwardsSuction = 2,
		sheering = 3,
		ejection = 4
	}
	private SheerPhase sheerPhase = SheerPhase.idle;
	private float phaseTimer = 0f;
	private Vector3 animationBasePosition = Vector3.zero;
	private Quaternion animationBaseRotation = Quaternion.identity;
	private Quaternion backwardsEntranceRotation;

	// Use this for initialization
	void Start () 
	{
		backwardsEntranceRotation = Quaternion.Euler(0, 180, 0) * entrancePoint.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{
		phaseTimer += Time.deltaTime;
		switch (sheerPhase)
		{
			case SheerPhase.idle:
			{ 
				// See if we have a sheep to sheer
				var sheerIndex = (sheepToProcess.Count - 1);
				if (sheerIndex >= 0)
				{
					var newSheep = sheepToProcess[sheerIndex];
					activeObject = newSheep.gameObject;
					sheepToProcess.RemoveAt(sheerIndex);


					// We begin the sheering process!
					if (newSheep.runFromMachine)
					{
						sheerPhase = SheerPhase.backwardsSuction;
					}
					else
					{
						sheerPhase = SheerPhase.suction;
					}

					// Remove the sheep component so the sheep stops trying to wander or flock
					Destroy(newSheep);

					phaseTimer = 0f;
					animationBasePosition = activeObject.transform.position;
					animationBaseRotation = activeObject.transform.rotation;
					animator.SetTrigger("TurnOn");

					// Sheep are scared!
					var sheepAnimator = activeObject.GetComponent<Animator>();
					sheepAnimator.SetTrigger("Suction");
					sheepAnimator.SetBool("Cinematic", true);
				}
			}
			break;

			case SheerPhase.suction:
			{
				var phasePercent = Mathf.Clamp01(phaseTimer / suctionTime);

				// Move the sheep to the desired position and orientation
				var suctionPosition = Vector3.Lerp(animationBasePosition, entrancePoint.position, phasePercent);
				suctionPosition.y = animationBasePosition.y;
				activeObject.transform.position = suctionPosition;

				activeObject.transform.rotation = Quaternion.Lerp(animationBaseRotation, entrancePoint.rotation, phasePercent);

				// At the end, destroy the object and go to the next phase
				if (phasePercent >= 1)
				{
					sheerPhase = SheerPhase.sheering;
					phaseTimer = 0f;
					Destroy(activeObject);
					activeObject = null;
				}
			}
			break;

			case SheerPhase.backwardsSuction:
			{
				// Backwards mode is a little more scripted rather than animation based
				var positionTime = suctionTime * .8f;

				var positionPercent = Mathf.Clamp01(phaseTimer / positionTime);
				var suctionPercent =  Mathf.Clamp01((phaseTimer - positionTime) / (1f - positionTime));

				// Move the sheep to the desired position and orientation
				var suctionPosition = Vector3.Lerp(animationBasePosition, entrancePoint.position, positionPercent) + (entrancePoint.forward*suctionPercent*3);
				suctionPosition.y = animationBasePosition.y;
				activeObject.transform.position = suctionPosition;

				activeObject.transform.rotation = Quaternion.Lerp(animationBaseRotation, backwardsEntranceRotation, positionPercent);

				// At the end, destroy the object and go to the next phase
				if (suctionPercent >= 1)
				{
					sheerPhase = SheerPhase.sheering;
					phaseTimer = 0f;
					Destroy(activeObject);
					activeObject = null;
				}
			}
			break;

			case SheerPhase.sheering:
			{
				// For now, just wait
				var phasePercent = Mathf.Clamp01(phaseTimer / sheerTime);

				// At the end, create the naked sheep and go to the next phase
				if (phasePercent >= 1)
				{
					sheerPhase = SheerPhase.ejection;
					phaseTimer = 0f;
					Instantiate(nakedSheepPrefab, exitPoint.position, exitPoint.rotation);
				}
			}
			break;

			case SheerPhase.ejection:
			{ 
				var phasePercent = Mathf.Clamp01(phaseTimer / ejectTime);
				if (phasePercent >= 1)
				{
					// Just go back to idle
					sheerPhase = SheerPhase.idle;
					phaseTimer = 0f;
				}
			}
			break;
		}
	}

	void OnTriggerEnter(Collider triggeree)
	{
		// Check if the object in sheer-o-matic's grabby range is a sheep
		var toSheer = triggeree.GetComponent<Sheep>();
		if (toSheer == null)
		{
			return;
		}

		// Add it to the list, if it is not already there
		if (sheepToProcess.Contains(toSheer) == false)
		{
			sheepToProcess.Add(toSheer);
		}
	}

	void OnTriggerExit(Collider triggeree)
	{
		// Check if the object leaving the sheer-o-matic's grabby range is a sheep
		var toSheer = triggeree.GetComponent<Sheep>();
		if (toSheer == null)
		{
			return;
		}

		// If it is, remove it from the list
		sheepToProcess.Remove(toSheer);
	}
}
