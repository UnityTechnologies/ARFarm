/*
using UnityEngine;
using Vuforia;		//Vuforia library

public class DisableObjectTrackerHandler : MonoBehaviour, ITrackableEventHandler // <--The event handler for finding and losing tracked targets
{
    public GameObject world;				//The game object that we will enable / disable when we find / lose our target

    TrackableBehaviour mTrackableBehaviour;	//A reference to the Tracking behavior

    void Awake()
    {
    	//Get a reference to the Trackable behavior and if it exists, register this script with it.
    	//We do this so that this script is notified when we find / lose a target
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

	//Event handler method needed to track the state of our targets
    public void OnTrackableStateChanged(
        TrackableBehaviour.Status previousStatus,
        TrackableBehaviour.Status newStatus)
    {
    	//Depending on our settings, we are looking to Detect, Track, or Extended Track
    	//a target. If this happens (our camera sees its target)...
        if (newStatus == TrackableBehaviour.Status.DETECTED ||
            newStatus == TrackableBehaviour.Status.TRACKED ||
            newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
        {
        	//...we found the target...
            TargetFound();
        }
        else
        {
        	//...otherwise, we lost the target
            TargetLost();
        }
    }

	//When we find the target...
    void TargetFound()
    {
    	//...enable the "world" object (which enables all its children)
        world.SetActive(true);
    }

	//When we lose the target...
    void TargetLost()
    {
    	//...disable the "world" object (which disables all its children)
        world.SetActive(false);
    }
}
*/