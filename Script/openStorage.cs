using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openStorage : MonoBehaviour {

	public GameObject tapa;
	public GameObject Icono;
	private Animator animatorTapa;

	void Start(){
		animatorTapa = tapa.GetComponent<Animator> ();
		GlobalObject.CofrePuesto ();
	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "dedo") {
			animatorTapa.SetBool ("Open", true);
			Icono.SetActive (false);
			GlobalObject.cofreEncontrado ();
			Destroy (this.gameObject);
		}
	}
}
