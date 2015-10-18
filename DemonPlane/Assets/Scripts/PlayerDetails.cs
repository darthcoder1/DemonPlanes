using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameJolt;

public class PlayerDetails : MonoBehaviour {
	
	public int MaxAmmo;
	public int AmmoRefillPerSecond;
	public int AmmoUsagePerSecond;
	
	//bonus stuff
	public int BonusMaxAmmo;
	public int BonusMaxSpeed;
	public int BonusShootingRange;
	public float BonusDuration;
	
	public int CurrentAmmo;
	public Vector2 FlightVelocity;
	private bool bOverLand;
	private bool bDied;
	private int Malus;
	private int NumPieplSaved;
	
	public Text GameOverText;
	public Text AmmoDisplay;
	
	//public Text SavedPieplDisplay;
	public Text FinalScoreDisplay;
	public Text FinalScoreDemons;
	public Text FinalScoreWaves;
	public Text FinalForestFire;
	public Text FinalPieplSaved;
	public Text FinalScoreTotal;
	public Text FinalScoreRank;
	
	public Text Bonus;
	
	//Bonus stuff
	private Text SpeedBonusText;
	private Text MaxAmmoBonusText;
	private Text MaxShootingRangeText;
	
	private int NumSpeedBonus;
	private int NumMaxAmmoBonus;
	private int NumMaxShootingRange;
	
	private Text BonusChanceText;
	private Image BonusChanceImage;
	private Animator BonusChanceAnimation;
	
	private Text BonusDetailText;
	
	private ScoreComponent ScoreComp; 
	private AudioSource SFXPieplCollected;
	
	public ParticleSystem CollectWaterFX;
	//private GameObject Piepl;
	
	private bool WaitingForRestart;
	private bool WaitingForLeaderboard;
	
	//dieinn
	private SpriteRenderer rend; 
	Transform spawnTransform;
	
	public bool IsOverLand { get { return bOverLand; } }
	public bool IsDead { get { return bDied; } }
	
	private AudioSource[] audios;
	
	// Use this for initialization
	void Start () 
	{
		//WaitingForRestart = false;
		WaitingForLeaderboard = false;
		WaitingForRestart = false;
		
		CurrentAmmo = MaxAmmo;
		NumPieplSaved = 0; 
		audios = GetComponents<AudioSource> ();
		SFXPieplCollected = audios [5];
		
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		//SavedPieplDisplay.text = NumPieplSaved.ToString ();
		
		//Bonus Texts
		//		Bonus.enabled = false;
		
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
		
		SpeedBonusText=GameObject.Find("MaxSpeedText").GetComponent<Text>();
		MaxAmmoBonusText=GameObject.Find("MaxAmmoText").GetComponent<Text>();
		MaxShootingRangeText=GameObject.Find("MaxShootingRange").GetComponent<Text>();
		
		GameObject.Find("FinalScore").GetComponent<Image>().enabled=false;
		GameObject.Find("FS_Demons_IMG").GetComponent<Image>().enabled=false;
		GameObject.Find("FS_Piggies_IMG").GetComponent<Image>().enabled=false;
		
		GameObject.Find("BonusChance_text").GetComponent<Text>().enabled=false;
		
		
		BonusChanceText = GameObject.Find ("BonusChance_text").GetComponent<Text> ();
		BonusChanceImage = GameObject.Find ("BonusChancePig").GetComponent<Image> ();
		BonusChanceAnimation = GameObject.Find ("BonusChancePig").GetComponent<Animator> ();
		
		BonusDetailText=GameObject.Find ("RarePigText").GetComponent<Text> ();
		BonusDetailText.enabled = false;
		
		/*
		BonusChanceHasty = GameObject.Find ("BonusChanceHasty").GetComponent<Image> ();
		BonusChanceHasty
			BonusChanceWhale
				BonusChanceSniper
				*/
		
		
		
		BonusChanceText.enabled = false;
		BonusChanceImage.enabled = false;
		BonusChanceAnimation.enabled = false;
		
		NumSpeedBonus=0;
		NumMaxAmmoBonus=0;
		NumMaxShootingRange=0;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (WaitingForLeaderboard)
		{
			if (Input.GetButton("AltClimb") || Input.GetButton("ReleaseWater") || Input.GetButton("Shoot"))
			{
				WaitingForLeaderboard = false;

                if (GlobalSettings.Instance.UseGameJolt)
                {
                    GameJolt.UI.Manager.Instance.ShowLeaderboards((bool success) =>
                                                                  {
                                                                      WaitingForLeaderboard = false;
                                                                      WaitingForRestart = true;
                                                                  });
                }
                else
                {
                    RestartLevel();
                }
			}
			return;
		}
		if (WaitingForRestart)
		{
			RestartLevel();
			return;
		}
		
		
		if (AmmoDisplay)
		{
			AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		}
		//SavedPieplDisplay.text = NumPieplSaved.ToString ();
		if (SpeedBonusText)
		{
			SpeedBonusText.text = "x" + NumSpeedBonus.ToString();
		}
		if (MaxAmmoBonusText)
		{
			MaxAmmoBonusText.text = "x" + NumMaxAmmoBonus.ToString();
		}
		if (MaxShootingRangeText)
		{
			MaxShootingRangeText.text = "x" + NumMaxShootingRange.ToString();
		}
		//ScoreDisplay.text = "Score: " + GetComponent<ScoreComponent>().Score.ToString();
		
		/*
		Malus = GetComponent<ScoreComponent>().GetCurrentMalus();
        if (Malus > 0)
        {
            ScoreDisplay.text += "-" + Malus;
        }*/
	}
	
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "land")
		{
			bOverLand = true;
		}
		if (collision.gameObject.tag == "piepl" && GetComponent<ControllerScript> ().Altitude <= 0.01f) {
			
			PieplBehavior piggie =collision.gameObject.GetComponent<PieplBehavior>();
			CollectPiepl(piggie.isSpecial,piggie.PigName);
			
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
		if (ScoreComp.bInvincibleBoost)
			return;
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
		//int FireMalus = ScoreComp.NumBurningFires * ScoreComp.BurningFireMalus*(-1);
		int FinalScore = DemonScore + PieplScore + WaveScore;
		
		if (GlobalSettings.Instance.UseGameJolt)
		{
			if (GameJolt.API.Manager.Instance.CurrentUser != null)
			{
				GameJolt.API.Objects.Score score = new GameJolt.API.Objects.Score(FinalScore, FinalScore.ToString());
				GameJolt.API.Scores.Add(score);
			}
			else
			{
				GameJolt.API.Scores.Add(FinalScore, string.Format("{0} points", FinalScore), "Guest");
			}
		}
		
		FinalScoreDemons.text = "  x" + ScoreComp.NumDemonsKilled.ToString () +  " = "+DemonScore.ToString ();
		FinalPieplSaved.text = "    x" + NumPieplSaved.ToString () + " = "+ PieplScore.ToString ();
		FinalScoreWaves.text = "Waves x" +ScoreComp.NumWavesSurvived.ToString () + " = "+ WaveScore.ToString ();
		//FinalForestFire.text = ScoreComp.NumBurningFires.ToString () + FinalForestFire.text +  FireMalus.ToString ();
		FinalScoreTotal.text = FinalScoreTotal.text+" " + FinalScore.ToString ();
		FinalScoreRank.text = FinalScoreRank.text + CalculateRank (FinalScore);
		
		FinalScoreDisplay.enabled = true;
		FinalScoreDemons.enabled = true;
		FinalScoreWaves.enabled = true;
		FinalPieplSaved.enabled =true;
		FinalForestFire.enabled = true;
		FinalScoreTotal.enabled = true;
		FinalScoreRank.enabled = true;
		GameObject.Find("FinalScore").GetComponent<Image>().enabled=true;
		GameObject.Find("FS_Demons_IMG").GetComponent<Image>().enabled=true;
		GameObject.Find("FS_Piggies_IMG").GetComponent<Image>().enabled=true;
		
		WaitingForLeaderboard = true;
	}
	
	
	
	void RestartLevel()
	{
		WaitingForLeaderboard = false;
		WaitingForRestart = false;
		Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}
	
	string CalculateRank(int TotalScore)
	{	
		if (TotalScore > 150000) {
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Rank_AmericanEagle);
			return "AMERICAN EAGLE";
		} 
		if (TotalScore > 100000) {
			
			return "ALBATROSS";
		} 
		if (TotalScore > 80000) {
			
			return "FLAMINGO";
		} 
		
		else if (TotalScore > 70000) {
			
			return "SWANE";
		}
		else if (TotalScore > 60000) {
			
			return "STORK";
		}
		else if (TotalScore > 50000) {
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Rank_KingFisher);
			return "KINGFISHER";
		}
		else if (TotalScore > 45000) {
			
			return "CORMORAN";
		}
		
		else if (TotalScore > 40000) {
			
			return "LOON";
		}
		else if (TotalScore > 35000) {
			
			return "SHOREBIRD";
		}
		else if (TotalScore > 30000) {
			
			return "SEAGULL";
		}
		else if (TotalScore > 25000) {
			
			return "DUCK";
		}
		else if (TotalScore > 20000) {
			
			return "MOORHEN";
		}
		else if (TotalScore > 15000) {
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Rank_DragonFly);
			return "DRAGONFLY";
		}
		else if (TotalScore > 1000) {
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Rank_Frog);
			return "FROG";
		}
		else {
			
			return "POLLYWOG";
		}
		
		
	}
	void CollectPiepl(bool isSpecial, string nPigName)
	{
		//collect a person in water
		NumPieplSaved++;
		SendMessage("PiggieSaved");
		
		if (NumPieplSaved >= 50)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_50);
		}
		else if (NumPieplSaved >= 20)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_20);
		}
		else if (NumPieplSaved >= 5)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_5);
		}
		else if (NumPieplSaved >= 1)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_1);
		}
		
		SFXPieplCollected.Play ();
		//CALCULATE WHICH BONUS IF PIGGIE IS SPECIAL
		if(nPigName != "default")
		{
			audios [6].Play ();
			
			if(nPigName =="rainbow")
			{
				BonusChanceText.text = "°°RAINBOW PIG°°";
				BonusChanceImage.enabled = false;
				BonusChanceImage=GameObject.Find ("RarePigRainbow").GetComponent<Image> ();
				BonusChanceText.enabled = true;
				BonusChanceImage.enabled = true;
				BonusDetailText.text="°°DOUBLE SCORE FOR 30 SECONDS°°";
				BonusDetailText.enabled=true;
				SendMessage("TurnOnScoreBooster");
				
				AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_Rainbow);
			}
			if(nPigName =="dark")
			{
				BonusChanceText.text = "°°DARK PIG°°";
				BonusChanceImage.enabled = false;
				BonusChanceImage=GameObject.Find ("RarePigDark").GetComponent<Image> ();
				BonusChanceText.enabled = true;
				BonusChanceImage.enabled = true;
				BonusDetailText.text="°°DOUBLE DAMAGE FOR 30 SECONDS°°";
				BonusDetailText.enabled=true;
				SendMessage("TurnOnDamageBooster");
				AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_Dark);
			}
			if(nPigName =="zombie")
			{
				BonusChanceText.text = "°°ZOMBIE PIG°°";
				BonusChanceImage.enabled = false;
				BonusChanceImage=GameObject.Find ("RarePigZombie").GetComponent<Image> ();
				BonusChanceText.enabled = true;
				BonusChanceImage.enabled = true;
				BonusDetailText.text="°°INVINCIBLE FOR 20 SECONDS°°";
				BonusDetailText.enabled=true;
				SendMessage("TurnOnInvincibleBooster");
				AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_SaveThePig_Zombie);
			}
			Invoke ("EndBonusDisplay", 5);
		}
		else if (isSpecial) {
			audios [6].Play ();
			BonusChanceText.text = "°°SPECIAL PIG°°";
			
			BonusChanceText.enabled = true;
			BonusChanceImage.enabled = true;
			BonusChanceAnimation.enabled = true;
			Invoke ("RollDieWhichBonus", 2);
			
		} 
		
		
	}
	void RollDieWhichBonus()
	{
		int RandomBonus= Random.Range(0,3);
		//GameObject.Find ("BonusChancePig").GetComponent<AudioSource>().Play ();
		print ("BONUS: " + RandomBonus.ToString ());
		switch(RandomBonus){
			
		case 0: StartTriggerBonusAmmo();
			break;
			
		case 1: StartTriggerBonusSpeed();
			break;
			
		case 2: StartTriggerBonusShootingRange();
			break;
		}
		//BonusChanceText.enabled = false;
		//BonusChanceImage.enabled = false;
		//BonusChanceAnimation.enabled = false;
	}
	
	//Bonus stuff
	//BONUS MAX AMMO
	void StartTriggerBonusAmmo()
	{
		MaxAmmo += BonusMaxAmmo;
		//Bonus Texts
		BonusChanceImage.enabled = false;
		BonusChanceText.text="°°WHALE PIG°°";
		BonusChanceImage=GameObject.Find ("BonusChanceWhale").GetComponent<Image> ();
		//GameObject.Find ("BonusChancePig").GetComponent<AudioSource>().Stop();
		//GameObject.Find ("BonusChanceHasty").GetComponent<AudioSource>().Play ();
		BonusChanceImage.enabled = true;
		
		BonusDetailText.text="°°STORE MORE WATER °°";
		BonusDetailText.enabled = true;
		
		NumMaxAmmoBonus++;
		
		if (NumMaxAmmoBonus >= 5)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Whale_5);
		}
		else if (NumMaxAmmoBonus >= 3)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Whale_3);
		}
		else if (NumMaxAmmoBonus >= 2)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Whale_2);
		}
		else if (NumMaxAmmoBonus >= 1)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Whale_1);
		}
		Invoke ("EndBonusDisplay", 5);
		//Invoke ("EndTriggerBonusAmmo", BonusDuration);
	}
	void EndTriggerBonusAmmo()
	{
		MaxAmmo -= BonusMaxAmmo;
		NumMaxAmmoBonus--;
	}
	//BONUS MAX SPEED
	void StartTriggerBonusSpeed()
	{
		//Bonus Texts
		BonusChanceImage.enabled = false;
		BonusChanceText.text = "°°HASTY PIG°°";
		BonusChanceImage=GameObject.Find ("BonusChanceHasty").GetComponent<Image> ();
		//GameObject.Find ("BonusChancePig").GetComponent<AudioSource>().Stop();
		//GameObject.Find ("BonusChanceHasty").GetComponent<AudioSource>().Play ();
		BonusChanceImage.enabled = true;
		BonusDetailText.text="°°FLY FASTER°°";
		BonusDetailText.enabled = true;
		NumSpeedBonus++;
		
		if (NumSpeedBonus >= 5)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Hasty_5);
		}
		else if (NumSpeedBonus >= 3)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Hasty_3);
		}
		else if (NumSpeedBonus >= 2)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Hasty_2);
		}
		else if (NumSpeedBonus >= 1)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Hasty_1);
		}
		
		GetComponent<ControllerScript>().MaxSpeed += BonusMaxSpeed;
		Invoke ("EndBonusDisplay", 5);
		//Invoke ("EndTriggerBonusSpeed", BonusDuration);
	}
	void EndTriggerBonusSpeed()
	{
		Bonus.enabled = false;
		NumSpeedBonus--;
		GetComponent<ControllerScript>().MaxSpeed -= BonusMaxSpeed;
		
	}
	//BONUS MAX SHOOTING RANGE
	void StartTriggerBonusShootingRange()
	{
		//Bonus Texts
		BonusChanceImage.enabled = false;
		BonusChanceText.text = "°°SNIPER PIG°°";
		BonusChanceImage=GameObject.Find ("BonusChanceSniper").GetComponent<Image> ();
		//GameObject.Find ("BonusChancePig").GetComponent<AudioSource>().Stop();
		//GameObject.Find ("BonusChanceHasty").GetComponent<AudioSource>().Play ();
		BonusDetailText.text="°°SHOOT FURTHER°°";
		BonusDetailText.enabled = true;
		BonusChanceImage.enabled = true;
		
		NumMaxShootingRange++;
		
		if (NumMaxShootingRange >= 5)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Sniper_5);
		}
		else if (NumMaxShootingRange >= 3)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Sniper_3);
		}
		else if (NumMaxShootingRange >= 2)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Sniper_2);
		}
		else if (NumMaxShootingRange >= 1)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_Skill_Sniper_1);
		}
		
		GetComponent<DropWater>().WaterBulletSpeed += BonusShootingRange;
		Invoke ("EndBonusDisplay", 5);
		//Invoke ("EndTriggerBonusShootingRange", BonusDuration);
	}
	void EndTriggerBonusShootingRange()
	{
		Bonus.enabled = false;
		NumMaxShootingRange--;
		GetComponent<DropWater>().WaterBulletSpeed -= BonusShootingRange;
		
	}
	void EndBonusDisplay()
	{
		//BonusChanceImage.sprite=Resources.Load("shooting_range") as Sprite;
		print (BonusChanceText.text);
		
		BonusChanceImage.enabled = false;
		BonusChanceText.enabled = false;
		BonusDetailText.enabled=false;
		BonusChanceImage = GameObject.Find ("BonusChancePig").GetComponent<Image> ();
		
	}
	
	
	
	
}
