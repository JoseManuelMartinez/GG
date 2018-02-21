using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour {

	public float velocity = 2.0f;
	public int damage = 35;

	private AudioSource source;
	public AudioClip shotSound;
	public float volumenShotSound = 0.5f;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		StartCoroutine (shotSoundRutine ());
		Destroy (this.gameObject,10);
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Translate(Vector3.down * velocity * Time.deltaTime);
	}

	public void setDamage(int dam){damage = dam;}
	public int getDamage(){return damage ;}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Enemy"){
			Destroy (this.gameObject);
		}
	}

	IEnumerator shotSoundRutine(){
		source.PlayOneShot (shotSound, volumenShotSound);
		yield return new WaitForSeconds (0);
		//source.Stop ();
		//yield return new WaitForSeconds (2);

	}
}
