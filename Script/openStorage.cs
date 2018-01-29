using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openStorage : MonoBehaviour {

	private Animator animator;

	void Start(){
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "dedo") {
			animator.SetBool ("Open", true);
		}
	}
}
