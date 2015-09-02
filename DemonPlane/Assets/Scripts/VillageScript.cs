using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillageScript : MonoBehaviour 
{
	List<GameObject> DemonsInVillage;
	private HealthBar HealthBarComp;

	// Use this for initialization
	void Start () 
	{
		DemonsInVillage = new List<GameObject> ();
		HealthBarComp = GetComponent<HealthBar> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float calcHealth = (float)HealthBarComp.Health;
		foreach (GameObject Demon in DemonsInVillage) 
		{
			float DmgPerSec = (float)(Demon.GetComponent<DemonBehavior>().VillageDamagePerSecond);
			calcHealth -= DmgPerSec * Time.deltaTime;
		}

		HealthBarComp.Health = Mathf.FloorToInt (Mathf.Max (calcHealth, 0.0f));
	}
	
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			DemonsInVillage.Add (collision.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			DemonsInVillage.Remove (collision.gameObject);
		}
	}

	void Die()
	{
		GameObject.FindGameObjectWithTag ("Player").SendMessage ("Die");
	}
}
