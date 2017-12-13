using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectionImage : MonoBehaviour, IPointerDownHandler //<--The event handler for clicking a UI element
{
	[SerializeField] GameObject selectablePrefab;		//The prefab this image represents
	[SerializeField] PlacementManager placementManager;	//A reference to the placement manager

	GameObject placeholder;		//A local copy of this image's reference prefab

	//Reset() is called in the editor whenever we add a script to an object or choose "Reset" from the component menu.
	//We use this method to fill in script properties as we can perform heavy searching methods here without it
	//affecting performance at runtime
	void Reset()
	{
		//Find a reference to the placement manager
		placementManager = GameObject.FindObjectOfType<PlacementManager> ();
	}

	void Awake()
	{
		//Instantiate a copy of the reference prefab and then nest it under the placement manager and hide it
		placeholder = (GameObject)Instantiate (selectablePrefab);
		placeholder.transform.parent = placementManager.transform;
		placeholder.SetActive (false);
	}

	//This method comes from the event system when we click this object
	public void OnPointerDown (PointerEventData data) 
	{
		//Enable our placehold object and give it to the placementManager script
		placeholder.SetActive (true);
		placementManager.AttachNewObject(placeholder);
	}
}
