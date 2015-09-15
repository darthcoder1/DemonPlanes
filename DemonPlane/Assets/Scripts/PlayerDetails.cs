using UnityEngine;
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

	public Text GameOverText;
	public Text AmmoDisplay;
    public Text ScoreDisplay;
	public Text FinalScoreDisplay;
	public Text FinalScoreDemons;
	public Text FinalScoreWaves;
	public Text FinalScoreTotal;
	public Text FinalScoreRank;

	public ParticleSystem CollectWaterFX;

	//dieinn
	private SpriteRenderer rend; 
	Transform spawnTransform;

	public bool IsOverLand { get { return bOverLand; } }
	public bool IsDead { get { return bDied; } }

	// Use this for initialization
	void Start () 
	{
		CurrentAmmo = MaxAmmo;
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		GameOverText.enabled = false;
		FinalScoreDisplay.enabled = false;
		FinalScoreDemons.enabled = false;
		FinalScoreWaves.enabled = false;
		FinalScoreTotal.enabled = false;
		FinalScoreRank.enabled = false;
		rend = gameObject.GetComponent<SpriteRenderer>();

	}
	
	// Update is called once per frame
	void Update () 
	{
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
        ScoreDisplay.text = "Score: " + GetComponent<ScoreComponent>().Score.ToString();

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
		bDied = true;
		spawnTransform = transform;
		//turn off rendering of the plane - TO DO: hide the drop shadow too
		rend.enabled = false;
		//spawn a broken plane
		GameObject.Instantiate (Resources.Load ("DeadPlayer"), spawnTransform.position, spawnTransform.rotation);

		//GameOverText.enabled = true;
		//GameOverText.text = "Game Over!";
		//show final scores:
		ScoreDisplay.text = "Score: " + GetComponent<ScoreComponent>().Score.ToString();

		FinalScoreDisplay.enabled = true;
		FinalScoreDemons.enabled = true;
		FinalScoreWaves.enabled = true;
		FinalScoreTotal.enabled = true;
		FinalScoreRank.enabled = true;

		Invoke("RestartLevel", 10.0f);
	}
	
	void RestartLevel()
	{
		Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}
	
}
