using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public int MaxHealth = 100;


	public int Health;
	private bool bDrawGUI = false;

	// Use this for initialization
	void Start () 
	{
		Health = MaxHealth;
		bDrawGUI = true;
	}

	void OnGUI()
	{
		if (bDrawGUI && Camera.current) 
		{
			Vector3 screenPosition = Camera.current.WorldToScreenPoint(transform.position);// gets screen position.
			screenPosition.y = Screen.height - (screenPosition.y + 1);// inverts y
			Rect rect = new Rect(screenPosition.x - 50, screenPosition.y - 12, 100, 24);// makes a rect centered at the player ( 100x24 )

			GUI.color = Color.red;
			GUI.HorizontalScrollbar(rect, 0, Health, 0, MaxHealth); //displays a healthbar
			
			GUI.color = Color.white;
			GUI.contentColor = Color.white;                
			GUI.Label(rect, ""+Health+"/"+MaxHealth); //displays health in text format
		}
	}
	void OnBecameVisible () 
	{
		bDrawGUI = true;
	}
	
	void OnBecameInvisible () 
	{
		bDrawGUI = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Health <= 0) 
		{
			gameObject.SendMessage("Die");
		}
	}
}
