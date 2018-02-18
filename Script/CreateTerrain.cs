using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTerrain : MonoBehaviour {

	public GameObject Map;
	public bool EjecutarTest ;
	public int numIntentos ;

	// Use this for initialization
	void Start () {

		EjecutarTest = false;
		numIntentos = 10;

		if (!EjecutarTest) {
			GameObject MapGenerate = Instantiate (Map, transform.position, transform.rotation) as GameObject;
			GlobalObject.SetMap (Map.GetComponent<Terrain> ());
			GlobalObject.SetGameObjectMap (Map);
		} else {
			Debug.Log ("EJECUTANDO TEST");
			StartCoroutine( ejecutaTestCreacionTerrenos ());
		}
	}
	
	IEnumerator ejecutaTestCreacionTerrenos() {

		int porcentajeTotalAcierto, intento, sumatorioIntentos = 0;

		for (int i = 0; i < numIntentos; ++i) {

			GameObject MapGenerate = Instantiate (Map, transform.position, transform.rotation) as GameObject;
			GlobalObject.SetMap (Map.GetComponent<Terrain> ());
			GlobalObject.SetGameObjectMap (Map);

			yield return new WaitForSeconds (1);

			intento = MapGenerate.GetComponent<TestMountainGenerator> ().Resultado;

			/*if(intento >= 60){
				Debug.Log ("Terreno jugable superior al 60% ? PASS");
			} else {
				Debug.Log ("Terreno jugable superior al 60% ? FAIL");
			}*/

			sumatorioIntentos += intento;
			Destroy (MapGenerate.gameObject);
		}

		porcentajeTotalAcierto = sumatorioIntentos / numIntentos;
		Debug.Log ("Media total de los porcentajes de acierto para "+ numIntentos +" intentos: " + porcentajeTotalAcierto +"%");
	}
}
