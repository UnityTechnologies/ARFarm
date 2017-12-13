using UnityEngine;
using System.Linq;

//[AddComponentMenu("Shepherd/Sheep")]
public class Sheep : MonoBehaviour 
{
	public Transform followTarget;
	//public Transform fleeTarget;
	//public Transform runMagnet;

	// Fleeing behavior
	public float scareDistance = 5;
	public float scareDuration = 2f;
	public float scareFallOff = 1f;
	public float runSpeed = 10;

	// Wandering behavior
	public float wanderInterval = 2f;
	public float wanderSpeed = 2f;
	[Range(0f,360f)]
	public float wanderMaxAngle = 180f;

	// Leader-following behavior
	public float maxLeaderDistance = 15f;

	private float scareTimer = 0f;
	private float wanderTimer = 0f;
	private bool fleeing = true;

	// Sheer-o-matic behavior
	public bool runFromMachine = false;
	public bool alphaSheep = false;

	// Movement Hint behavior
	private Transform forcedMovementTarget = null;
	private float forcedMovementTime = 0f;
	private UnityEngine.AI.NavMeshAgent agent;
//	private Dog sheepDog;

	// Alert behavior
	private Animator animator;

	private RandomAudioPlayer sounds;

	void Start()
	{
		//agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		//if (agent == null)
		//{
		//	Debug.LogError("No nav mesh agent found on sheep, aborting script!");
		//	enabled = false;
		//}
		//sheepDog = fleeTarget.GetComponent<Dog>();

		animator = GetComponent<Animator>();
		sounds = GetComponent<RandomAudioPlayer>();
		/*
		if (fleeTarget == null)
		{
			var possibleDog = FindObjectOfType<Dog>();
			if (possibleDog != null)
			{
				fleeTarget = possibleDog.transform;
			}
		}
		if (followTarget == null && alphaSheep == false)
		{
			var newAlpha = FindObjectsOfType<Sheep>().Where(otherSheep => otherSheep.alphaSheep == true).FirstOrDefault();
			if (newAlpha != null)
			{
				followTarget = newAlpha.transform;
			}
		}*/
	}

	// Update is called once per frame
	void Update () 
	{
		// Determine if we are being forced to a certain position (for crossing the bridge)
		if (forcedMovementTarget != null)
		{
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			//agent.speed = runSpeed;
            //agent.destination = forcedMovementTarget.position;
			forcedMovementTime -= Time.deltaTime;
			if (forcedMovementTime < 0 || (transform.position - agent.destination).sqrMagnitude < 1f)
			{
				forcedMovementTarget = null;
				scareTimer = scareDuration;
				fleeing = true;
			}
			return;
		}
		// Get the vector to our target
		//var toTarget = fleeTarget.position - transform.position;
		var toFriend = Vector3.zero;

		if (followTarget != null)
		{
			toFriend = followTarget.position - transform.position;
		}

		// If the object we flee from is too close, get scared!
		//if ((toTarget.sqrMagnitude <= (scareDistance * scareDistance)) || sheepDog.isBarking)
		//{
			//scareTimer = scareDuration;
			//wanderTimer = 0f;
			//fleeing = true;
		//}
		//else
		//{
			if ((toFriend.sqrMagnitude > (maxLeaderDistance * maxLeaderDistance)) && fleeing == false)
			{
				scareTimer = scareDuration;
				wanderTimer = 0f;
			}
		//}
		
		// If we're scared, run from the target
		if (scareTimer > 0)
		{
			// We store our expected motion vector ahead of time
			var scaredMotion = Vector3.zero;

			//if (fleeing)
			//{
				//scaredMotion = toTarget.normalized * -1f;
			//}
			//else
			//{
				scaredMotion = toFriend;
			//}

			// We also run slightly towards the run magnet, to prevent the object from getting stuck along walls
			//var towardsMagnet = (transform.position).normalized;
			//agent.destination = transform.position + (scaredMotion);

			// Determine if our speed is slowing down at all
			var speedDampening = 1f - Mathf.Clamp01((scareFallOff - scareTimer) / scareFallOff);

			//agent.speed = runSpeed * speedDampening;
		}
		else
		{
			fleeing = false;
			// See if it is time to stop or change direction
			if (wanderTimer <= 0f)
			{
				wanderTimer = wanderInterval;
				// Stop half the time
				if (Random.Range(0f,1f) > .5f)
				{
					//agent.speed = 0;
					//agent.destination = transform.position;
				}
				else
				{
					// Wander randomly half the time
					//agent.speed = wanderSpeed;

					var randomAngle = Mathf.Atan2(transform.forward.z, transform.forward.x) + Random.Range(-.5f, .5f) * wanderMaxAngle * Mathf.Deg2Rad;
					randomAngle = (randomAngle + 2 * Mathf.PI) % (2 * Mathf.PI);
					var randomDir = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
					//agent.destination = transform.position + randomDir*10;
				}
			}
			wanderTimer -= Time.deltaTime;
		}

		// Sanity check for barely moving sheep
		//if (agent.speed < .1f)
		//{
			//agent.destination = transform.position;
		//}
		//scareTimer -= Time.deltaTime;

		//if (sheepDog.isBarking)
		//{
			//if (sounds)
			//{
				//sounds.PlayRandomSound();
			//}
			//animator.SetTrigger("Scared");
		//}
	}

	public void SetForcedMovementTarget(Transform newTarget, float forcedTime = 3f)
	{
		if (forcedMovementTarget != null)
		{
			return;
		}
		forcedMovementTarget = newTarget;
		forcedMovementTime = forcedTime;
	}
}
