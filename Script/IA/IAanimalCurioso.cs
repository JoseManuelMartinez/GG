using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAanimalCurioso : MonoBehaviour {

	private Terrain map;
	private NavMeshAgent agent;
	private GameObject Player;
	private Animator animator;
	private Vector3 destination;
	private bool flat = true;
	private bool flatSound = true;
	private AudioSource source;

	public float distance = 2.0f;
	public float velocidad = 2.0f;
	public AudioClip animalSound;
	public float volumenAnimalSound = 0.5f;


	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		agent.Warp(this.gameObject.transform.position);
		map = GlobalObject.GetMap ();
		Player = GlobalObject.GetPlayer ();
		animator = GetComponent<Animator> ();
		source = GetComponent<AudioSource> ();
		//int x = (int)Random.Range (0, map.terrainData.heightmapWidth);
		//int z = (int)Random.Range (0, map.terrainData.heightmapHeight);
		//destination = new Vector3 (x, map.terrainData.GetHeight(x,z), z);
		//StartCoroutine (walkingAnimal());
	}
	
	void Update() {

		if(Vector3.Distance (Player.transform.position, transform.position) < distance){//Si el jugador esta cerca le mira
			Debug.Log("Mira");
			animator.SetBool ("Idle", true);
			animator.SetBool ("Walk", false);
			agent.SetDestination (this.transform.position);	
			//agent.isStopped = true;
			Vector3 direccion = Player.transform.position - transform.position;
			Quaternion rotacion = Quaternion.LookRotation (direccion);
			transform.rotation =  Quaternion.Lerp (transform.rotation,rotacion, velocidad * Time.deltaTime);
		}else {//Sino anda
			StartCoroutine (walkingAnimal());
		}

	}

	IEnumerator walkingAnimal() {

		if (flat) {
			flat = false;
			int x = (int)Random.Range (0, map.terrainData.heightmapWidth);
			int z = (int)Random.Range (0, map.terrainData.heightmapHeight);
			Vector3 destination = new Vector3 (x, map.terrainData.GetHeight (x, z), z);
			animator.SetBool ("Idle", false);
			animator.SetBool ("Walk", true);
			agent.Warp (this.gameObject.transform.position);
			agent.SetDestination (destination);	
			//agent.isStopped = false;
			yield return new WaitForSeconds (3);
			flat = true;
		}

	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "dedo") {
			if (flatSound) {
				flatSound = false;
				StartCoroutine (startAnimalSound ());
			}
		}
	}

	IEnumerator startAnimalSound(){
		source.PlayOneShot (animalSound, volumenAnimalSound);
		yield return new WaitForSeconds (1);
		source.Stop ();
		yield return new WaitForSeconds (2);
		flatSound = true;
	}
}
