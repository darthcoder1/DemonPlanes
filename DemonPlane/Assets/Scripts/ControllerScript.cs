using UnityEngine;
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
	private bool bDied;

	public float Altitude; // 0.0 == ground, 1.0f == normal flight height
	private float TargetAltitude;
	private Vector3 OriginalScale;
	public  Vector3 Direction;
	public float currentSpeed;

	private PlayerDetails PlayerDetailsComp;
	private DropWater DropWaterComp;
	public Text GameOverText;

	//dieinn
	private SpriteRenderer rend; 
	Transform spawnTransform;

    private GameObject DropShadowObject;



	private readonly string HorizontalAxixName;
	private readonly string VerticalAxixName;
	private readonly string ClimbButtonName;
	private readonly string SinkButtonName;
	private readonly string ReleaseWaterName;
	private readonly string StrafeLeftName;
	private readonly string StrafeRightName;
	private readonly string ShootButtonName;
	private readonly string StartButtonName;

	private Image HowToPlay;
	private bool bGamePaused;

	ControllerScript()
	{
		HorizontalAxixName = "Horizontal";
		VerticalAxixName = "Vertical";
		ClimbButtonName = "AltClimb";
		SinkButtonName = "AltSink";
		ReleaseWaterName = "ReleaseWater";
		StrafeLeftName = "StrafeLeft";
		StrafeRightName = "StrafeRight";
		ShootButtonName = "Shoot";
		StartButtonName = "Start";
	}

	// Use this for initialization
	void Start () 
	{
		Altitude = TargetAltitude = 1.0f;
		Direction = new Vector3 (0, 1, 0);
		bDied = false;
		bGamePaused = false;
		HowToPlay=GameObject.Find ("HowToPlay").GetComponent<Image> ();
		HowToPlay.enabled = false;
		DropWaterComp = GetComponent<DropWater> ();
		PlayerDetailsComp = GetComponent<PlayerDetails>();
		GameOverText.text = "";

        DropShadowObject = transform.GetChild(0).gameObject;
		OriginalScale = transform.localScale;
		rend = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void Die()
	{
		bDied = true;
		GetComponent<PlayerDetails> ().Die ();

	}

    void Win()
    {
        bDied = true;
        GameOverText.text = "You won!";
        GameOverText.enabled = true;
        Invoke("RestartLevel", 10.0f);
    }

    void RestartLevel()
	{
		GameObject.Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}

	void UpdateWaterDrop()
	{
		bool bShouldDrop = Input.GetAxis (ReleaseWaterName) > 0.3f;
		if (!DropWaterComp.DroppingWater && bShouldDrop) 
		{
			DropWaterComp.DropWaterStart();
		}
		else if (DropWaterComp.DroppingWater && !bShouldDrop)
		{
			DropWaterComp.DropWaterStop();
		}
	}
	void UpdateShoot()
	{
		bool bShouldShoot = Input.GetAxis (ShootButtonName) > 0f;
		if (!DropWaterComp.DroppingWater && bShouldShoot) 
		{
			DropWaterComp.ShootWater();
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

		bool pressedStart = Input.GetButton (StartButtonName);

		/*
		float pressedStrafeLeft = Input.GetAxis (StrafeLeftName);
		float pressedStrafeRight = Input.GetAxis (StrafeRightName);

		float pressedShoot = Input.GetAxis (ShootButtonName);


		if (pressedStart) {
			if(bGamePaused)
			{
				Time.timeScale = 1.0f;
				Time.fixedDeltaTime = 1.0f;
				Invoke("ResumeGame",0.1f);
			}
			else Invoke("PauseGame",0.1f);

		}*/

		// Rotation
		//float rotateBy = RotationSpeed * Time.deltaTime * hAxis;
		float rotateBy= RotationSpeed * Time.deltaTime * hAxis;

		Quaternion rotateByQuat = Quaternion.AngleAxis (rotateBy, Vector3.back);
		Vector3 newDirection = rotateByQuat * Direction;

		Vector3 position = transform.position;
		Debug.DrawLine( position, position + Direction * 100, Color.red );

		transform.rotation = rotateByQuat * transform.rotation;


		currentSpeed = NormalSpeed;

		// Movement
		if (vAxis < 0)
		{
			currentSpeed = Mathf.Lerp(MinSpeed, NormalSpeed,  1.0f + vAxis);
		}
		else if (vAxis > 0)
		{
			currentSpeed = Mathf.Lerp(NormalSpeed, MaxSpeed,  vAxis);
		}
		GetComponent<AudioSource> ().pitch =currentSpeed /10;

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
					GetComponent<PlayerDetails> ().FinalScoreDisplay.text="You Crashed!";
					Die();
				}
				else
				{
					int AmmoToAdd = (int)Mathf.Floor((float)PlayerDetailsComp.AmmoRefillPerSecond * Time.deltaTime);
					PlayerDetailsComp.CurrentAmmo = Mathf.Clamp(PlayerDetailsComp.CurrentAmmo + AmmoToAdd, 0, PlayerDetailsComp.MaxAmmo);


				}
			}
			//only shjoot or drop water when high up
			else{
				UpdateWaterDrop ();
				UpdateShoot ();
			}
		}
		else
		{
			float diff = Mathf.Sign(TargetAltitude - Altitude) * AltitudeChangeSpeed * Time.deltaTime;
			Altitude = Mathf.Clamp(Altitude + diff, 0.0f, 1.0f);

			transform.localScale = new Vector3(Mathf.Lerp(OriginalScale.x * 0.5f, OriginalScale.x * 1.0f, Altitude), 
			                                   Mathf.Lerp(OriginalScale.y * 0.5f, OriginalScale.y * 1.0f, Altitude), 
			                                   OriginalScale.z * 1.0f);

            DropShadowObject.transform.localScale = Vector3.Lerp(new Vector3(0.5f, 0.5f, 0.5f),
                                                                 new Vector3(0.75f, 0.75f, 0.75f), 1.0f-Altitude);
            DropShadowObject.transform.localPosition = Vector3.Lerp(new Vector3(1.0f, 1.0f, 1.0f),
                                                                    new Vector3(0.5f, 0.5f, 0.5f), 1.0f-Altitude);
        }

	
	}
	void PauseGame()
	{
		Time.timeScale = 0.0f;
		Time.fixedDeltaTime = 0.0f;
		bGamePaused = true;
		HowToPlay.enabled = true;

	}
	void ResumeGame()
	{

		bGamePaused = false;
		HowToPlay.enabled = false;
	}


}
