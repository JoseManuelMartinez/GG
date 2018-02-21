using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsWhale : MonoBehaviour {

	private AudioSource source;
	public AudioClip[] ScacySound;
	public float volumenScacySound = 0.2f;

	// Use this for initialization
	void Start () {

		source = GetComponent<AudioSource> ();
		StartCoroutine (SoundRutine ());

	}
	
	IEnumerator SoundRutine(){

		while (true) {
			float random = Random.Range (10.0f, 50.0f);
			int index = (int)Random.Range (0.0f, ScacySound.Length - 1);
			yield return new WaitForSeconds (random);
			source.PlayOneShot (ScacySound [index], volumenScacySound);

		}
	}
}
