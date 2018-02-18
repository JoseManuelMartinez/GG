using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniMapa : MonoBehaviour {

	public GameObject Player;
	private Vector3 position;


	
	// Update is called once per frame
	void Update () {

		position = new Vector3 (Player.transform.position.x, Player.transform.position.y + 80, Player.transform.position.z);
		this.transform.position = position;

	}

	public void setPlater(GameObject player){Player = player;}
}
