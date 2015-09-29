﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDetails : MonoBehaviour {

	public int MaxAmmo;
	public int AmmoRefillPerSecond;
	public int AmmoUsagePerSecond;

	public int CurrentAmmo;
	public Vector2 FlightVelocity;
	private bool bOverLand;
	private bool bDied;
	private int Malus;
	private int NumPieplSaved;

	public Text GameOverText;
	public Text AmmoDisplay;
    public Text ScoreDisplay;
	public Text SavedPieplDisplay;
	public Text FinalScoreDisplay;
	public Text FinalScoreDemons;
	public Text FinalScoreWaves;
	public Text FinalForestFire;
	public Text FinalPieplSaved;
	public Text FinalScoreTotal;
	public Text FinalScoreRank;

	private ScoreComponent ScoreComp; 
	private AudioSource SFXPieplCollected;

	public ParticleSystem CollectWaterFX;
	//private GameObject Piepl;

	//dieinn
	private SpriteRenderer rend; 
	Transform spawnTransform;

	public bool IsOverLand { get { return bOverLand; } }
	public bool IsDead { get { return bDied; } }


	// Use this for initialization
	void Start () 
	{
		CurrentAmmo = MaxAmmo;
		NumPieplSaved = 0; 
		AudioSource[] audios = GetComponents<AudioSource> ();
		SFXPieplCollected = audios [5];

		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		SavedPieplDisplay.text = NumPieplSaved.ToString ();

		FinalScoreDisplay.text="The Demons destroyed the Village";
		FinalScoreDisplay.enabled = false;
		FinalScoreDemons.enabled = false;
		FinalScoreWaves.enabled = false;
		FinalPieplSaved.enabled =false;
		FinalForestFire.enabled = false;
		FinalScoreTotal.enabled = false;
		FinalScoreRank.enabled = false;
		ScoreComp = GetComponent<ScoreComponent>(); 
		rend = gameObject.GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		SavedPieplDisplay.text = NumPieplSaved.ToString ();
        //ScoreDisplay.text = "Score: " + GetComponent<ScoreComponent>().Score.ToString();

        Malus = GetComponent<ScoreComponent>().GetCurrentMalus();
        if (Malus > 0)
        {
            ScoreDisplay.text += "-" + Malus;
        }
    }

	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "land")
		{
			bOverLand = true;
		}
		if (collision.gameObject.tag == "piepl" && GetComponent<ControllerScript> ().Altitude <= 0.01f) {

			CollectPiepl();

			GameObject.Destroy (collision.gameObject);

		}

	}

	void OnTriggerExit2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "land")
		{
			bOverLand = false;

		}
	}

	public void Die()
	{
		if(bDied) return;

		bDied = true;
		//turn off rendering of the plane - TO DO: hide the drop shadow too
		rend.enabled = false;
		spawnTransform = transform;
		//stop airplane sound
		GetComponent<AudioSource> ().Stop ();

		//spawn a broken plane
		GameObject.Instantiate (Resources.Load ("DeadPlayer"), spawnTransform.position, spawnTransform.rotation);

		int DemonScore = ScoreComp.NumDemonsKilled * ScoreComp.KilledDemonBonus;
		int PieplScore = NumPieplSaved * 500;
		int WaveScore = ScoreComp.NumWavesSurvived * ScoreComp.SurvivedWaveBonus;
		int FireMalus = ScoreComp.NumBurningFires * ScoreComp.BurningFireMalus*(-1);
		int FinalScore = DemonScore + WaveScore + WaveScore;

		FinalScoreDemons.text = ScoreComp.NumDemonsKilled.ToString () + FinalScoreDemons.text + DemonScore.ToString ();
		FinalPieplSaved.text = NumPieplSaved.ToString () + FinalPieplSaved.text + PieplScore.ToString ();
		FinalScoreWaves.text = ScoreComp.NumWavesSurvived.ToString () + FinalScoreWaves.text + WaveScore.ToString ();
		FinalForestFire.text = ScoreComp.NumBurningFires.ToString () + FinalForestFire.text +  FireMalus.ToString ();
		FinalScoreTotal.text = FinalScoreTotal.text+" " + FinalScore.ToString ();
		FinalScoreRank.text = FinalScoreRank.text + CalculateRank (FinalScore);

		FinalScoreDisplay.enabled = true;
		FinalScoreDemons.enabled = true;
		FinalScoreWaves.enabled = true;
		FinalPieplSaved.enabled =true;
		FinalForestFire.enabled = true;
		FinalScoreTotal.enabled = true;
		FinalScoreRank.enabled = true;

		Invoke("RestartLevel", 10.0f);
	}
	void CalculateFinalScore()
	{

	}

	
	void RestartLevel()
	{
		Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}

	string CalculateRank(int TotalScore)
	{	
		if (TotalScore > 50000) {

			return "AMERICAN EAGLE";
		} 
		else if (TotalScore > 20000) {

			return "SWANE";
		}
		else if (TotalScore > 15000) {
			
			return "DUCK";
		}
		else if (TotalScore > 6000) {
			
			return "MOORHEN";
		}
		else if (TotalScore > 1000) {
			
			return "FROG";
		}
		else {
			
			return "POLLYWOG";
		}


	}
	void CollectPiepl()
	{
		//collect a person in water
		++NumPieplSaved;
		SFXPieplCollected.Play ();

	}

	
}
