using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCube : MonoBehaviour {
	
	public bool valid = true;
	public TerrainData terrainData;

	public int top;
	public int down;
	public int right;
	public int left;

	private float limitHeight = 25.0f;
	// Use this for initialization
	void Start () {

		top = 0;
		down = 0;
		right = 0;
		left = 0;
	}

	public void setMap(TerrainData t){

		terrainData = t;
	}

	public void setName(int i, int j){

		this.gameObject.name = i + "-" + j;
	}
	
	public string ToString(){
		return name + " [Top: "+top+" ,Down: "+down+" ,Right: "+right+" ,Left: "+left+"]";
	}

	public void CalcDistance(){

		string[] aux = name.Split ('-');

		int iterI = int.Parse(aux[0]);
		int iterJ = int.Parse(aux[1]);

		//Top
		for(int i = iterI - 5; i >= 0 ; i -= 5){

			if (terrainData.GetHeight(i,iterJ) > limitHeight) {
				break;
			} else {
				++top;
			}

		}//for top

		//Down
		for(int i = iterI + 5; i < terrainData.alphamapHeight - 10 ; i += 5){

			if (terrainData.GetHeight(i,iterJ) > limitHeight) {
				break;
			} else {
				++down;
			}

		}//for down

		//Left
		for(int j = iterJ - 5; j >= 0 ; j -= 5){

			if (terrainData.GetHeight(iterI,j) > limitHeight) {
				break;
			} else {
				++left;
			}

		}//for left

		//Right
		for(int j = iterJ + 5; j < terrainData.alphamapWidth - 10 ; j += 5){

			if (terrainData.GetHeight(iterI,j) > limitHeight) {
				break;
			} else {
				++right;
			}

		}//for right

	}//calcDistance()
}
