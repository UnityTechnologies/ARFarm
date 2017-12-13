using UnityEngine;

//[AddComponentMenu("Shepherd/Nav Agent Animator Control")]
public class NavAgentToAnimator : MonoBehaviour 
{
	//public UnityEngine.AI.NavMeshAgent agent;
	public Animator animator;

	private int paramSpeed;
	private int paramDirection;

	private Transform agentTransform;
	private Vector3 lastPosition;
//	private float lastForwardAngle = 0f;
	void Start()
	{
		// Precache dog animation values
		paramSpeed = Animator.StringToHash("Speed");
		paramDirection = Animator.StringToHash("Direction");
		agentTransform = this.transform;

		// Precache transform data
		lastPosition = agentTransform.position;
	}

	// Update is called once per frame
	void Update () 
	{
		// Calculate distance moved in the last frame and determine speed from that
		var frameSpeed = (agentTransform.position - lastPosition).magnitude / Time.deltaTime;

		// Calculate the difference between our facing direction and desired direction
		//var toDestination = agent.destination - agentTransform.position;
		var forwardAngle = Mathf.Atan2(agentTransform.forward.z, agentTransform.forward.x) / Mathf.PI;
		//var destinationAngle = Mathf.Atan2(toDestination.z, toDestination.x) / Mathf.PI;

		// Calculate the shortest angle difference
		forwardAngle = (forwardAngle + 2f) % 2f;
		//destinationAngle = (destinationAngle + 2f) % 2f;


		// At  short range, we use the difference in angular motion for rotation
		var angleDiff = 0f;
		/*
		if (toDestination.sqrMagnitude < 4f)
		{
			angleDiff = lastForwardAngle - forwardAngle;
			if (angleDiff > 1)
			{
				angleDiff = -(2 - angleDiff);
			}
			if (angleDiff < -1)
			{
				angleDiff = (2 + angleDiff);
			}
			angleDiff /= Time.deltaTime;
		}
		else
		{
			angleDiff = forwardAngle - destinationAngle;
			if (angleDiff > 1)
			{
				angleDiff = -(2 - angleDiff);
			}
			if (angleDiff < -1)
			{
				angleDiff = (2 + angleDiff);
			}
			angleDiff *= 10;
		}
*/
		// Give this info to the animator
		animator.SetFloat(paramSpeed, frameSpeed);
		animator.SetFloat(paramDirection, angleDiff, .125f, Time.deltaTime);

		lastPosition = agentTransform.position;
		//lastForwardAngle = forwardAngle;
	}
}
