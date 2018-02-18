using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gun : MonoBehaviour {

	public GameObject shot;
	public int damage;
	public bool municiónIlimitada = false;
	private int municion = 66;
	public Text textoMunición;

	void Start(){
		if (!municiónIlimitada) {
			textoMunición.text = municion + " Municion";
		} else {
			textoMunición.text = "";
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {

			if (!municiónIlimitada) {
				if (municion != 0) {
					--municion;
					textoMunición.text = municion+" Municion";
					GameObject Shot = Instantiate (shot, this.transform.position, this.transform.rotation) as GameObject;
					Shot.gameObject.GetComponent<shot> ().setDamage (damage);
				}
			} else {
				GameObject Shot = Instantiate (shot, this.transform.position, this.transform.rotation) as GameObject;
				Shot.gameObject.GetComponent<shot> ().setDamage (damage);
			}
		}
	}

	public void setMunicion(int mun){municion += mun;}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "municion") {
			municion += 20;
			textoMunición.text = municion+" Municion";
			Destroy (c.gameObject);
		}
	}

}
