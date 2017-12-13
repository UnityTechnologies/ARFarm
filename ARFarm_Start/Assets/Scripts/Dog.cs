using UnityEngine;

[AddComponentMenu("Shepherd/Dog")]
public class Dog : MonoBehaviour 
{
	public Transform target;
	public bool follow = true;

	public float runSpeed = 10;
	public float walkSpeed = 5;
	public float walkDistance = 7;

	private bool barkRequest = false;
	private bool bark = false;
	private UnityEngine.AI.NavMeshAgent agent;
	private Animator animator;
	private RandomAudioPlayer sounds;

	void Start()
	{
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (agent == null)
		{
			Debug.LogError("No nav mesh agent found on dog, aborting script!");
			enabled = false;
		}
		animator = GetComponent<Animator>();
		if (animator == null)
		{
			Debug.LogError("No animator found on dog, aborting script!");
			enabled = false;
		}
		sounds = GetComponent<RandomAudioPlayer>();
		if (target == null)
		{
			target = GameObject.FindObjectOfType<ControlReticle>().transform;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		// Get the vector to our target
		var toTarget = target.position - transform.position;

		// Determine how close we are to the target
		var closeToTarget = (toTarget.sqrMagnitude <= (walkDistance * walkDistance));

		// Make	the cursor position our destination
		agent.destination = target.position;

		// Determine our run speed
		if (closeToTarget)
		{
			agent.speed = walkSpeed;
		}
		else
		{
			agent.speed = runSpeed;
		}

		// Update barking state
		bark = barkRequest || Input.GetMouseButtonDown(0);
		if (bark)
		{
			animator.SetTrigger("Bark");
			if (sounds)
			{
				sounds.PlayRandomSound();
			}
		}
		barkRequest = false;
		
	}

	public void Bark()
	{
		barkRequest = true;
	}

	public bool isBarking
	{
		get { return bark; }
	}
}
