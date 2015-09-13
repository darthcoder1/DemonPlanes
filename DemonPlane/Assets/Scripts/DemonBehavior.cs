using UnityEngine;
using System.Collections;

public class DemonBehavior : MonoBehaviour 
{

	public int VillageDamagePerSecond = 5;

	private GameObject TargetVillage;
	private HealthBar HealthBarComp;
	//private bool CountingForDeath;

	public float WalkingSpeed;

	private bool bSoundPlayed;
	private double TimeTillDeath;

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
			TimeTillDeath = Time.time + 10.0;
		} 
		else if (Time.time > TimeTillDeath)
		{
			GameObject.Destroy (gameObject);
		}
	}
}
