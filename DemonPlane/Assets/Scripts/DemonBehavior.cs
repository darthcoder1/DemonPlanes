using UnityEngine;
using System.Collections;

public class DemonBehavior : MonoBehaviour 
{

	public int VillageDamagePerSecond = 5;

	private GameObject TargetVillage;
	private HealthBar HealthBarComp;


	public float WalkingSpeed;
	public double TimeToDisappear; // the time between the demon reaching 0 HP and disappearing

	//rotation
	private Transform target;

	private bool bSoundPlayed;
	private double TimeTillDeath;
	private float HealthScale; 
	private bool hit;

	public AudioSource DemonExtinguishSFX;
	public AudioSource DemonDefeatedSFX;

	public GameObject HitFX;

	// Use this for initialization
	void Start () 
	{
		HealthBarComp = GetComponent<HealthBar> ();
		TimeTillDeath = 0;
		hit = false;
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
		target = TargetVillage.transform;

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
			HealthScale=Mathf.Max((float)HealthBarComp.Health/30.0f, 0.25f); 
			if(HealthScale <1) 
			{
				transform.localScale = new Vector3(HealthScale,HealthScale,HealthScale);
			}


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
		//move
		Vector3 WalkDir = Vector3.Normalize(TargetVillage.transform.position - transform.position);
		Quaternion randomRotation = Quaternion.AngleAxis((float)Random.Range(-10, 10), Vector3.back);

		transform.position += (randomRotation * WalkDir) * WalkingSpeed * Time.deltaTime;
		///rotate
		Vector3 relativePos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		//transform.rotation = rotation;
		if (hit)
			HitUpdate ();

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

            GameObject.FindGameObjectWithTag("Player").SendMessage("DemonKilled");
		}
	}
	public void Hit()
	{

		//spawn a ash particle
		HitFX=(GameObject)Instantiate (Resources.Load ("DemonHit"), transform.position, transform.rotation);
		hit = true;
	}
	void HitUpdate()
	{
		HitFX.transform.position = transform.position;

	}


}
