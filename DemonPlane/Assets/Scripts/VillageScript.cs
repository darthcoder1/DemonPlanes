﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VillageScript : MonoBehaviour 
{
	List<GameObject> DemonsInVillage;
	private HealthBar HealthBarComp;
	private GameObject FireSpawnPoint;
	private GameObject VillagBurns;
	public Text VillageIntact;
	private int VillageHealth;

    public bool IsDestroyed { get { return HealthBarComp.Health <= 0; } }

	// Use this for initialization
	void Start () 
	{
		DemonsInVillage = new List<GameObject> ();
		//HealthBarComp = GetComponent<HealthBar> ();
		VillageHealth = 100;
		VillageIntact.text=VillageHealth.ToString()+"% of village intact";
		VillageIntact.enabled = true;
	}
	
	// Update is called once per frame
	void Update () 
	{

		VillageIntact.text=VillageHealth.ToString()+"% of village intact";
		/*
		float calcHealth = (float)HealthBarComp.Health;
		foreach (GameObject Demon in DemonsInVillage) 
		{
			float DmgPerSec = (float)(Demon.GetComponent<DemonBehavior>().VillageDamagePerSecond);
			calcHealth -= DmgPerSec * Time.deltaTime;
		}

		HealthBarComp.Health = Mathf.FloorToInt (Mathf.Max (calcHealth, 0.0f));
		*/
	}
	
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			if(VillageHealth > 0)
			{
				GetComponent<AudioSource>().Play();
				GameObject[] foundFireSpawnpoints = GameObject.FindGameObjectsWithTag("village_fire_spawn");
				GameObject nearestFireSpawn = null;
				print("Demon in village");
				Vector3 demonPos = collision.gameObject.transform.position;
				GameObject.Destroy (collision.gameObject);

				float nearestFireSpawnDistance = float.MaxValue;
				foreach (GameObject spawnPoint in foundFireSpawnpoints)
				{
					float dist = Vector2.Distance(spawnPoint.transform.position, demonPos);
					if (nearestFireSpawn == null || dist < nearestFireSpawnDistance)
					{
						print ("spawn a fire");
						nearestFireSpawn = spawnPoint;
						nearestFireSpawnDistance = dist;
					}
				}
				FireSpawnPoint = nearestFireSpawn;
				VillageHealth -= 10;
				VillagBurns=(GameObject)Instantiate (Resources.Load ("Smoke_01"), FireSpawnPoint.transform.position, FireSpawnPoint.transform.rotation);
				GameObject.Destroy (FireSpawnPoint);
				if(VillageHealth == 10) Die ();	
			}
						

		}
	}
	
	void OnTriggerExit2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			DemonsInVillage.Remove (collision.gameObject);
		}
	}

	void Die()
	{
		GameObject.FindGameObjectWithTag ("Player").SendMessage ("Die");
	}
}
