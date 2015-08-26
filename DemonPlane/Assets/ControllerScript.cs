﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ControllerScript : MonoBehaviour {

	public int PlayerId;				// Id of the player, zero based
	public float MinSpeed;				// Minimum speed of the airplane (this is the speed when stick is pulled back)
	public float NormalSpeed;			// Normal flight speed, when stick is neither pulled nor pushed)
	public float MaxSpeed;				// Max flight speed reached when stick is pressed forward
	public float RotationSpeed;			// Rotation speed of the airplane
	public float AltitudeChangeSpeed;	// Specifies how fast altitude is changed
	public float WaterCollisionCheckInterval; // Specifies the interval in which the dropped water is checking for collision
	public float DropWaterInheritVelocity; // This must match the value from the particlesystem
	private bool bDied;

	private float Altitude; // 0.0 == ground, 1.0f == normal flight height
	private float TargetAltitude;
	private Vector3 OriginalScale;
	private Vector3 Direction;
	private float TimeSinceLastWaterCollisionCheck;
	private List<Vector3> WaterCollisionCheckList;

	private PlayerDetails PlayerDetailsComp;
	public Text GameOverText;

	private readonly string HorizontalAxixName;
	private readonly string VerticalAxixName;
	private readonly string ClimbButtonName;
	private readonly string SinkButtonName;
	private readonly string ReleaseWaterName;

	ControllerScript()
	{
		string PlayerIdStr = PlayerId.ToString();

		HorizontalAxixName = "Horizontal" + PlayerIdStr;
		VerticalAxixName = "Vertical" + PlayerIdStr;
		ClimbButtonName = "AltClimb" + PlayerIdStr;
		SinkButtonName = "AltSink" + PlayerIdStr;
		ReleaseWaterName = "ReleaseWater" + PlayerIdStr;
	}

	// Use this for initialization
	void Start () 
	{
		Altitude = TargetAltitude = 1.0f;
		Direction = new Vector3 (0, 1, 0);
		bDied = false;

		PlayerDetailsComp = GetComponent<PlayerDetails>();
		GameOverText.text = "";

		WaterCollisionCheckList = new List<Vector3>();
		TimeSinceLastWaterCollisionCheck = 0.0f;

		OriginalScale = transform.localScale;
	}
	

	void Die()
	{
		bDied = true;
		GameOverText.text = "Game Over!";
		Invoke("RestartLevel", 3.0f);
	}

	void RestartLevel()
	{
		Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}

	void CheckWaterCollision()
	{
		Vector3 Pos = WaterCollisionCheckList[0];
		WaterCollisionCheckList.RemoveAt(0);

		Debug.DrawLine(Pos, Pos + Vector3.up * PlayerDetailsComp.WaterDropRadius, Color.red);
		Debug.DrawLine(Pos, Pos + Vector3.left * PlayerDetailsComp.WaterDropRadius, Color.red);
		Debug.DrawLine(Pos, Pos + Vector3.right * PlayerDetailsComp.WaterDropRadius, Color.red);
		Debug.DrawLine(Pos, Pos + Vector3.down * PlayerDetailsComp.WaterDropRadius, Color.red);

		Collider2D[] HitObjects = Physics2D.OverlapCircleAll(Pos, PlayerDetailsComp.WaterDropRadius);

		foreach (Collider2D coll in HitObjects)
		{
			if (coll.CompareTag("forest"))
			{

			}
			else if (coll.CompareTag("enemy"))
			{
				DemonBehavior demon = coll.GetComponent<DemonBehavior>();
				demon.Health -= PlayerDetailsComp.WaterDamage;

			}
		}
	}

	// Update is called once per frame
	void Update () 
	{

		if (bDied) { return; }

		float hAxis = Input.GetAxis (HorizontalAxixName);
		float vAxis = Input.GetAxis (VerticalAxixName);

		float pressedAltClimb = Input.GetAxis (ClimbButtonName);
		float pressedAltSink = Input.GetAxis (SinkButtonName);

		// Rotation
		float rotateBy = RotationSpeed * Time.deltaTime * hAxis;

		Quaternion rotateByQuat = Quaternion.AngleAxis (rotateBy, Vector3.back);
		Vector3 newDirection = rotateByQuat * Direction;

		Vector3 position = transform.position;
		Debug.DrawLine( position, position + Direction * 100, Color.red );

		transform.rotation = rotateByQuat * transform.rotation;


		float currentSpeed = NormalSpeed;
		// Movement
		if (vAxis < 0)
		{
			currentSpeed = Mathf.Lerp(MinSpeed, NormalSpeed,  1.0f + vAxis);
		}
		else if (vAxis > 0)
		{
			currentSpeed = Mathf.Lerp(NormalSpeed, MaxSpeed,  vAxis);
		}

		transform.position += Direction * currentSpeed * Time.deltaTime;

		Direction = newDirection;

		PlayerDetailsComp.CollectWaterFX.enableEmission = Altitude < 0.01f && !PlayerDetailsComp.IsOverLand;

		if (TargetAltitude == Altitude)
		{
			TargetAltitude = pressedAltSink > 0 ? 0.0f : TargetAltitude;
			TargetAltitude = pressedAltClimb > 0 ? 1.0f : TargetAltitude;

			if (Altitude <= 0.01f)
			{
				if (PlayerDetailsComp.IsOverLand)
				{
					Die();
				}
				else
				{
					int AmmoToAdd = (int)Mathf.Floor((float)PlayerDetailsComp.AmmoRefillPerSecond * Time.deltaTime);
					PlayerDetailsComp.CurrentAmmo = Mathf.Clamp(PlayerDetailsComp.CurrentAmmo + AmmoToAdd, 0, PlayerDetailsComp.MaxAmmo);
				}
			}
		}
		else
		{
			float diff = Mathf.Sign(TargetAltitude - Altitude) * AltitudeChangeSpeed * Time.deltaTime;
			Altitude = Mathf.Clamp(Altitude + diff, 0.0f, 1.0f);

			transform.localScale = new Vector3(Mathf.Lerp(OriginalScale.x * 0.5f, OriginalScale.x * 1.0f, Altitude), 
			                                   Mathf.Lerp(OriginalScale.y * 0.5f, OriginalScale.y * 1.0f, Altitude), 
			                                   OriginalScale.z * 1.0f);
		}

		TimeSinceLastWaterCollisionCheck += Time.deltaTime;

		if (Altitude >= 0.99f && Altitude == TargetAltitude)
		{
			// Relase Water
			bool releaseWater = Input.GetAxis(ReleaseWaterName)>0 && PlayerDetailsComp.CurrentAmmo > 0;
			
			PlayerDetailsComp.DropWaterFX.enableEmission = releaseWater;
			
			if (releaseWater)
			{
				int AmmoToRemove = (int)Mathf.Floor((float)PlayerDetailsComp.AmmoUsagePerSecond * Time.deltaTime);
				PlayerDetailsComp.CurrentAmmo = Mathf.Clamp(PlayerDetailsComp.CurrentAmmo - AmmoToRemove, 0, PlayerDetailsComp.MaxAmmo);

				if (TimeSinceLastWaterCollisionCheck >= WaterCollisionCheckInterval)
				{
					TimeSinceLastWaterCollisionCheck = 0.0f;
					WaterCollisionCheckList.Add(transform.position + newDirection * currentSpeed * DropWaterInheritVelocity * PlayerDetailsComp.TimeForWaterToDrop);
					Invoke("CheckWaterCollision", PlayerDetailsComp.TimeForWaterToDrop);
				}
			}
		}

		/*if (releaseWater > 0 && !PlayerDetailsComp.DropWaterFX.enableEmission)
		{
			PlayerDetailsComp.DropWaterFX.enableEmission = true;
		}
		else if (releaseWater <= 0 && PlayerDetailsComp.DropWaterFX.enableEmission)
		{
			PlayerDetailsComp.DropWaterFX.enableEmission = false;
		}*/
	}
}
