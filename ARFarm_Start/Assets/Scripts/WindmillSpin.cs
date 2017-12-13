using UnityEngine;

[AddComponentMenu("Shepherd/Windmill Spin")]
public class WindmillSpin : MonoBehaviour 
{
	public float rotationsPerSecond = .25f;

	private Transform toSpin;
	private float currentRotation = 0;

	// Use this for initialization
	void Start () 
	{
		toSpin = transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
		currentRotation = ((currentRotation + (rotationsPerSecond * Time.deltaTime)*360f) + 360f) % 360f;
		toSpin.localRotation = Quaternion.Euler(0, 0, currentRotation);
	}
}
