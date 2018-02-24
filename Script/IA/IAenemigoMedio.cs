using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAenemigoMedio : MonoBehaviour {

	public GameObject Player;
	public int life = 100;
	public float speed = 3;
	private NavMeshAgent agent;
	private Animator animator;
	public GameObject IconoEnemigo;
	public bool stopDead = true;
	private Terrain map;

	public float distance = 30;

	void Start(){

		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		Player = GlobalObject.GetPlayer ();
		map = GlobalObject.GetMap ();

		if (!GlobalObject.IconoEnemigos) {
			IconoEnemigo.SetActive (false);
		}

	}
	// Update is called once per frame
	void Update () {
		

		if (life <= 0) {//si muere

			//animator.StopRecording ();
			animator.Play ("Death");

			Destroy (this.gameObject,2.0f);

			if(stopDead){
				agent.SetDestination (this.gameObject.transform.position); //Para que no siga avanzando mientras muere
			}

		} else if(Vector3.Distance (Player.transform.position, transform.position) < distance){//Si ve al jugador
			
			Vector3 distancia = Player.transform.position - transform.position;
			//agent.Warp(this.gameObject.transform.position);
			Vector3 destination = new Vector3 (Player.transform.position.x, map.terrainData.GetHeight((int)Player.transform.position.x,(int)Player.transform.position.z), Player.transform.position.z);
			agent.SetDestination (destination);
			agent.speed = speed;
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

	public void quitarIcono(){IconoEnemigo.SetActive(false);}
	public void getLife(int l){life = l;}
	public void getSpeed(int s){speed = s;}

	void OnTriggerEnter(Collider c){
		if (c.gameObject.tag == "shot") {
			life -= c.gameObject.GetComponent<shot>().getDamage();
		}
	}
}
