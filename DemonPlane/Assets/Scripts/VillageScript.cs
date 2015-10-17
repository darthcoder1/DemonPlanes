using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VillageScript : MonoBehaviour 
{
	List<GameObject> DemonsInVillage;
	private HealthBar HealthBarComp;
	private GameObject FireSpawnPoint;
	private GameObject VillagBurns;
	public Text VillageIntact;
	private int VillageHealth;
	private AudioSource[] audios;
	private bool bDead;

    public bool IsDestroyed { get { return HealthBarComp.Health <= 0; } }

	// Use this for initialization
	void Start () 
	{
		audios = GetComponents<AudioSource>();
		DemonsInVillage = new List<GameObject> ();
		//HealthBarComp = GetComponent<HealthBar> ();
		VillageHealth = 100;
		VillageIntact.text=VillageHealth.ToString()+" %";
		VillageIntact.enabled = true;
		bDead = false;

	}
	
	// Update is called once per frame
	void Update () 
	{
        if (VillageIntact)
        {
            VillageIntact.text = VillageHealth.ToString() + " %";
        }
        /*
		float calcHealth = (float)HealthBarComp.Health;
		foreach (GameObject Demon in DemonsInVillage) 
		{
			float DmgPerSec = (float)(Demon.GetComponent<DemonBehavior>().VillageDamagePerSecond);
			calcHealth -= DmgPerSec * Time.deltaTime;
		}

		HealthBarComp.Health = Mathf.FloorToInt (Mathf.Max (calcHealth, 0.0f));
		*/
	}
	
	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			if(VillageHealth > 0)
			{

				audios[0].Play();
				Invoke("PlayVillageDestroyedMessage", 1);
				GameObject[] foundFireSpawnpoints = GameObject.FindGameObjectsWithTag("village_fire_spawn");
				GameObject nearestFireSpawn = null;

				Vector3 demonPos = collision.gameObject.transform.position;
				GameObject.Destroy (collision.gameObject);

				float nearestFireSpawnDistance = float.MaxValue;
				foreach (GameObject spawnPoint in foundFireSpawnpoints)
				{
					float dist = Vector2.Distance(spawnPoint.transform.position, demonPos);
					if (nearestFireSpawn == null || dist < nearestFireSpawnDistance)
					{

						nearestFireSpawn = spawnPoint;
						nearestFireSpawnDistance = dist;
					}
				}
				if(nearestFireSpawn != null)
				{
					FireSpawnPoint = nearestFireSpawn;
					VillageHealth -= 10;
					Quaternion defaultRotation = GameObject.Find ("default_rotation").transform.rotation;
					VillagBurns=(GameObject)Instantiate (Resources.Load ("Smoke_01"), FireSpawnPoint.transform.position, defaultRotation);
					GameObject.Destroy (FireSpawnPoint);
				}
			    if(VillageHealth < 10){
					Die ();
				}	
			}
			else if(!bDead)
			{
				Die ();
			}
						

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
		if (!bDead) {
			print ("die dorf die!");
			bDead = true;
			GameObject.FindGameObjectWithTag ("Player").SendMessage ("Die");
		}
	}
	void PlayVillageDestroyedMessage()
	{

		audios [1].Play ();
	}
}
