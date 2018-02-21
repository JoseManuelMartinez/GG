using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunsController : MonoBehaviour {

	public GameObject armaPequenia;
	public GameObject armaMediana;
	public GameObject armaGrande;

	public GameObject shotArmaPequenia;
	public GameObject shotArmaMediana;
	public GameObject shotArmaGrande;

	public void activaArmaPequenia(){armaPequenia.SetActive (true);}
	public void activaArmaMediana(){armaMediana.SetActive (true);}
	public void activaArmaGrande(){armaGrande.SetActive (true);}

	public void ponerMunicion(int m){
		
		shotArmaPequenia.GetComponent<gun> ().setMunicion (m);
		shotArmaMediana.GetComponent<gun> ().setMunicion (m);
		shotArmaGrande.GetComponent<gun> ().setMunicion (m);
	}

}
