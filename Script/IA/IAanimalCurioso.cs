using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAanimalCurioso : MonoBehaviour {

	private GameObject map;
	private NavMeshAgent agent;
	private GameObject Player;
	private Animator animator;
	private Vector3 destination;

	public float distance = 2.0f;
	public float velocidad = 2.0f;

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		agent.Warp(this.gameObject.transform.position);
		map = GlobalObject.GetGameObjectMap ();
		Player = GlobalObject.GetPlayer ();
		animator = GetComponent<Animator> ();
		destination = map.GetComponent<MountainGenerator> ().GetPositionGreenCube ();
	}
	
	void Update() {

		if(Vector3.Distance (Player.transform.position, transform.position) < distance){//Si el jugador esta cerca le mira
			Debug.Log("Mira");
			animator.SetBool ("Idle", true);
			animator.SetBool ("Walk", false);
			agent.isStopped = true;
			Vector3 direccion = Player.transform.position - transform.position;
			Quaternion rotacion = Quaternion.LookRotation (direccion);
			transform.rotation =  Quaternion.Lerp (transform.rotation,rotacion, velocidad * Time.deltaTime);
		}else {//Sino anda
			agent.isStopped = false;
			if (transform.position == destination) {
				destination = map.GetComponent<MountainGenerator> ().GetPositionGreenCube ();
				Debug.Log (destination);
			}
			animator.SetBool ("Idle", false);
			animator.SetBool ("Walk", true);
			agent.SetDestination (destination);
		}

	}
}
