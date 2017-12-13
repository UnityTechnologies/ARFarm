using UnityEngine;
using System.Collections;

[AddComponentMenu("Shepherd/NakedSheep")]
public class NakedSheep : MonoBehaviour 
{
	// Wandering behavior
	public float wanderInterval = 2f;
	public float wanderSpeed = 2f;
	[Range(0f,360f)]
	public float wanderMaxAngle = 180f;

	// Ejection Behavior
	public bool startEjected = false;
	public float ejectionInterval1 = 3f;
	public float ejectionInterval2 = 3f;
	public float ejectionSpeed = 6f;

	private float wanderTimer = 0f;
	private float ejectionTimer = 0f;
	private float currentEjectionInterval = 3f;
	
	private UnityEngine.AI.NavMeshAgent agent;	

	void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (agent == null)
		{
			Debug.LogError("No nav mesh agent found on sheep, aborting script!");
			enabled = false;
		}
		if (startEjected)
		{
			agent.velocity = transform.forward * ejectionSpeed;

			// Choose ejection animation
			var animator = GetComponent<Animator>();
			
			animator.SetBool("Cinematic", true);
			var ejectionType = Mathf.RoundToInt(Random.Range(0f, 1f));
			if (ejectionType == 0)
			{
				currentEjectionInterval = ejectionInterval1;
				animator.SetTrigger("Eject1");
			}
			else
			{
				currentEjectionInterval = ejectionInterval2;
				animator.SetTrigger("Eject2");
			}
			ejectionTimer = currentEjectionInterval;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Ejection behavior occurs immediately after being launched from the sheer-o-matic
		if (ejectionTimer > 0f)
		{
			ejectionTimer -= Time.deltaTime;
			var runSpeed = ejectionSpeed * Mathf.Clamp01(ejectionTimer / currentEjectionInterval);
			agent.speed = runSpeed;
			agent.destination = transform.position + transform.forward;
			return;
		}
		// See if it is time to stop or change direction
		if (wanderTimer <= 0f)
		{
			wanderTimer = wanderInterval;
			// Stop half the time
			if (Random.Range(0f,1f) > .5f)
			{
				agent.speed = 0;
				agent.destination = transform.position;
			}
			else
			{
				// Wander randomly half the time
				agent.speed = wanderSpeed;

				var randomAngle = Mathf.Atan2(transform.forward.z, transform.forward.x) + Random.Range(-.5f, .5f) * wanderMaxAngle * Mathf.Deg2Rad;
				randomAngle = (randomAngle + 2 * Mathf.PI) % (2 * Mathf.PI);
				var randomDir = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
				agent.destination = transform.position + randomDir*10;
			}
		}
		wanderTimer -= Time.deltaTime;
	}

	
}
