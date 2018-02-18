using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aplicarDanioJugador : MonoBehaviour {

	public int damage = 60;

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "Player") {
			GlobalObject.setDamagePlayer (damage);

		}
	}
}
