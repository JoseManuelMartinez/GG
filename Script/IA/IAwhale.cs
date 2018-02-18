using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAwhale : MonoBehaviour {

	public GameObject Player;
	private NavMeshAgent agent;
	private Animator animator;

	public float distance = 30;

	void Start(){

		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		Player = GlobalObject.GetPlayer ();
	}
	// Update is called once per frame
	void Update () {

		if(Vector3.Distance (Player.transform.position, transform.position) < distance){//Si ve al jugador

			Vector3 distancia = Player.transform.position - transform.position;

			agent.SetDestination (Player.transform.position);
			agent.speed = 3;
			animator.SetBool ("isIdle",false);

			if (distancia.magnitude <= agent.stoppingDistance ) { //Ataca
				animator.SetBool ("isWalking", false);
				animator.SetBool ("isAttacking", true);
			} else { //Corre
				animator.SetBool ("isWalking", true);
				animator.SetBool ("isAttacking", false);
			}
		} else {//Si esta lejos 
			agent.speed = 0;
			animator.SetBool ("isWalking",false);
			animator.SetBool ("isAttacking",false);
			animator.SetBool ("isIdle",true);
		}
	}
}
