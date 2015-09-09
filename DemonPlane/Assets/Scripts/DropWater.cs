using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropWater : MonoBehaviour 
{

	public float WaterCollisionCheckInterval; // Specifies the interval in which the dropped water is checking for collision
	public float DropWaterInheritVelocity; // This must match the value from the particlesystem
	public ParticleSystem DropWaterFX;

	public float TimeForWaterToDrop = 3;
	public float WaterDropRadius = 1;
	public int WaterDamage = 25;

	public bool DroppingWater { get { return bDroppingWater; } }

	private float TimeSinceLastWaterCollisionCheck;
	private List<Vector3> WaterCollisionCheckList;
	private bool bDroppingWater;
	private PlayerDetails PlayerDetailsComp;

	// Use this for initialization
	void Start () 
	{
		WaterCollisionCheckList = new List<Vector3>();
		TimeSinceLastWaterCollisionCheck = 0.0f;

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
		if (PlayerDetailsComp.CurrentAmmo > 0) 
		{
			bDroppingWater = true;
			DropWaterFX.enableEmission = true;
		}
	}

	public void DropWaterStop()
	{
		bDroppingWater = false;
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
				demon.Health -= WaterDamage;
			}
		}
	}
}
