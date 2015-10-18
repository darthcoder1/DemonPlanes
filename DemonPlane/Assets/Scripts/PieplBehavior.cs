using UnityEngine;
using System.Collections;

public class PieplBehavior : MonoBehaviour {


	//the time in seconds before the piepl will be active
	public float SpawnDelay; 
	//the time in seconds the pieple will be alive
	public float LifeTime;

	public string PigName;

	public bool isSpecial;
	// Use this for initialization
	void Start () {


		int CurrentWave = GameObject.Find("GlobalManager").GetComponent<PieplSpawner>().CurrentWave;
		isSpecial = false;
		if(PigName=="default")
		{

			if ((Random.Range (0, CurrentWave + 10) < 3) ) {
				isSpecial = true;

			} else {

				isSpecial = false;
				int rare = Random.Range (0, 20);
				GameObject TheJustSpawnedPiepl=null;

				if(rare  < 6) 
				{
					if (rare == 0 || rare == 1) TheJustSpawnedPiepl=(GameObject)Instantiate (Resources.Load ("zombiepiggy"), transform.position, transform.rotation);
					if (rare == 2 || rare == 3) TheJustSpawnedPiepl=(GameObject)Instantiate (Resources.Load ("darkpiggy"), transform.position, transform.rotation);
					if (rare == 4 || rare == 5) TheJustSpawnedPiepl=(GameObject)Instantiate (Resources.Load ("rainbowpiggy"), transform.position, transform.rotation);

					TheJustSpawnedPiepl.GetComponent<PieplBehavior>().SpawnDelay= Random.Range(1,65+CurrentWave*2);
					TheJustSpawnedPiepl.GetComponent<PieplBehavior>().LifeTime= Random.Range(35,75+CurrentWave*2);
					Destroy(gameObject);
				}
			}
		}
		//isSpecial=true;
	
	}
	// This is called from the piepl spawner
	public void StartForReal () {
		
		Invoke ("Spawn", SpawnDelay);
		//print ("spawn delay" + SpawnDelay.ToString ());
				
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
