using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAanimal : MonoBehaviour {

	private Terrain map;
	private GameObject mapa;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		agent.Warp(this.gameObject.transform.position);
		map = GlobalObject.GetMap ();
		mapa = GlobalObject.GetGameObjectMap ();
		StartCoroutine (walkingAnimal());
	}
	
	IEnumerator walkingAnimal() {

		while (true) {
			//Vector3 destination = mapa.GetComponent<MountainGenerator> ().GetPositionGreenCube ();
			int x = (int)Random.Range (0, map.terrainData.heightmapWidth);
			int z = (int)Random.Range (0, map.terrainData.heightmapHeight);
			Vector3 destination = new Vector3 (x, map.terrainData.GetHeight(x,z), z);
			agent.Warp (this.gameObject.transform.position);
			agent.SetDestination (destination);			
			yield return new WaitForSeconds (10);
		}

	}
}
