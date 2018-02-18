using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAenemigo : MonoBehaviour {

	public GameObject Player;
	public int life = 100;
	private Animation animator;
	public GameObject IconoEnemigo;

	void Start(){

		animator = GetComponent<Animation> ();
		Player = GlobalObject.GetPlayer ();

		if (!GlobalObject.IconoEnemigos) {
			IconoEnemigo.SetActive (false);
		}

		animator.Play ("idle");
	}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "shot") {
			life -= c.gameObject.GetComponent<shot>().getDamage();
			if (life <= 0) {
				animator.Play ("die");
				Destroy (this.gameObject,1.0f);
			}
		}

		if (c.gameObject.tag == "Player") {
			StartCoroutine (atack());
		}
	}

	IEnumerator atack() {
		animator.Play ("hit");
		yield return new WaitForSeconds (1);
		animator.Play ("idle");
	}

}
