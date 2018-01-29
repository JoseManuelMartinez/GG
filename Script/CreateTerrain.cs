using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour {

	public GameObject Map;

	// Use this for initialization
	void Start () {

		GameObject MapGenerate = Instantiate (Map, transform.position, transform.rotation) as GameObject;
		GlobalObject.SetMap (Map.GetComponent<Terrain> ());
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
