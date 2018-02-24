using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAdragon : MonoBehaviour {

	private NavMeshAgent agent;
	private Animator animator;
	private Terrain map;
	private GameObject mapGO;

	void Start(){

		agent = GetComponent<NavMeshAgent> ();
		animator = GetComponent<Animator> ();
		map = GlobalObject.GetMap ();
		mapGO = GlobalObject.GetGameObjectMap ();
	}

	void OnTriggerEnter(Collider c){

		if (c.gameObject.tag == "Player") {

			int x = (int)Random.Range (0, map.terrainData.heightmapWidth);
			int z = (int)Random.Range (0, map.terrainData.heightmapHeight);
			//Vector3 destination = new Vector3 (x, map.terrainData.GetHeight(x,z), z);
			Vector3 destination = mapGO.gameObject.GetComponent<MountainGenerator>().GetPositionGreenCubeHeightZero();
			destination = new Vector3 (destination.x, map.terrainData.GetHeight((int)destination.x,(int)destination.z), destination.z);
			agent.Warp (this.gameObject.transform.position);
			agent.SetDestination (destination);	

			animator.SetBool ("fly", true);
			Destroy (this.gameObject,30.0f);

			//Debug.Log(map.terrainData.heightmapWidth +" - "+ map.terrainData.heightmapHeight);
		}
	}
}
