using UnityEngine;
using System.Collections;

[AddComponentMenu("Shepherd/Mouse Cursor")]
public class MouseCursor : MonoBehaviour 
{
	private Camera cameraSource;
	public Transform reticle;

	void Start () 
	{
		// This is on a camera
		cameraSource = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Convert the mouse position to a ray and raycast to the terrain
		var mouseRay = cameraSource.ScreenPointToRay(Input.mousePosition);
		var hitInfo = new RaycastHit();
		var terrainOnlyMask = 1 << 8;
		if (Physics.Raycast(mouseRay, out hitInfo, 1000, terrainOnlyMask))
		{
			// If we are on the navmesh, we update our position
			var navHitInfo = new UnityEngine.AI.NavMeshHit();
			if (UnityEngine.AI.NavMesh.SamplePosition(hitInfo.point, out navHitInfo, 5, UnityEngine.AI.NavMesh.AllAreas))
			{
				reticle.position = navHitInfo.position;
			}
		}
	}
}
