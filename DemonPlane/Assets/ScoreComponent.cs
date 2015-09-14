using UnityEngine;
using System.Collections;

public class ScoreComponent : MonoBehaviour
{
    public int KilledDemonBonus = 1000;
    public int SurvivedWaveBonus = 5000;
    public int BurningFireMalus = 100;

    public int Score;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void DemonKilled()
    {
        Score += KilledDemonBonus;
    }
    
    void WaveSurvived()
    {
        Score += SurvivedWaveBonus;
    }

    public int GetCurrentMalus()
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
        return NumBurning * BurningFireMalus;
    }

    public int GetFinalScore()
    {
        return Score - GetCurrentMalus();
    }
}
