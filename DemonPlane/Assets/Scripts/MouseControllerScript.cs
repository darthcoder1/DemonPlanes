using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MouseControllerScript : MonoBehaviour 
{
	public float NormalSpeed;			// Normal flight speed, when stick is neither pulled nor pushed)
	public float RotationSpeed;			// Rotation speed of the airplane
	public float AltitudeChangeSpeed;	// Specifies how fast altitude is changed
	public float WaterCollisionCheckInterval; // Specifies the interval in which the dropped water is checking for collision
	public float DropWaterInheritVelocity; // This must match the value from the particlesystem

	public Button AltitudeChangeBtn;
	private float Altitude; // 0.0 == ground, 1.0f == normal flight height
	private float TargetAltitude;
	private Vector3 OriginalScale;

	private PlayerDetails PlayerDetailsComp;
	private DropWater DropWaterComp;

	// Use this for initialization
	void Start () 
	{
		PlayerDetailsComp = GetComponent<PlayerDetails> ();
		DropWaterComp = GetComponent<DropWater> ();

		Altitude = TargetAltitude = 1.0f;
		OriginalScale = transform.localScale;
	}

	public void AltitudeButtonClicked()
	{
		AltitudeChangeBtn.interactable = false;
		TargetAltitude = Altitude >= 0.99f ? 0.0f : 1.0f;
	}

	void UpdateRotation()
	{
		if (Input.GetMouseButton (0)) 
		{
			Vector2 playerPos = transform.position;
			Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			
			Vector2 targetDir = (cursorPos - playerPos).normalized;
			Vector2 currentDir = transform.rotation * Vector3.up;
			Vector2 currentRightVec = new Vector2(currentDir.y, -currentDir.x);

			Debug.DrawLine (playerPos, playerPos + currentDir * 5.0f, Color.blue);
			Debug.DrawLine (playerPos, playerPos + targetDir * 5.0f, Color.green);
			Debug.DrawLine (playerPos, playerPos + currentRightVec * 5.0f, Color.yellow);

			float rotationAngle = RotationSpeed * Time.deltaTime;
			float angle = Vector2.Angle (currentDir, targetDir);

			if (Vector2.Dot (currentRightVec, targetDir) > 0)
			{
				rotationAngle *= -1.0f;
			}

			transform.Rotate (new Vector3 (0.0f, 0.0f, Mathf.Abs(angle) > rotationAngle ? rotationAngle : angle));
		}
		PlayerDetailsComp.FlightVelocity = (transform.rotation * Vector3.up) * NormalSpeed;
	}

	void UpdateSpeed()
	{
		Vector3 flightVel = PlayerDetailsComp.FlightVelocity;
		transform.position += flightVel * Time.deltaTime;
	}

	void UpdateWaterDrop()
	{
		bool bShouldDrop = Input.GetMouseButton (1);
		if (!DropWaterComp.DroppingWater && bShouldDrop) 
		{
			DropWaterComp.DropWaterStart();
		}
		else if (DropWaterComp.DroppingWater && !bShouldDrop)
		{
			DropWaterComp.DropWaterStop();
		}
	}

	void UpdateAltitudeChange()
	{
		if (TargetAltitude == Altitude)
		{
			AltitudeChangeBtn.interactable = true;

			if (Altitude <= 0.01f)
			{
				if (PlayerDetailsComp.IsOverLand)
				{
					PlayerDetailsComp.Die();
				}
				else
				{
					PlayerDetailsComp.CollectWaterFX.enableEmission = PlayerDetailsComp.CurrentAmmo < PlayerDetailsComp.MaxAmmo;

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
	}

	private bool ClickedOnUI()
	{
		UnityEngine.EventSystems.EventSystem ct
			= UnityEngine.EventSystems.EventSystem.current;
		
		if (! ct.IsPointerOverGameObject ()) 
		{
			return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () 
	{
		if (PlayerDetailsComp.IsDead) 
		{
			return;
		}

		if (!ClickedOnUI())
		{
			UpdateRotation ();
		}

		UpdateSpeed ();
		UpdateWaterDrop ();
		UpdateAltitudeChange ();
	}
}
