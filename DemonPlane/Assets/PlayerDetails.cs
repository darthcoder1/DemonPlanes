using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerDetails : MonoBehaviour {

	public int MaxAmmo;
	public int AmmoRefillPerSecond;
	public int AmmoUsagePerSecond;

	public int CurrentAmmo;
	private bool bOverLand;

	public Text AmmoDisplay;
	public ParticleSystem DropWaterFX;
	public ParticleSystem CollectWaterFX;
	public bool IsOverLand { get { return bOverLand; } }

	// Use this for initialization
	void Start () 
	{
		CurrentAmmo = MaxAmmo;

		AmmoDisplay.text = "Water: " + CurrentAmmo.ToString();
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
	
}
