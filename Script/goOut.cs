using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goOut : MonoBehaviour {

	public Vector3 newPosition;

	// Use this for initialization
	public void setNewPosition (Vector3 pos) {newPosition = pos;	}
	
	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "Player") {
			StartCoroutine (goOutPlace());
		}
	}

	IEnumerator goOutPlace() {

		yield return new WaitForSeconds (5);
		this.transform.position = newPosition;

	}
}
