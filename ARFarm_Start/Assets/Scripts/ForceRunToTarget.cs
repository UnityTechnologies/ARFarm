using UnityEngine;
using System.Collections;

[AddComponentMenu("Shepherd/Force Run To Target")]
public class ForceRunToTarget : MonoBehaviour 
{
	public Transform runTarget;

	void OnTriggerEnter(Collider triggeree)
	{
		// Check if the object in position switcher is a sheep
		var toSwitch = triggeree.GetComponent<Sheep>();
		if (toSwitch == null)
		{
			return;
		}
		toSwitch.SetForcedMovementTarget(runTarget);
	}
}
