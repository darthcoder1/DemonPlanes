﻿using UnityEngine;
using System.Collections;

public class DemonBehavior : MonoBehaviour 
{

	public int VillageDamagePerSecond = 5;

	private GameObject TargetVillage;
	private HealthBar HealthBarComp;


	public float WalkingSpeed;
	public double TimeToDisappear; // the time between the demon reaching 0 HP and disappearing

	private bool bSoundPlayed;
	private double TimeTillDeath;
	private float HealthScale; 

	public AudioSource DemonExtinguishSFX;
	public AudioSource DemonDefeatedSFX;

	// Use this for initialization
	void Start () 
	{
		HealthBarComp = GetComponent<HealthBar> ();
		TimeTillDeath = 0;
		GameObject[] foundVillages = GameObject.FindGameObjectsWithTag("village");
		GameObject nearestVillage = null;
		float nearestVillageDistance = float.MaxValue;
		foreach (GameObject village in foundVillages)
		{
			float dist = Vector2.Distance(village.transform.position, transform.position);
			if (nearestVillage == null || dist < nearestVillageDistance)
			{
				nearestVillage = village;
				nearestVillageDistance = dist;
			}
		}
		TargetVillage = nearestVillage;

		//play sound when spawning
		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();
		//DemonDefeatedSFX.Play ();


	}
	
	// Update is called once per frame
	void Update () 
	{
		//scale demon size
		if (HealthBarComp.Health > 0) 
		{
			HealthScale=Mathf.Max((float)HealthBarComp.Health/100.0f, 0.25f); 
			transform.localScale = new Vector3(HealthScale,HealthScale,HealthScale);
			print (HealthScale);
			//transform.localScale = new Vector3(0.5f,0.5f,0.5f);
		}

		if (HealthBarComp.Health < HealthBarComp.MaxHealth * 0.8f)
		{
			if(!bSoundPlayed)
			{
				bSoundPlayed=true;
				DemonExtinguishSFX.Play();

			}
		}
		if (HealthBarComp.Health <= 0)
		{

			Die();
			return;


		}

		Vector3 WalkDir = Vector3.Normalize(TargetVillage.transform.position - transform.position);
		Quaternion randomRotation = Quaternion.AngleAxis((float)Random.Range(-10, 10), Vector3.back);

		transform.position += (randomRotation * WalkDir) * WalkingSpeed * Time.deltaTime;
	}

	void Die()
	{
	   
		if (TimeTillDeath == 0) {
			//play sound when dieing
			DemonDefeatedSFX.Play ();
			//CountingForDeath=true;
			TimeTillDeath = Time.time + TimeToDisappear;
		} 
		else if (Time.time > TimeTillDeath)
		{
			GameObject.Destroy (gameObject);
		}
	}
}
