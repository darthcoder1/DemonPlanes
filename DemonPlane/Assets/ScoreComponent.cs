using UnityEngine;
using System.Collections;

public class ScoreComponent : MonoBehaviour
{
    public int KilledDemonBonus = 1000;
    public int SurvivedWaveBonus = 5000;
    public int BurningFireMalus = 100;

	public int NumDemonsKilled;
	public int NumWavesSurvived;

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
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void DemonKilled()
    {
		++NumDemonsKilled;
		Score += KilledDemonBonus;
    }
    
    void WaveSurvived()
    {
		NumWavesSurvived++;
		Score += SurvivedWaveBonus;
    }

    public int GetCurrentMalus()
    {
        return NumBurningFires * BurningFireMalus;
    }

    public int GetFinalScore()
    {
        return Score - GetCurrentMalus();
    }
}
