using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreComponent : MonoBehaviour
{
	public int KilledDemonBonus = 1000;
	public int SurvivedWaveBonus = 500;
	public int PiggieSavedBonus = 500;
	public int BurningFireMalus = 100;
	public Text ScoreDisplay;
	public int NumDemonsKilled;
	public int NumWavesSurvived;
	public int NumPiggiesSaved;
	
	private bool bScoreBoost;
	public float ScoreBoostTimer;
	private Text BoosterScore;
	private Image BoosterScoreImage;
	
	public bool bDamageBoost;
	public float DamageBoostTimer;
	private Text BoosterDamage;
	private Image BoosterDamageImage;
	
	public bool bInvincibleBoost;
	public float InvincibleBoostTimer;
	private Text BoosterInvincible;
	private Image BoosterInvincibleImage;
	
	public int Score;
	
	public int NumBurningFires
	{
		get
		{
			int NumBurning = 0;
			FireCell[] fireCells = GameObject.FindObjectsOfType<FireCell>();
			foreach (FireCell cell in fireCells)
			{
				if (cell.IsBurning)
				{
					++NumBurning;
				}
			}
			return NumBurning;
		}
	}
	
	
	// Use this for initialization
	void Start ()
	{
		NumDemonsKilled = 0;
		NumWavesSurvived = 0;
		NumPiggiesSaved = 0;
		bScoreBoost = false;
		bInvincibleBoost = false;
		bDamageBoost = false;

		ScoreBoostTimer = 0f;
		DamageBoostTimer = 0;
		InvincibleBoostTimer = 0;
		
		BoosterScore=GameObject.Find("BoosterScore").GetComponent<Text>();
		BoosterScoreImage=GameObject.Find("BoosterScoreImage").GetComponent<Image>();
		
		BoosterScore.enabled = false;
		BoosterScoreImage.enabled = false;
		
		BoosterDamage=GameObject.Find("BoosterDamage").GetComponent<Text>();
		BoosterDamageImage=GameObject.Find("BoosterDamageImage").GetComponent<Image>();
		BoosterDamage.enabled = false;
		BoosterDamageImage.enabled = false;

		BoosterInvincible=GameObject.Find("BoosterInvincible").GetComponent<Text>();
		BoosterInvincibleImage=GameObject.Find("BoosterInvincibleImage").GetComponent<Image>();
		BoosterInvincible.enabled = false;
		BoosterInvincibleImage.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		ScoreDisplay.text =  Score.ToString();
		BoosterScore.text = ScoreBoostTimer.ToString()+" s";
		BoosterDamage.text = DamageBoostTimer.ToString()+" s";
		BoosterInvincible.text = InvincibleBoostTimer.ToString()+" s";
	}
	
	void DemonKilled()
	{
		NumDemonsKilled++;
		//Score += KilledDemonBonus;
		AddScore (KilledDemonBonus);
		print ("Demon kill gets called");
		
		if (NumDemonsKilled >= 100)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_KillDemon_100);
		}
		else if (NumDemonsKilled >= 20)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_KillDemon_20);
		}
		else if (NumDemonsKilled >= 5)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_KillDemon_5);
		}
		else if (NumDemonsKilled >= 1)
		{
			AchievmentManager.Instance.UnlockAchievement(AchievmentManager.kAchievement_KillDemon_1);
		}
	}
	
	void WaveSurvived()
	{
		NumWavesSurvived++;
		//Score += SurvivedWaveBonus;
		AddScore (SurvivedWaveBonus*NumWavesSurvived);
		
		print ("WAVE FINISHED BONUS:"+ (SurvivedWaveBonus*NumWavesSurvived).ToString());
	}
	void PiggieSaved()
	{
		NumPiggiesSaved++;
		//Score += SurvivedWaveBonus;
		AddScore (PiggieSavedBonus);
		//print ("WAVE FINISHED gets called");
	}
	
	public int GetCurrentMalus()
	{
		return NumBurningFires * BurningFireMalus;
	}
	
	public int GetFinalScore()
	{
		return Score - GetCurrentMalus();
	}
	void AddScore(int ScoreToAdd)
	{
		int i;
		if(bScoreBoost){ ScoreToAdd*=2;}
		
		for (i=0; i<ScoreToAdd; i++) {
			
			Invoke("AddOnePoint",(i+1)*0.001f);
			
		}
		
	}
	void AddOnePoint()
	{
		Score++;
	}
	//BOOSTER
	//score booster
	void TurnOnScoreBooster()
	{
		ScoreBoostTimer += 30f;
		if (bScoreBoost) {
			return;
		}
		bScoreBoost = true;
		ScoreBoosterCountDown ();
		BoosterScore.enabled = true;
		BoosterScoreImage.enabled = true;
		
	}
	void ScoreBoosterCountDown()
	{
		ScoreBoostTimer -= 1.0f;
		if (ScoreBoostTimer > 0f) {
			Invoke ("ScoreBoosterCountDown", 1);
		} else {
			bScoreBoost = false;
			ScoreBoostTimer=0f;
			BoosterScore.enabled = false;
			BoosterScoreImage.enabled = false;
		}
	}
	//damage booster
	void TurnOnDamageBooster()
	{
		DamageBoostTimer += 30f;
		if (bDamageBoost) {
			return;
		}
		bDamageBoost = true;
		DamageBoosterCountDown ();
		BoosterDamage.enabled = true;
		BoosterDamageImage.enabled = true;
		
	}
	void DamageBoosterCountDown()
	{
		DamageBoostTimer -= 1.0f;
		if (DamageBoostTimer > 0f) {
			Invoke ("DamageBoosterCountDown", 1);
		} else {
			bDamageBoost = false;
			DamageBoostTimer=0f;
			BoosterDamage.enabled = false;
			BoosterDamageImage.enabled = false;
		}
	}
	//invincible booster
	void TurnOnInvincibleBooster()
	{
		InvincibleBoostTimer += 20f;
		if (bInvincibleBoost) {
			return;
		}
		bInvincibleBoost = true;
		InvincibleBoosterCountDown ();
		BoosterInvincible.enabled = true;
		BoosterInvincibleImage.enabled = true;
		
	}
	void InvincibleBoosterCountDown()
	{
		InvincibleBoostTimer -= 1.0f;
		if (InvincibleBoostTimer > 0f) {
			Invoke ("InvincibleBoosterCountDown", 1);
		} else {
			bInvincibleBoost = false;
			InvincibleBoostTimer=0f;
			BoosterInvincible.enabled = false;
			BoosterInvincibleImage.enabled = false;
		}
	}
	
	
}
