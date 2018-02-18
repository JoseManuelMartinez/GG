using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.AI;
using UnityEngine.AI;

public class TestMountainGenerator : MonoBehaviour {

	private TerrainData terrainData;
	private List<GameObject> listGreenBox; //Lista donde se almacenan las casillas validas
	private int SpaceBerweenCube = 10;
	private int sizeMap;

	private GameObject Player;
	public GameObject Map;


	public int perlinPasses = 1;
	public int perlinScaleRangex = 2; 
	public int perlinScaleRangey = 2;
	public float limit = 0.25f;
	public float limitHeight = 25.0f;

	//Prefap de casilla valida o no valida
	public GameObject redCube;
	public GameObject greenCube;

	public int Resultado = 0;

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

		//Debug.Log ("TERRAIN DATA");
		//Debug.Log (terrainMap.terrainData.alphamapHeight);
		//Debug.Log (terrainMap.terrainData.alphamapWidth);
		//Debug.Log (terrainMap.terrainData.heightmapHeight);
		//Debug.Log (terrainMap.terrainData.heightmapWidth);

		GenerateHeightmap ();
		//PaintMap ();

		this.gameObject.isStatic = true;

		scannerMap ();


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

		for (int y = 0; y < terrainData.heightmapHeight; y++){
			for (int x = 0; x < terrainData.heightmapWidth; x++){
				float height = terrainData.GetHeight(y,x);
				if (height>max_altura) max_altura = height;
			}
		}

		float[, ,] pTexturas= new float[terrainData.heightmapWidth, terrainData.heightmapHeight, terrainData.alphamapLayers];

		float amplitud = max_altura/terrainData.alphamapLayers; 

		float altura = 0.0f;
		float peso = 0.0f;

		for (int y = 0; y < terrainData.heightmapHeight; y++) {
			for (int x = 0; x < terrainData.heightmapWidth; x++) {

				altura = terrainData.GetHeight(y,x);

				int textura_base = (int) Mathf.Floor(altura/amplitud);

				if (textura_base>terrainData.alphamapLayers-1) textura_base = terrainData.alphamapLayers-1;

				/*pTexturas[x,y,textura_base] = peso;
				if (textura_base<terrainData.alphamapLayers-1) pTexturas[x,y,textura_base+1] = (1.0f-peso)/2; // Comentar para evitar el mezclado
				if (textura_base>1) pTexturas[x,y,textura_base-1] = (1.0f-peso)/2; // comentar para evitar el mezclado*/

				if (altura > limit * 2)
					peso = Random.Range (0.3f, 0.4f);
				else
					peso = 0.5f;

				pTexturas[x,y,textura_base] = peso;
				//if (textura_base<terrainData.alphamapLayers-1) pTexturas[x,y,textura_base+1] = (1.0f-peso)/2; // Comentar para evitar el mezclado
				//if (textura_base>1) pTexturas[x,y,textura_base-1] = (1.0f-peso)/2; // comentar para evitar el mezclado
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
		for (int x = SpaceBerweenCube; x < terrainData.heightmapHeight - SpaceBerweenCube; x+= SpaceBerweenCube) {
			for (int y = SpaceBerweenCube; y < terrainData.heightmapWidth - SpaceBerweenCube; y+=SpaceBerweenCube) {

				posicion = new Vector3 (x, terrainData.GetHeight (x, y), y);

				if (terrainData.GetHeight (x, y) > limitHeight) {
					GameObject box = Instantiate (redCube, posicion, transform.rotation) as GameObject;
					box.GetComponent<GreenCube> ().setName (x,y);
					box.GetComponent<GreenCube> ().setMap (terrainData);
					box.GetComponent<GreenCube> ().valid = false;
					contCubosRojos++;
				} else {
					GameObject box = Instantiate (greenCube, posicion, transform.rotation) as GameObject;
					box.GetComponent<GreenCube> ().setName (x,y);
					box.GetComponent<GreenCube> ().setMap (terrainData);
					listGreenBox.Add(box);
					contCubosVerdes++;
				}
			}
		}

		int total = contCubosRojos + contCubosVerdes;
		int porcientoAcierto = contCubosVerdes * 100 / total;

		//Debug.Log ("Casillas totales: " + total + ". Casillas validas: " + contCubosVerdes + ". Casillas no validas: " + contCubosRojos);
		//Debug.Log ("El porcentaje de terreno jugable es del "+porcientoAcierto+" %");

		if (porcientoAcierto >= 60) {
			Debug.Log ("Terreno jugable superior al 60% ? PASS");
		} else {
			Debug.Log ("Terreno jugable superior al 60% ? FAIL");
		}


		//Si no hay mas de un 60% del terreno jugable, se genera otro terreno y se borra el actual
		/*if (porcientoAcierto < 60) {

			GameObject MapGenerate = Instantiate (Map, transform.position, transform.rotation) as GameObject;
			Destroy (this.gameObject);
		}*/


		Resultado = porcientoAcierto;


	}//scannerMap() 


}
