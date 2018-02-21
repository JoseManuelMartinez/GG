using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class openStorage : MonoBehaviour {

	public GameObject tapa;
	public GameObject Icono;
	public GameObject Luz;
	private Animator animatorTapa;
	private bool flat = true;

	void Awake(){
		animatorTapa = tapa.GetComponent<Animator> ();
		GlobalObject.CofrePuesto ();
	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "dedo") {
			if (flat) {
				flat = false;
				Icono.SetActive (false);
				GlobalObject.cofreEncontrado ();
				GameObject light = Instantiate (Luz, tapa.transform.parent.position, tapa.transform.rotation) as GameObject;
				Destroy (light.gameObject,5.0f);
				animatorTapa.SetBool ("Open", true);
				Destroy (this.gameObject);
			}
		}
	}
}
