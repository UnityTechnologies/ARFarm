using UnityEngine;
using System.Collections.Generic;

//[AddComponentMenu("Shepherd/Random Audio Player")]
public class RandomAudioPlayer : MonoBehaviour 
{
	public List<AudioClip> soundList = new List<AudioClip>();
	public float randomDelay = 0f;

	private AudioSource soundPlayer = null;

	void Start () 
	{
		soundPlayer = GetComponent<AudioSource>();
	}
	
	public void PlayRandomSound()
	{
		var newClip = soundList[Random.Range(0, soundList.Count)];
		if (newClip == null)
		{
			return;
		}

		soundPlayer.clip = newClip;

		if (randomDelay > 0f)
		{
			soundPlayer.PlayDelayed(Random.Range(0, randomDelay));
		}
		else
		{
			soundPlayer.Play();
		}
	}
}
