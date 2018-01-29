using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class menuInicio : MonoBehaviour {

	public Toggle checkPequenio;
	public Toggle checkMediano;
	public Toggle checkGrande;

	public Scrollbar Valencia;
	public Scrollbar Excitacion;
	public Scrollbar Dominio;

	public Text SuspenseValue;

	private float suspense = 0.0f;

	// Use this for initialization
	void Start () {

		SuspenseValue.text = "0";

		//Asinamos tamaño a mapa
		tamañoMapa ();
	}

	public void tamañoMapa(){

		if (checkPequenio.isOn) {
			PlayerPrefs.SetInt ("TamanioMapa", 257);
		} else if (checkMediano.isOn) {
			PlayerPrefs.SetInt ("TamanioMapa", 513);
		} else {
			PlayerPrefs.SetInt ("TamanioMapa", 1025);
		}
	}

	public void StartGame(){

		PlayerPrefs.SetFloat ("Valencia", Valencia.value);
		PlayerPrefs.SetFloat ("Excitacion", Excitacion.value);
		PlayerPrefs.SetFloat ("Dominio", Dominio.value);

		PlayerPrefs.SetFloat ("Suspense", suspense);

		SceneManager.LoadScene ("GenerarMapa");
	}

	public void calcularNivelSuspense(){

		float maxValue = 1.0f, minValue = 0.0f;

		//Aplicacion de la funcion para obtener el nivel de suspense del juego
		suspense = (maxValue - Valencia.value + Excitacion.value - minValue + maxValue - Dominio.value) / 3;			

		SuspenseValue.text = suspense.ToString();
	}
}
