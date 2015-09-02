using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct WaveInfo
{
	// Amount of demons to spawn in this wave
	public int NumDemons;
	// Time in seconds this wave takes for spawing
	public float SpawnTimeInSeconds;
	// Start delay, before this wave starts spawning after the last wave have been defeated
	public float StartSpawningDelay;
}

public class WaveSpawner : MonoBehaviour 
{
	public readonly string ObjTag_Volcano = "volcano";

	public WaveInfo[] EnemyWaves;
	public Text NextWaveStartsText;
	public Text WaveStartsText;

	// List of all volanos in the level
	private GameObject[] Volcanos;
	// Currently active wave
	private int CurrentWave;
	// indicates whether the wave is currently spawning
	bool bWaveSpawningActive;

	private float SpawnInterval;
	private float TimeLeftBeforeWaveStarts;
	private float TimeLeftBeforeNextSpawn;
	private int NextSpawnVolcanoIndex;
	private int NumSpawned;



	// Use this for initialization
	void Start () 
	{
		Volcanos = GameObject.FindGameObjectsWithTag (ObjTag_Volcano);

		CurrentWave = -1;
		bWaveSpawningActive = PrepareNextWave ();

		NextWaveStartsText.enabled = false;
		WaveStartsText.enabled = false;
	}

	bool PrepareNextWave()
	{
		if (++CurrentWave >= EnemyWaves.Length) 
		{
			// won
			return false;
		}

		SpawnInterval = EnemyWaves [CurrentWave].SpawnTimeInSeconds / (float)EnemyWaves [CurrentWave].NumDemons;
		TimeLeftBeforeWaveStarts = EnemyWaves [CurrentWave].StartSpawningDelay;
		TimeLeftBeforeNextSpawn = 0.0f;
		NextSpawnVolcanoIndex = 0;
		NumSpawned = 0;
		return true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (bWaveSpawningActive) {
			UpdateSpawning ();
		} 
		else 
		{
			GameObject[] aliveDemons = GameObject.FindGameObjectsWithTag("enemy");
			if (aliveDemons.Length <= 0)
			{
				bWaveSpawningActive = PrepareNextWave ();
			}
		}
	}

	void HideWaveText()
	{
		WaveStartsText.enabled = false;
	}

	void UpdateSpawning()
	{
		if (TimeLeftBeforeWaveStarts > 0) 
		{
			TimeLeftBeforeWaveStarts = Mathf.Max (TimeLeftBeforeWaveStarts - Time.deltaTime, 0.0f);
			NextWaveStartsText.enabled = true;
			NextWaveStartsText.text = string.Format("Next Wave starts in {0}s", Mathf.CeilToInt(TimeLeftBeforeWaveStarts));

			if (TimeLeftBeforeWaveStarts <= 0)
			{
				NextWaveStartsText.enabled = false;

				WaveStartsText.enabled = true;
				WaveStartsText.text = "Wave " + (CurrentWave+1).ToString();
				Invoke ("HideWaveText", 2.0f);
			}
			return;
		}

		if (TimeLeftBeforeNextSpawn > 0) 
		{
			TimeLeftBeforeNextSpawn = Mathf.Max (TimeLeftBeforeNextSpawn - Time.deltaTime, 0.0f);
			return;
		}

		Transform spawnTransform = GetNextSpawnTransform();
		GameObject.Instantiate (Resources.Load ("PrefDemon"), spawnTransform.position, spawnTransform.rotation);
		TimeLeftBeforeNextSpawn = SpawnInterval;

		if (++NumSpawned >= EnemyWaves[CurrentWave].NumDemons)
		{
			bWaveSpawningActive = false;
		}
	}
	
	Transform GetNextSpawnTransform()
	{
		GameObject volcano = Volcanos [NextSpawnVolcanoIndex++];

		if (NextSpawnVolcanoIndex >= Volcanos.Length) 
		{
			NextSpawnVolcanoIndex = 0;
		}

		return volcano.transform.GetChild (Random.Range (0, volcano.transform.childCount));
	}
}
