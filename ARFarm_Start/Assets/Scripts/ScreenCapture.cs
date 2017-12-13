using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenCapture : MonoBehaviour 
{
	[SerializeField] Canvas UI;					//The UI to hide if we don't want it in our screenshot
	[SerializeField] Text infoText;				//The text element to display our save information
	[SerializeField] bool addTimeStamp;			//Do we want to add a millisecond timestamp to our images?
	[SerializeField] float displayTime = 5f;	//How long we should display the information text on the screen

	const string fileName = "Screenshot";		//Constant string for the screenshot's file name
	const string fileExt = ".png";				//Constant string for the screenshot's file extension

	WaitForSeconds delay;						//The variable that will store our delay

	void Awake()
	{
		//Set up our info text delay
		delay = new WaitForSeconds (displayTime);
	}

	//Public method called by our Screenshot button
	public void Capture(bool withUI)
	{
		//Start the coroutine which takes our screenshot
		StartCoroutine(ProcessImage(withUI));
	}

	//The coroutine that takes our screenshot
	IEnumerator ProcessImage(bool withUI)
	{
		//If we don't want the UI in the picture, hide it
		if (!withUI)
			UI.enabled = false;

		//Record our filename
		string path = fileName;
		//If we want a timestamp, add it to our path with the file extension. If we don't,
		//just add the file extension
		if (addTimeStamp)
			path += System.DateTime.Now.Millisecond + fileExt;
		else
			path += fileExt;

		//Now we can actually take the screenshot
		UnityEngine.ScreenCapture.CaptureScreenshot (path);

		//Taking the screenshot happens at the end of the frame, so wait a frame before doing anything else
		yield return null;

		//If we have infoText, add our message to it
		if(infoText != null)
			infoText.text = "Captured: " + Application.persistentDataPath + path;

		//If we hid the UI, show it again
		if(!withUI)
			UI.enabled = true;

		//Wait our designated amount of time
		yield return delay;

		//If we have infoText, remove our message (which effectively hides it)
		if(infoText != null)
			infoText.text = "";
	}
}
