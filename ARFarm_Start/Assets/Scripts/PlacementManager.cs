using UnityEngine;
using System.Collections;

public class PlacementManager : MonoBehaviour 
{
	[Header("Raycast Properties")]
	[SerializeField] Camera mainCamera;				//The camera that we will be raycasting from
	[SerializeField] float defaultDistance = 50f;	//How far in front of the camera should our objects float when we aren't clicking on the ground
	[SerializeField] LayerMask whatIsGround;		//LayerMask containing what layers we consider to be "the ground"

	[Header("Placement Properties")]
	[SerializeField] float upscaleFactor = 1.2f;	//How much we should scale up an object that we've place. This causes placed objects to visually "pop"
	[SerializeField] float arrowHeight = 20f;		//How high above our placement point the arrow prefab should sit
	[SerializeField] ParticleSystem arrowEffect;	//A reference to our arrow particle effect
	[SerializeField] Transform sandbox;				//A reference to our sandbox game object's transform

	GameObject objectToPlace;	//A reference to the game object we are trying to place
	Ray ray;					//The ray we will be using to raycast into the world
	RaycastHit rayHit;			//The RaycastHit that will contain information about what our raycast hits
	int touchID;				//ID of a screentouch that is dragging a placeable object
	bool isDragging;			//Are we currently dragging a placeable object?
	bool isTouchInput;			//Are we currently using touch input on the screen?
	bool isPlacementValid;		//Is the spot we are trying to place an object valid?

	//Reset() is called in the editor whenever we add a script to an object or choose "Reset" from the component menu.
	//We use this method to fill in script properties as we can perform heavy searching methods here without it
	//affecting performance at runtime
	void Reset()
	{
		//Find the main camera
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		//Find the arrow particle effect
		arrowEffect = GetComponentInChildren<ParticleSystem> ();
		//Find the sandbox game object
		sandbox = GameObject.Find ("Sandbox").transform;
	}

	void Update () 
	{
		//If we aren't trying to place an object, leave
		if (!objectToPlace)
			return;

		//Check to see if we have any input. If we do...
		if (CheckInput()) 
		{
			//...move the object we are dragging
			MovePlaceableObject ();
		} 
		//If we don't have any input, but we are currently dragging an object around...
		else if(isDragging) 
		{
			//... try to place that object in the world
			CheckForPlacement ();
		}
	}

	//This method checks for touch input on mobile devices or mouse input an non-mobile devices
	bool CheckInput()
	{
		//Handle touch events if we are on a mobile platform (but only for actual builds, not the editor)
		#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			//If we have at least one screen touch...
			if (Input.touches.Length > 0) 
			{
				//...and we aren't currently tracking a touch...
				if (!isTouchInput) 
				{
					//...we are now tracking a touch...
					isTouchInput = true;
					//...Record this touch as "our touch"...
					touchID = Input.touches [0].fingerId;
					//...return true (we do have input)
					return true;
				} 
				//...else, if the touch we are tracking has just ended...
				else if (Input.GetTouch (touchID).phase == TouchPhase.Ended) 
				{
					//...we are no longer tracking a touch...
					isTouchInput = false;
					//...return false (we do not have input)
					return false;
				} 
				//...otherwise...
				else 
				{
					//...return true (we do have input)
					return true;
				}
			} 
			//If we don't have any touches, return false (we don't have input)
			return false;

		//Handle mouse input for non-mobile or editor projects
		#else
			//Return whether or not the left mouse button is pressed
			return Input.GetMouseButton(0);
		#endif
	}

	//This method handles moving the object we are dragging around
	void MovePlaceableObject()
	{
		//We are currently dragging
		isDragging = true;

		Vector3 point;				//The point to move the object to
		Vector3 screenPosition;		//The position on the screen that represents our mouse or finger touch

		//If we are on a built mobile platform
		#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
			//Locate our touch and grab its screen position
			Touch touch = Input.GetTouch(touchID);
			screenPosition = new Vector3 (touch.position.x, touch.position.y, 0f);

		//If we are in the editor or on a non-mobile platform
		#else
			//Grab the mouse position
			screenPosition = Input.mousePosition;
		#endif

		//Create a ray from the camera, through our screen position
		ray = mainCamera.ScreenPointToRay (screenPosition);
		//If the ray hits anything on our "what is ground" layermask...
		if (Physics.Raycast (ray, out rayHit, 500f, whatIsGround)) 
		{
			//...record the point the ray hit and flag our placement as valid
			point = rayHit.point;
			isPlacementValid = true;
		} 
		else 
		{
			//...otherwise find the point along our ray that is our default distance away. Flag our placement as invalid
			point = ray.GetPoint (defaultDistance);
			isPlacementValid = false;
		}

		//Move our object to the desired point
		objectToPlace.transform.position = point;

		//Rotate the object we will place based on our camera's rotation
		Vector3 rotation = new Vector3 (0f, mainCamera.transform.eulerAngles.y, 0f);
		objectToPlace.transform.rotation = Quaternion.Euler (rotation);

		//Move our arrow particle system to be above our desired point
		arrowEffect.transform.position = point + Vector3.up * arrowHeight;
		//If the arrow particle effect isn't playing, play it
		if (!arrowEffect.isPlaying)
			arrowEffect.Play ();
	}

	//This method checks to see if we should try to place an object
	void CheckForPlacement()
	{
		//If this method was called, we are no longer dragging
		isDragging = false;
		//Stop and clear the arrow particle effect
		arrowEffect.Stop ();
		arrowEffect.Clear ();

		//If we are trying to place an object in a valid spot, go ahead and place it
		if (isPlacementValid)
			PlaceObject ();

		//We are no longer trying to place an object
		objectToPlace.SetActive (false);
		objectToPlace = null;
	}

	//This method actually places our object
	void PlaceObject()
	{
		//Instantiate a new object. 
		GameObject obj = Instantiate (objectToPlace) as GameObject;
		//Position, rotate, and scale thew object
		obj.transform.position = objectToPlace.transform.position;
		obj.transform.rotation = objectToPlace.transform.rotation;
		obj.transform.localScale *= upscaleFactor;
		//Nest the new object under our sandbox (so it will be properly tracked)
		obj.transform.parent = sandbox;
	}

	//This method is called from our selection image UI elements when we click them
	public void AttachNewObject(GameObject newObject)
	{
		//If we already have an object to place, disable it
		if (objectToPlace)
			objectToPlace.SetActive (false);

		//Get a reference to the new object we want to place
		objectToPlace = newObject;
	}
}
