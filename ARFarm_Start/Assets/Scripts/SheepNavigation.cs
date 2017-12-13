using UnityEngine;
using System.Collections;

public class SheepNavigation : MonoBehaviour 
{
	[SerializeField] float xBound = 32f;			//Width of the arena. Used for picking a random location to drive
	[SerializeField] float zBound = 32f;			//Depth of the arena. Used for picking a random location to drive
	[SerializeField] float radius = 1.4f;			//Width of the tank's navmesh agent
	[SerializeField] float speed = 4.5f;			//Speed of the tank's navmesh agent
	[SerializeField] float stoppingDistance = 1f;	//How far in front of the tank's destination it will stop

	static int tankCount = 0;	//How many tanks have we made so far. This is needed because the first won't need to navigate

	UnityEngine.AI.NavMeshAgent navAgent;		//A reference to the tank's navmesh agent component
	bool canSeek;				//Can this tank navigate around?

	void Start()
	{
		//If this isn't the very first tank, then Initialize it (the first tank is just a placehold and thus doesn't
		//need to be initialized as a real tank
		if (++tankCount > 1)
			StartCoroutine (Initialize ());
	}

	IEnumerator Initialize()
	{
		//Wait a frame. If we don't, adding a navmesh agent will throw an error
		yield return null;

		//Add a navmesh agent component
		AddNavMeshAgent ();

		//This tank can now seek a target
		canSeek = true;
	}

	void Update()
	{
		//If the tank can't seek, leave
		if (!canSeek)
			return;

		//If the tank doesn't have a path OR it has reached its destination, seek a new destination
		if(!navAgent.hasPath || navAgent.remainingDistance <= navAgent.stoppingDistance)
			SeekTarget ();
	}

	void AddNavMeshAgent()
	{
		//Add a navmesh component and fill out its properties
		navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent> ();
		navAgent.radius = radius;
		navAgent.speed = speed;
		navAgent.stoppingDistance = stoppingDistance;
	}

	void SeekTarget()
	{
		//Find a new random point within our bounds
		float x = Random.Range (-xBound, xBound);
		float z = Random.Range (-zBound, zBound);

		//Set that point as the destination of our tank
		navAgent.SetDestination (new Vector3 (x, transform.position.y, z));
	}
}
