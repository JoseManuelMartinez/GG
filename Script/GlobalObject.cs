using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalObject : MonoBehaviour {

	//Collección de arboles
	private static GameObject[] prefabsTrees;
	public GameObject[] trees;

	//Collección de elementos de suspense
	private static GameObject[] prefabsElementosSuspense;
	public GameObject[] elementosSuspense;

	//Collección de elementos de valencia
	private static GameObject[] prefabsElementosEnemigos;
	public GameObject[] elementosEnemigos;

	private static GameObject[] prefabsElementosEnemigosGrandes;
	public GameObject[] elementosEnemigosGrandes;

	private static GameObject[] prefabsElementosAnimales;
	public GameObject[] elementosAnimales;

	//Referencia al terreno del mapa
	private static Terrain Mapa;
	public static GameObject Map;

	//Referencia al nav mesh
	private static GameObject GlobalNavMesh;
	public GameObject NavMesh;

	//Referencia al minimapa
	private static GameObject GlobalMinimapa;
	public GameObject MiniMapa;
	public static bool IconoEnemigos = true;

	//Referencia a jugador
	private static GameObject GlobalPlayer;
	public GameObject Player;

	//Referencia a camaraDelJugador
	private static GameObject GlobalCamara;
	public GameObject Camara;

	//Referencia a luz auxiliar
	private static GameObject GlobalLuzAuxiliar;
	public GameObject LuzAuxiliar;

	//Vida jugador
	private static int GlobalLifePlayer;
	public static bool inmortal = false;
	public int lifePlayer = 100;
	public Text lifeText; 
	public static Text GlobalLifeText; 

	//cantidad de cofres
	private static int cofresTotales = 0;
	private static int cofresEncontrados = 0;
	public Text cofreText; 
	public static Text GlobalcofreText; 

	void Awake(){
		//Mapa = Map.GetComponent<Terrain> ();
		GlobalPlayer = Player;
		prefabsTrees = trees;
		prefabsElementosSuspense = elementosSuspense;
		prefabsElementosAnimales = elementosAnimales;
		prefabsElementosEnemigos = elementosEnemigos;
		prefabsElementosEnemigosGrandes = elementosEnemigosGrandes;

		GlobalNavMesh = NavMesh;
		GlobalCamara = Camara;
		GlobalMinimapa = MiniMapa;
		GlobalLifePlayer = lifePlayer;
		GlobalLifeText = lifeText;
		GlobalcofreText = cofreText;
		GlobalLuzAuxiliar = LuzAuxiliar;
		lifeText.text = GlobalLifePlayer.ToString(); 
		cofresTotales = 0;
		cofresEncontrados = 0;
	}

	public static GameObject GetNavMesh(){return GlobalNavMesh; }	

	public static void SetMap(Terrain m){Mapa = m; }
	public static void SetGameObjectMap(GameObject m){Map = m; }
	public static Terrain GetMap(){return Mapa; }
	public static GameObject GetGameObjectMap(){return Map; }

	public static void SetPlayer(GameObject p){GlobalPlayer = p; }
	public static GameObject GetPlayer(){return GlobalPlayer; }	

	public static GameObject GetTree(int index){return prefabsTrees[index]; }	
	public static int sizeTree(){return prefabsTrees.Length; }	

	public static GameObject GetAnimal(int index){return prefabsElementosAnimales[index]; }	
	public static int sizeAnimal(){return prefabsElementosAnimales.Length; }	

	public static GameObject GetEnemigos(int index){return prefabsElementosEnemigos[index]; }	
	public static int sizeEnemigos(){return prefabsElementosEnemigos.Length; }

	public static GameObject GetEnemigosGrandes(int index){return prefabsElementosEnemigosGrandes[index]; }	
	public static int sizeEnemigosGrandes(){return prefabsElementosEnemigosGrandes.Length; }

	public static GameObject GetSuspense(int index){return prefabsElementosSuspense[index]; }	
	public static int sizeSuspense(){return prefabsElementosSuspense.Length; }	

	public static GameObject getCanvasMiniMap(){return GlobalMinimapa;}

	public static void encenderLuzAuxiliar(){GlobalLuzAuxiliar.SetActive (true);}

	public static void fondoNegroCamara(){
		GlobalCamara.GetComponent<Camera> ().clearFlags = CameraClearFlags.SolidColor;
	}

	public static void setLivePlayer(int l){GlobalLifePlayer = l; GlobalLifeText.text = GlobalLifePlayer.ToString() + " Vida";}
	public static void setDamagePlayer(int damage){
		if (!inmortal) {
			GlobalLifePlayer -= damage;

			GlobalLifeText.text = "0 Vida";
			if (GlobalLifePlayer <= 0) {
				endGame ();
			}

			GlobalLifeText.text = GlobalLifePlayer.ToString () + " Vida";

		}
	}

	//public static void setCantidadCofres(int cantidad){cofresTotales = cantidad;}
	public static void CofrePuesto(){
		++cofresTotales;
		GlobalcofreText.text = cofresEncontrados + "/" + cofresTotales + " Cofres";
	}

	public static void cofreEncontrado(){

		++cofresEncontrados;
		GlobalcofreText.text = cofresEncontrados + "/" + cofresTotales + " Cofres";
		if (cofresEncontrados == cofresTotales) {
			endGame ();
		}
	}

	private static void endGame(){
		PlayerPrefs.SetInt ("CofresTotales", cofresTotales);
		PlayerPrefs.SetInt ("CofresEncontrados", cofresEncontrados);
		//ir a creditos
		SceneManager.LoadScene ("Resultado");
	}
}
