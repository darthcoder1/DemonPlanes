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
		ScoreBoostTimer = 0f;

		BoosterScore=GameObject.Find("BoosterScore").GetComponent<Text>();
		BoosterScoreImage=GameObject.Find("BoosterScoreImage").GetComponent<Image>();

		BoosterScore.enabled = false;
		BoosterScoreImage.enabled = false;

	}
	
	// Update is called once per frame
	void Update ()
    {
		ScoreDisplay.text =  Score.ToString();
		BoosterScore.text = ScoreBoostTimer.ToString()+" s";
	}

    void DemonKilled()
    {
		NumDemonsKilled++;
		//Score += KilledDemonBonus;
		AddScore (KilledDemonBonus);
		print ("Demon kill gets called");
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
		bScoreBoost = true;
		ScoreBoostTimer += 30f;
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

}
