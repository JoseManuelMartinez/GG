using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_EDITOR
//using UnityEditor.AI;
//#endif
using UnityEngine.AI;

public class MountainGenerator : MonoBehaviour {

	private TerrainData terrainData;
	private List<GameObject> listGreenBox; //Lista donde se almacenan las casillas validas
	private bool listGreenBoxCreated = false;
	private int SpaceBerweenCube = 10;
	private int sizeMap;
	private float valencia;
	private float excitacion;
	private float dominio;
	private float suspense;
	private int numCofres;

	private GameObject Player;
	public GameObject Map;
	public GameObject Whale;
	public GameObject Municion;
	public GameObject cofre;
	public GameObject testCofre;

	public int perlinPasses = 1;
	public int perlinScaleRangex = 2; 
	public int perlinScaleRangey = 2;
	public float limit = 0.25f;
	public float limitHeight = 25.0f;

	//Prefap de casilla valida o no valida
	public GameObject redCube;
	public GameObject greenCube;

	private GameObject masterCube; //Casilla central en el que se colocará al jugador

	// Use this for initialization
	void Start () {

		Player = new GameObject ();
		terrainData = Terrain.activeTerrain.terrainData;


		//Leemos tamaño del mapa que se eligió en el menu
		sizeMap = PlayerPrefs.GetInt("TamanioMapa");
		terrainData.heightmapResolution = sizeMap;
		//terrainData.
		Terrain terrainMap = this.GetComponent<Terrain>();
		Vector3 sizeVector = new Vector3 (sizeMap, 300, sizeMap);
		terrainMap.terrainData.size = sizeVector;
		GlobalObject.SetMap (terrainMap);

		Debug.Log ("TERRAIN DATA");
		Debug.Log (terrainMap.terrainData.alphamapHeight);
		Debug.Log (terrainMap.terrainData.alphamapWidth);
		Debug.Log (terrainMap.terrainData.heightmapHeight);
		Debug.Log (terrainMap.terrainData.heightmapWidth);

		GenerateHeightmap ();
		PaintMap ();

		this.gameObject.isStatic = true;

		this.gameObject.SetActive (false);
		this.gameObject.SetActive (true);

		scannerMap ();

		createDecoration ();

		putElement ();


		Player.SetActive (true);
	}

	void PerlinPass(float[,] heights, float perlinPasses) {
		var ox = Random.value * 10;
		var oy = Random.value * 10;

		var hw = heights.GetLength(0);
		var hh = heights.GetLength(1);
		var perlinScale = Random.Range(perlinScaleRangex, perlinScaleRangey);
		for (var x = 0; x < hw; x++) {
			for (var y = 0; y < hh; y++) {
				var px = ox + (float)x / hw * perlinScale;
				var py = oy + (float)y / hh * perlinScale;

				double e = Mathf.PerlinNoise(py, px); //+ 0.4 * Mathf.PerlinNoise(py * 2, px * 2) + 0.25 * Mathf.PerlinNoise(py * 4, px * 4);
				if (e <= limit)
					heights [x, y] = 0f;
				else
					heights[x,y] = Mathf.Pow((float)e, 5.00F);


			}
		}
	}

	void GenerateHeightmap() { //Este método genera el mapa llamando a la función de Perlin Pass
		//terrainData = Terrain.activeTerrain.terrainData;
		var hw = terrainData.heightmapWidth;
		var hh = terrainData.heightmapHeight;
		var heights = new float[hw,hh];

		for (var i = 0; i < perlinPasses; ++i)
			PerlinPass(heights, perlinPasses);

		terrainData.SetHeights(0, 0, heights);
	}

	void PaintMap() { //Este método se encarga de asignar las diferentes texturas al terreno dependiendo de su altura

		float max_altura = 0;

		for (int y = 0; y < terrainData.alphamapHeight; y++){
			for (int x = 0; x < terrainData.alphamapWidth; x++){
				float height = terrainData.GetHeight(y,x);
				if (height>max_altura) max_altura = height;
			}
		}

		float[, ,] pTexturas= new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

		float amplitud = max_altura/terrainData.alphamapLayers; 

		float altura = 0.0f;
		float peso = 0.0f;

		for (int y = 0; y < terrainData.alphamapHeight; y++) {
			for (int x = 0; x < terrainData.alphamapWidth; x++) {

				altura = terrainData.GetHeight(y,x);

				int textura_base = (int) Mathf.Floor(altura/amplitud);

				if (textura_base>terrainData.alphamapLayers-1) textura_base = terrainData.alphamapLayers-1;

				pTexturas[x,y,textura_base] = peso;
				if (textura_base<terrainData.alphamapLayers-1) pTexturas[x,y,textura_base+1] = (1.0f-peso)/2; // Comentar para evitar el mezclado
				if (textura_base>1) pTexturas[x,y,textura_base-1] = (1.0f-peso)/2; // comentar para evitar el mezclado

				if (altura > limit * 2)
					peso = Random.Range (0.3f, 0.4f);
				else
					peso = 0.5f;

				pTexturas[x,y,textura_base] = peso;
				if (textura_base<terrainData.alphamapLayers-1) pTexturas[x,y,textura_base+1] = (1.0f-peso)/2; // Comentar para evitar el mezclado
				if (textura_base>1) pTexturas[x,y,textura_base-1] = (1.0f-peso)/2; // comentar para evitar el mezclado
			}
		}

		terrainData.SetAlphamaps(0, 0, pTexturas);

	}

	public void scannerMap(){

		var terrainData = Terrain.activeTerrain.terrainData;
		listGreenBox = new List<GameObject> ();

		int contCubosVerdes = 0;
		int contCubosRojos = 0;
		Vector3 posicion = new Vector3 (0, 0, 0);

		GameObject boxParent = new GameObject();
		boxParent.name = "BoxParent";

		for (int x = SpaceBerweenCube; x < terrainData.heightmapHeight - SpaceBerweenCube; x+= SpaceBerweenCube) {
			for (int y = SpaceBerweenCube; y < terrainData.heightmapWidth - SpaceBerweenCube; y+=SpaceBerweenCube) {

				posicion = new Vector3 (x, terrainData.GetHeight (x, y), y);

				if (terrainData.GetHeight (x, y) > limitHeight) {
					GameObject box = Instantiate (redCube, posicion, transform.rotation) as GameObject;
					box.transform.parent = boxParent.transform;
					box.GetComponent<GreenCube> ().setName (x,y);
					box.GetComponent<GreenCube> ().setMap (terrainData);
					box.GetComponent<GreenCube> ().valid = false;
					contCubosRojos++;
				} else {
					GameObject box = Instantiate (greenCube, posicion, transform.rotation) as GameObject;
					box.transform.parent = boxParent.transform;
					box.GetComponent<GreenCube> ().setName (x,y);
					box.GetComponent<GreenCube> ().setMap (terrainData);
					listGreenBox.Add(box);
					contCubosVerdes++;
				}
			}
		}

		listGreenBoxCreated = true;

		int total = contCubosRojos + contCubosVerdes;
		int porcientoAcierto = contCubosVerdes * 100 / total;

		Debug.Log ("Casillas totales: " + total + ". Casillas validas: " + contCubosVerdes + ". Casillas no validas: " + contCubosRojos);
		Debug.Log ("El porcentaje de terreno jugable es del "+porcientoAcierto+" %");

		if (porcientoAcierto >= 60) {
			Debug.Log ("Terreno jugable superior al 60% ? PASS");
		} else {
			Debug.Log ("Terreno jugable superior al 60% ? FAIL");
		}
			

		//Calculamos las casillas validas que hay alrededor de cada casilla valida
		foreach (GameObject cube in listGreenBox){

			cube.GetComponent<GreenCube> ().CalcDistance ();

			//Debug.Log(cube.GetComponent<GreenCube> ().ToString ());
		}

		//Buscamos la casilla maestra

		int sumValores = 0;
		int resValoresx = int.MaxValue;
		int resValoresy = int.MaxValue;
		int sum;
		int resx;
		int resy;

		foreach (GameObject cube in listGreenBox) {

			sum = cube.GetComponent<GreenCube> ().top + cube.GetComponent<GreenCube> ().down + cube.GetComponent<GreenCube> ().left + cube.GetComponent<GreenCube> ().right;
			resx = Mathf.Abs (cube.GetComponent<GreenCube> ().left - cube.GetComponent<GreenCube> ().right);
			resy = Mathf.Abs (cube.GetComponent<GreenCube> ().top - cube.GetComponent<GreenCube> ().down);

			/*if (sum > sumValores && resx < resValoresx && resy < resValoresy) {

				sumValores = sum;
				resValoresx = resx;
				resValoresy = resy;
				masterCube = cube;
			}*/

			if (sum > sumValores) {
				
				sumValores = sum;
				resValoresx = resx;
				resValoresy = resy;
				masterCube = cube;

			} else if (sum == sumValores) {
				if (resx < resValoresx && resy < resValoresy) {
					sumValores = sum;
					resValoresx = resx;
					resValoresy = resy;
					masterCube = cube;
				}
			}
		}

		//eliminamos la posición del usuario de la lista de casillas disponibles
		listGreenBox.Remove (masterCube);

		//Debug.Log("CASILLA MASTER:");
		//Debug.Log (masterCube.GetComponent<GreenCube> ().ToString ());

		Player = GlobalObject.GetPlayer ();



	}//scannerMap() 

	void createDecoration(){
		
		int numTree = 0;
		int cont = 0;
		int randomIndex = 0;

		switch (sizeMap) {
		case 257:
			numTree = 150;
			numCofres = 3;
			break;
		case 513:
			numTree = 300;
			numCofres = 6;
			break;
		case 1025:
			numTree = 750;
			numCofres = 10;
			break;
		}

		GameObject treeParent = new GameObject();
		treeParent.name = "Arboles";

		//Colocamos aleatoriamente arboles en el mapa
		for (int i = 0; i < numTree; ++i) {

			randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
			cont = 0;

			foreach (GameObject box in listGreenBox) {
				if (randomIndex == cont) {
					randomIndex = Random.Range (0, GlobalObject.sizeTree());
					//Bajamos la posicion del arbol para que en las cuestas la raiz atraviese el suelo y no quede levitando
					Vector3 newPosition = new Vector3 (box.transform.position.x, box.transform.position.y - 1.0f, box.transform.position.z);
					GameObject aux = Instantiate (GlobalObject.GetTree (randomIndex), newPosition, box.transform.rotation) as GameObject;
					aux.transform.parent = treeParent.transform;
					listGreenBox.Remove (box);
					break;
				} else {
					++cont;
				}
			}

		}

		//Creamos el NavMesh para que los NPCs se muevan por el mapa
		//NavMeshBuilder.BuildNavMesh();

		GameObject NavMesh = GlobalObject.GetNavMesh();
		NavMesh.GetComponent<NavMeshSurface> ().BuildNavMesh ();


	}//createDecoration()

	void putElement (){

		valencia = PlayerPrefs.GetFloat ("Valencia");
		excitacion = PlayerPrefs.GetFloat ("Excitacion");
		dominio = PlayerPrefs.GetFloat ("Dominio");

		suspense = PlayerPrefs.GetFloat ("Suspense");
		int randomIndex, cont, numSuspense, numMunicion = 0;

		Light luzGlobal = Player.GetComponent<lightPlayerController> ().getLuzGlobal();

		if (suspense >= 0.9) {
			Light linterna = Player.GetComponent<lightPlayerController> ().getLinterna ();
			linterna.gameObject.SetActive (true);
			GlobalObject.getCanvasMiniMap ().gameObject.SetActive (false);
			//luzGlobal.gameObject.transform.Rotate (230, 0, 0, Space.World);
			//luzGlobal.gameObject.GetComponent<Light> ().intensity = 0.1f;
			luzGlobal.gameObject.SetActive (false);

			GlobalObject.fondoNegroCamara ();
			numSuspense = 4;

			//Colocamos al depredador en una posicion aleatoria

			randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
			cont = 0;

			foreach (GameObject box in listGreenBox) {
				if (randomIndex == cont) {
					//Bajamos la posicion del arbol para que en las cuestas la raiz atraviese el suelo y no quede levitando
					//Vector3 newPosition = new Vector3 (box.transform.position.x, box.transform.position.y - 1.0f, box.transform.position.z);
					GameObject aux = Instantiate (Whale, box.gameObject.transform.position, Whale.transform.rotation) as GameObject;
					aux.GetComponent<IAwhale> ().Player = Player;
					listGreenBox.Remove (box);
					break;
				} else {
					++cont;
				}
			}

		} else {

			if (suspense >= 0.5) {
				Light linterna = Player.GetComponent<lightPlayerController> ().getLinterna ();
				linterna.gameObject.SetActive (true);
				luzGlobal.gameObject.transform.Rotate (135, 0, 0, Space.World);
				numSuspense = 3;
			} else if (suspense >= 0.25) {
				Light linterna = Player.GetComponent<lightPlayerController> ().getLinterna ();
				linterna.gameObject.SetActive (true);
				luzGlobal.gameObject.transform.Rotate (120, 0, 0, Space.World);
				numSuspense = 2;
			} else {
				luzGlobal.gameObject.transform.Rotate(40, 0, 0,Space.World);
				GlobalObject.encenderLuzAuxiliar ();
				numSuspense = 1;
			}

			//Colocamos elementos de suspense

			GameObject SuspenseElements = new GameObject();
			SuspenseElements.name = "ElementosDeSuspense";

			for (int i = 0; i < numSuspense; ++i) {

				randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
				cont = 0;

				foreach (GameObject box in listGreenBox) {
					if (randomIndex == cont) {
						randomIndex = Random.Range (0, GlobalObject.sizeSuspense ());
						//Bajamos la posicion del arbol para que en las cuestas la raiz atraviese el suelo y no quede levitando
						//Vector3 newPosition = new Vector3 (box.transform.position.x, box.transform.position.y - 1.0f, box.transform.position.z);
						GameObject aux = Instantiate (GlobalObject.GetSuspense (randomIndex), box.gameObject.transform.position, GlobalObject.GetSuspense (randomIndex).transform.rotation) as GameObject;
						aux.transform.parent = SuspenseElements.transform;
						listGreenBox.Remove (box);
						break;
					} else {
						++cont;
					}
				}

			}//for suspense

			//DOMINIO
			if (dominio >= 0.75) {

				GlobalObject.inmortal = true;
				GlobalObject.GlobalLifeText.text = "Inmortal";

			} else if (dominio >= 0.5) {

				GlobalObject.IconoEnemigos = false;
				GlobalObject.setLivePlayer (1000);

			} else if (dominio >= 0.25) {
				GlobalObject.setLivePlayer (500);
				GlobalObject.getCanvasMiniMap ().gameObject.SetActive (false);
			} else {
				GlobalObject.setLivePlayer (100);
				GlobalObject.getCanvasMiniMap ().gameObject.SetActive (false);
			}//dominio

			//EXCITACION
			if (excitacion >= 0.75) {

				Player.GetComponent<gunsController> ().activaArmaGrande ();
				GlobalObject.deleteHandL ();

			} else if (excitacion >= 0.5) {

				Player.GetComponent<gunsController> ().activaArmaMediana ();
				Player.GetComponent<gunsController> ().ponerMunicion(300);
				numMunicion = 30;
				GlobalObject.deleteHandL ();

			} else if (excitacion >= 0.25) {
				Player.GetComponent<gunsController> ().activaArmaPequenia ();
				Player.GetComponent<gunsController> ().ponerMunicion(100);
				numMunicion = 10;
				GlobalObject.deleteHandL ();
			} else {
				//Sin armas
			}//excitacion


			GameObject GrupoMunicion = new GameObject();
			GrupoMunicion.name = "Municion";

			//Si el jugador tiene armas que necesitan municion 
			if (excitacion < 0.75 && excitacion >= 0.25) {

				for (int i = 0; i < numMunicion; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							GameObject aux = Instantiate (Municion, box.gameObject.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = GrupoMunicion.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}
			}


			//VALENCIA
			int numRandom = (int)Random.Range (5, 20);

			GameObject Animals = new GameObject();
			Animals.name = "Animales";

			GameObject Enemigos = new GameObject();
			Enemigos.name = "Enemigos";

			if (valencia >= 0.75) {

				//Colocamos aleatoriamente animales en el mapa
				for (int i = 0; i < numRandom; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							randomIndex = Random.Range (0, GlobalObject.sizeAnimal ());
							//Bajamos la posicion del arbol para que en las cuestas la raiz atraviese el suelo y no quede levitando
							//Vector3 newPosition = new Vector3 (box.transform.position.x, box.transform.position.y - 1.0f, box.transform.position.z);
							GameObject aux = Instantiate (GlobalObject.GetAnimal (randomIndex), box.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = Animals.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}

			} else if (valencia >= 0.5) {

				for (int i = 0; i < numRandom; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							randomIndex = Random.Range (0, GlobalObject.sizeEnemigos ());

							GameObject aux = Instantiate (GlobalObject.GetEnemigos (randomIndex), box.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = Enemigos.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}

			} else if (valencia >= 0.25) {
			
				for (int i = 0; i < numRandom; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							randomIndex = Random.Range (0, GlobalObject.sizeEnemigos ());

							GameObject aux = Instantiate (GlobalObject.GetEnemigos (randomIndex), box.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = Enemigos.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}

			} else {
				
				for (int i = 0; i < numRandom; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							randomIndex = Random.Range (0, GlobalObject.sizeEnemigos ());

							GameObject aux = Instantiate (GlobalObject.GetEnemigos (randomIndex), box.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = Enemigos.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}

				//Enemigos grandes
				numRandom = (int)Random.Range (2, 5);

				for (int i = 0; i < numRandom; ++i) {

					randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
					cont = 0;

					foreach (GameObject box in listGreenBox) {
						if (randomIndex == cont) {
							randomIndex = Random.Range (0, GlobalObject.sizeEnemigosGrandes ());

							GameObject aux = Instantiate (GlobalObject.GetEnemigosGrandes (randomIndex), box.transform.position, box.transform.rotation) as GameObject;
							aux.transform.parent = Enemigos.transform;
							listGreenBox.Remove (box);
							break;
						} else {
							++cont;
						}
					}

				}

			}//valencia

		}//0.9 se suspense

		Vector3 playerPosition = new Vector3 (masterCube.gameObject.transform.position.x, masterCube.gameObject.transform.position.y + 1.0f, masterCube.gameObject.transform.position.z);
		Player.gameObject.transform.position = playerPosition;
		GameObject testPlayerCofres = Instantiate (testCofre, masterCube.gameObject.transform.position, masterCube.gameObject.transform.rotation) as GameObject;
		//Colocamos los cofres

		int contadorCofres = 1;
		GameObject Cofres = new GameObject();
		Cofres.name = "Cofres";

		for (int i = 0; i < numCofres; ++i) {

			randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
			cont = 0;

			foreach (GameObject box in listGreenBox) {
				if (randomIndex == cont) {
					GameObject cofreColocado = Instantiate (cofre, box.gameObject.transform.position, cofre.transform.rotation) as GameObject;
					cofreColocado.transform.parent = Cofres.transform;
					testPlayerCofres.GetComponent<testNacMeshJugadorCofres> ().getPosition (cofreColocado.transform.position, contadorCofres);
					++contadorCofres;
					listGreenBox.Remove (box);

					//NavMeshVisualizationSettings.

					break;
				} else {
					++cont;
				}
			}

		}

		//Destroy (testPlayerCofres.gameObject);
	}//putElement()

	public Vector3 GetPositionGreenCube(){

		Vector3 position = new Vector3 ();

		if (listGreenBoxCreated) {
			int randomIndex = (int)Random.Range (0, listGreenBox.Count - 1);
			int cont = 0;


			foreach (GameObject box in listGreenBox) {
				if (randomIndex == cont) {
					position = box.transform.position;
					break;

				} else {
					++cont;
				}
			}
		}

		return position;
	}
}
