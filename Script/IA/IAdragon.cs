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

			int x = (int)Random.Range (0, map.terrainData.heightmapWidth);
			int z = (int)Random.Range (0, map.terrainData.heightmapHeight);
			Vector3 destination = new Vector3 (x, map.terrainData.GetHeight(x,z), z);
			agent.Warp (this.gameObject.transform.position);
			agent.SetDestination (destination);	

			animator.SetBool ("fly", true);
			Destroy (this.gameObject,30.0f);

			//Debug.Log(map.terrainData.heightmapWidth +" - "+ map.terrainData.heightmapHeight);
		}
	}
}
