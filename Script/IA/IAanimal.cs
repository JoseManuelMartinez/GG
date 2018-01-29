using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IAanimal : MonoBehaviour {

	private Terrain map;
	private NavMeshAgent agent;

	// Use this for initialization
	void Start () {

		agent = GetComponent<NavMeshAgent> ();
		map = GlobalObject.GetMap ();
		StartCoroutine (walkingAnimal());
	}
	
	IEnumerator walkingAnimal() {

		while (true) {
			Vector3 destination = new Vector3 (Random.Range(0 , map.terrainData.heightmapWidth), 0 ,Random.Range(0, map.terrainData.heightmapHeight));
			agent.SetDestination (destination);
			yield return new WaitForSeconds (5);
		}

	}
}
