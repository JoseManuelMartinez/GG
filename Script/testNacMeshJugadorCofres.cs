using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class testNacMeshJugadorCofres : MonoBehaviour {

	private NavMeshAgent agent;

	// Use this for initialization
	void Awake () {

		agent = GetComponent<NavMeshAgent> ();

	}
	
	public void getPosition(Vector3 position, int num){

		agent.SetDestination (position);
		StartCoroutine (testCaminoCofre(num));

	}

	IEnumerator testCaminoCofre(int num) {

		yield return new WaitForSeconds (2);

		if (agent.hasPath) {
			Debug.Log ("El jugador puede llegar al cofre numero " + num.ToString () + "? : PASS");
		} else {
			Debug.Log ("El jugador puede llegar al cofre numero " + num.ToString () + "? : FAIL");
		}
	}
}
