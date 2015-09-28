using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DropWater : MonoBehaviour 
{

	public float WaterCollisionCheckInterval; // Specifies the interval in which the dropped water is checking for collision
	public float DropWaterInheritVelocity; // This must match the value from the particlesystem
	public ParticleSystem DropWaterFX;

	public float TimeForWaterToDrop = 3;
	public float WaterDropRadius = 1;
	public int WaterDamage = 25;
	public float ShootingDelay; // time betweeen each shot
	public float WaterBulletSpeed; // the speed of a water shot
	public int WaterBulletDamage = 2;

	private AudioSource [] SFX;
	private AudioSource OutOfWaterSFX;
	private AudioSource DropWaterSFX;
	public Text OutOfWaterText;



	public bool DroppingWater { get { return bDroppingWater; } }

	public bool ShootingWater { get { return bShootingWater; } }

	private float TimeSinceLastWaterCollisionCheck;
	private List<Vector3> WaterCollisionCheckList;
	private bool bDroppingWater;
	private bool bShootingWater;
	private PlayerDetails PlayerDetailsComp;

	// Use this for initialization
	void Start () 
	{
		WaterCollisionCheckList = new List<Vector3>();
		TimeSinceLastWaterCollisionCheck = 0.0f;
		SFX = GetComponents<AudioSource>();
		OutOfWaterSFX = SFX[2];
		DropWaterSFX = SFX[1];
		OutOfWaterText.enabled = false;
		DropWaterFX.startLifetime = TimeForWaterToDrop;

		PlayerDetailsComp = GetComponent<PlayerDetails> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		TimeSinceLastWaterCollisionCheck += Time.deltaTime;

		if (bDroppingWater)
		{
			if (TimeSinceLastWaterCollisionCheck >= WaterCollisionCheckInterval)
			{
				Vector3 flightVel = PlayerDetailsComp.FlightVelocity;
				TimeSinceLastWaterCollisionCheck = 0.0f;
				WaterCollisionCheckList.Add(transform.position +  flightVel * DropWaterInheritVelocity * TimeForWaterToDrop);
				Invoke("CheckWaterCollision", TimeForWaterToDrop);
			}

			int AmmoToRemove = (int)Mathf.Floor((float)PlayerDetailsComp.AmmoUsagePerSecond * Time.deltaTime);
			PlayerDetailsComp.CurrentAmmo = Mathf.Clamp(PlayerDetailsComp.CurrentAmmo - AmmoToRemove, 0, PlayerDetailsComp.MaxAmmo);

			if (PlayerDetailsComp.CurrentAmmo <= 0)
			{
				DropWaterStop();

			}
		}

	}


	public void DropWaterStart()
	{
		if (PlayerDetailsComp.CurrentAmmo > 0) {
			bDroppingWater = true;
			if(!DropWaterSFX.isPlaying)DropWaterSFX.Play ();
			DropWaterFX.enableEmission = true;
		} else {
			SFX[1].Play();
		}
	}

	public void ShootWater()
	{
		if (PlayerDetailsComp.CurrentAmmo > 0 && !bShootingWater) {
			bShootingWater = true;
			GameObject WaterShot;
			ControllerScript ControllerComp = GetComponent<ControllerScript>();
			int AmmoToRemove = (int)Mathf.Floor((float)PlayerDetailsComp.AmmoUsagePerSecond * Time.deltaTime);
			PlayerDetailsComp.CurrentAmmo = Mathf.Clamp(PlayerDetailsComp.CurrentAmmo - AmmoToRemove, 0, PlayerDetailsComp.MaxAmmo);
			WaterShot=(GameObject)Instantiate (Resources.Load ("watershot"), ControllerComp.transform.position,ControllerComp.transform.rotation);
			WaterShot.GetComponent<WaterShot>().Direction=ControllerComp.Direction;
			WaterShot.GetComponent<WaterShot>().currentSpeed=ControllerComp.MaxSpeed+WaterBulletSpeed;
			Invoke("ResetShooting", ShootingDelay);

		}
	}
	void ResetShooting()
	{
		bShootingWater = false;
	}


	public void DropWaterStop()
	{
		bDroppingWater = false;
		if (DropWaterSFX.isPlaying) Invoke ("StopWaterDroppingSound", 1);
		DropWaterFX.enableEmission = false;
	}

	
	void CheckWaterCollision()
	{
		Vector3 Pos = WaterCollisionCheckList[0];
		WaterCollisionCheckList.RemoveAt(0);
		
		Debug.DrawLine(Pos - Vector3.up * WaterDropRadius, Pos + Vector3.up * WaterDropRadius, Color.red);
		Debug.DrawLine(Pos - Vector3.left * WaterDropRadius, Pos + Vector3.left * WaterDropRadius, Color.red);
		
		Collider2D[] HitObjects = Physics2D.OverlapCircleAll(Pos, WaterDropRadius);
		
		foreach (Collider2D coll in HitObjects)
		{
			if (coll.CompareTag("forest"))
			{
                FireCell cell = coll.gameObject.GetComponent<FireCell>();

                if (cell)
                {
                    cell.CurrentHealth -= WaterDamage;
                }
			}
			else if (coll.CompareTag("enemy"))
			{
				HealthBar demon = coll.GetComponent<HealthBar>();
				DemonBehavior demon_behavior = coll.GetComponent<DemonBehavior>();
				demon.Health -= WaterDamage;
				demon_behavior.Hit();
			}
		}
	}
	
	void StopWaterDroppingSound()
	{
		//AudioFadeOut.FadeOut (DropWaterSFX, 0.1f);

		DropWaterSFX.Stop ();

	}

}
