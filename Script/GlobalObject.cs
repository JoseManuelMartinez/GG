using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalObject : MonoBehaviour {

	private static GameObject[] prefabsTrees;
	public GameObject[] trees;

	private static Terrain Mapa;
	public GameObject Map;

	private static GameObject GlobalPlayer;
	public GameObject Player;

	void Awake(){
		//Mapa = Map.GetComponent<Terrain> ();
		GlobalPlayer = Player;
		prefabsTrees = trees;
	}

	public static void SetMap(Terrain m){Mapa = m; }
	public static Terrain GetMap(){return Mapa; }

	public static void SetPlayer(GameObject p){GlobalPlayer = p; }
	public static GameObject GetPlayer(){return GlobalPlayer; }	

	public static GameObject GetTree(int index){return prefabsTrees[index]; }	
	public static int sizeTree(){return prefabsTrees.Length; }	
}
