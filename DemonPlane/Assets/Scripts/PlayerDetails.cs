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

	public Text GameOverText;
	public Text AmmoDisplay;
	public ParticleSystem CollectWaterFX;

	public bool IsOverLand { get { return bOverLand; } }
	public bool IsDead { get { return bDied; } }

	// Use this for initialization
	void Start () 
	{
		CurrentAmmo = MaxAmmo;
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
		GameOverText.enabled = false;


	}
	
	// Update is called once per frame
	void Update () 
	{
		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
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
		GameOverText.enabled = true;
		GameOverText.text = "Game Over!";
		Invoke("RestartLevel", 3.0f);
	}
	
	void RestartLevel()
	{
		Destroy(gameObject);
		Application.LoadLevel(Application.loadedLevel);
	}
	
}
