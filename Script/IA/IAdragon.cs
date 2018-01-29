using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAdragon : MonoBehaviour {

	private NavMeshAgent agent;
	private Animator animator;
	private Terrain map;

	void Start(){

		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		map = GlobalObject.GetMap ();
	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "Player") {
			animator.SetBool ("fly", true);
			Destroy (this.gameObject,30.0f);
			Vector3 destination = new Vector3 (Random.Range(0,map.terrainData.heightmapWidth), 0, Random.Range(0,map.terrainData.heightmapHeight));
			agent.SetDestination (destination);
			//Debug.Log(map.terrainData.heightmapWidth +" - "+ map.terrainData.heightmapHeight);
		}
	}
}
