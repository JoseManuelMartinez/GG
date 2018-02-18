using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shot : MonoBehaviour {

	public float velocity = 2.0f;
	public int damage = 35;

	// Use this for initialization
	void Start () {
		Destroy (this.gameObject,10);
	}
	
	// Update is called once per frame
	void Update () {

		this.transform.Translate(Vector3.down * velocity * Time.deltaTime);
	}

	public void setDamage(int dam){damage = dam;}
	public int getDamage(){return damage ;}

	void OnTriggerEnter(Collider c){
		if(c.gameObject.tag == "Enemy"){
			Destroy (this.gameObject);
		}
	}
}
