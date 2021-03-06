﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlResultados : MonoBehaviour {

	public Text Resultado;
	public Text Cofres;
	public Text Niveles1;
	public Text Niveles2;
	private AudioSource source;

	public AudioClip victorySound;
	public AudioClip loseSound;
	public float volumenSound = 0.5f;

	// Use this for initialization
	void Start () {

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		source = GetComponent<AudioSource> ();
		int cofresTotales = PlayerPrefs.GetInt ("CofresTotales");
		int cofresEncontrados = PlayerPrefs.GetInt ("CofresEncontrados");
		float valencia = PlayerPrefs.GetFloat ("Valencia");
		float excitacion = PlayerPrefs.GetFloat ("Excitacion");
		float dominio = PlayerPrefs.GetFloat ("Dominio");

		float suspense = PlayerPrefs.GetFloat ("Suspense");

		Cofres.text = "Cofres encontrados: " + cofresEncontrados.ToString () + " - Cofres totales: " + cofresTotales.ToString ();

		if (cofresEncontrados == cofresTotales) {
			Resultado.text = "VICTORIA";
			source.PlayOneShot (victorySound, volumenSound);
		} else {
			Resultado.text = "DERROTA";
			source.PlayOneShot (loseSound, volumenSound);
		}

		Niveles1.text = "Nivel Valencia: "+ String.Format("{0:0.00}", valencia) +"    Nivel Excitacion: " + String.Format("{0:0.00}", excitacion);
		Niveles2.text = "Nivel Dominio: "+ String.Format("{0:0.00}", dominio) +"    Nivel Suspense: "+String.Format("{0:0.00}", suspense);
	}
	

}
