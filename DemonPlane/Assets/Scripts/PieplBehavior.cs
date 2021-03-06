﻿using UnityEngine;
using System.Collections;

public class PieplBehavior : MonoBehaviour {


	//the time in seconds before the piepl will be active
	public float SpawnDelay; 
	//the time in seconds the pieple will be alive
	public float LifeTime;

	public string PigName;

	public bool isSpecial = false;

	// Use this for initialization
	void Start () 
    {

	}

	// This is called from the piepl spawner
	public void StartForReal () {
		
		Invoke ("Spawn", SpawnDelay);
	}

	//from now on the piepl can be picked up and all - also now we start counting the life time
	void Spawn () {
		
		GetComponent<SpriteRenderer> ().enabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;
		Invoke ("Die", LifeTime);
	}


	// Update is called once per frame
	/*
	void Update () {
	
	}*/
	void Die()
	{
		GameObject.Destroy(gameObject);
	}
}
