//This script is used to smoothly move the reticle to a position

using UnityEngine;

public class ReticuleMover : MonoBehaviour 
{
	public float speed = 5f;	//How fast the reticle moves

	Vector3 targetPos;			//Where the reticle is trying to go		

	void Start () 
	{
		//Our starting target is our current position
		targetPos = transform.position;
	}

	void Update () 
	{
		//Smoothly "Lerp" in between our current position and where we want to go
		transform.position = Vector3.Lerp (transform.position, targetPos, speed * Time.deltaTime);
	}

	//A method for other objects to set the position of the reticle
	public void SetTarget(Vector3 newTarget)
	{
		//Record the new position
		targetPos = newTarget;
	}
}
