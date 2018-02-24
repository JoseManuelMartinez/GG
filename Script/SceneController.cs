using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public Canvas canvas;

	public void gotoReload(){
		canvas.gameObject.SetActive (false);
		SceneManager.LoadScene ("GenerarMapa");
	}

	public void gotoMenu(){
		SceneManager.LoadScene ("Inicio");
	}

	public void gotoCreditos(){
		SceneManager.LoadScene ("Creditos");
	}

	public void quitGame(){
		Application.Quit ();
	}
}
