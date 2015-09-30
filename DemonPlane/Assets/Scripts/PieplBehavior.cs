using UnityEngine;
using System.Collections;

public class PieplBehavior : MonoBehaviour {


	//the time in seconds before the piepl will be active
	public float SpawnDelay; 
	//the time in seconds the pieple will be alive
	public float LifeTime;

	public bool isSpecial;
	// Use this for initialization
	void Start () {


		int CurrentWave = GameObject.Find("GlobalManager").GetComponent<PieplSpawner>().CurrentWave;


		if((Random.Range(0, CurrentWave +1) < 3)) {
			isSpecial=true;
			print("SPECIAL PIG BORN!");
		}
		else isSpecial = false;
	
	}
	// This is called from the piepl spawner
	public void StartForReal () {
		
		Invoke ("Spawn", SpawnDelay);
		//print ("spawn delay" + SpawnDelay.ToString ());
				
	}

	//from now on the piepl can be picked up and all - also now we start counting the life time
	void Spawn () {
		
		if(isSpecial) print("I AM SO SPECIAL!!");
		else  print("I AM ORDINARY");
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
