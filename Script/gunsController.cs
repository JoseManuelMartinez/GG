using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunsController : MonoBehaviour {

	public GameObject armaPequenia;
	public GameObject armaMediana;
	public GameObject armaGrande;

	public void activaArmaPequenia(){armaPequenia.SetActive (true);}
	public void activaArmaMediana(){armaMediana.SetActive (true);}
	public void activaArmaGrande(){armaGrande.SetActive (true);}

	public void ponerMunicion(int m){
		armaPequenia.GetComponent<gun> ().setMunicion (m);
		armaMediana.GetComponent<gun> ().setMunicion (m);
		armaGrande.GetComponent<gun> ().setMunicion (m);
	}

}
