using UnityEngine;
using System.Collections;

public class DemonBehavior : MonoBehaviour 
{

	public int MaxHealth;

	private GameObject TargetVillage;

	public int Health;

	public float WalkingSpeed;

	private bool bSoundPlayed;

	private GameObject DemonExtinguishSFX;

	// Use this for initialization
	void Start () 
	{
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

		Health = MaxHealth;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Health < Health / 2)
		{
			/*
			if(!bSoundPlayed)
			{
				bSoundPlayed=true;
				AudioSource audio = DemonExtinguishSFX.GetComponent<AudioSource>();
				audio.Play();
				audio.Play(44100);
			}
		
		*/
		}
		if (Health <= 0)
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
		GameObject.Destroy(gameObject);
	}
}
