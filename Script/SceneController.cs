using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void gotoReload(){
		SceneManager.LoadScene ("GenerarMapa");
	}

	public void gotoMenu(){
		SceneManager.LoadScene ("Inicio");
	}

	public void gotoCreditos(){
		SceneManager.LoadScene ("Creditos");
	}
}
