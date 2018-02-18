using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treeDown : MonoBehaviour {

	private Animator animator;

	void Start(){
		animator = GetComponent<Animator> ();
	}


	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "Player") {
			animator.SetBool ("down", true);
		}
	}


}
