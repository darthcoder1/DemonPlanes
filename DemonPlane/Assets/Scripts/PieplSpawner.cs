using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public struct PieplWaveInfo
{


	// Amount of Piepl to spawn in this wave
	public int NumPiepl;
}


public class PieplSpawner : MonoBehaviour 
{

	public readonly string ObjTag_PieplSpawn = "piepl_spawn";
	
	public PieplWaveInfo[] PieplWaves;

	//list of all piepl spawn points
	private GameObject[] PieplSpawns;
	
	//list of all spawned Piepl
	private GameObject[] SpawnedPiepl;
	
	// Currently active wave
	public int CurrentWave;

	private bool bInvokedWinning;
	
	
	
	// Use this for initialization
	void Start () 
	{
		PieplSpawns = GameObject.FindGameObjectsWithTag (ObjTag_PieplSpawn);
		
		CurrentWave = 0;
		SpawnPiepl ();
		//bWaveSpawningActive = PrepareNextWave ();
		
	}

	public void SpawnPiepl()
	{
		//ShuffleArray<int>(PieplSpawns); 
		int CurrentNumPiepl= PieplWaves[CurrentWave].NumPiepl;
		int[] randomList = MakeIntList (PieplSpawns);
		GameObject JustSpawnedPiepl;

		if (CurrentNumPiepl > PieplSpawns.Length)
			CurrentNumPiepl = PieplSpawns.Length;

		Shuffle (randomList);
		int j = 0;
		int k = 0;

		for (j=0; j<CurrentNumPiepl;j++)
		{
			k=randomList[j];
			Transform spawnTransform = PieplSpawns[k].transform;
			//GameObject.Instantiate (Resources.Load ("piepl0"), spawnTransform.position, spawnTransform.rotation);
			//JustSpawnedPiepl=(GameObject)Instantiate (Resources.Load ("poorpiggy_01"), spawnTransform.position, spawnTransform.rotation);
			JustSpawnedPiepl=(GameObject)Instantiate (Resources.Load ("rainbowpiggy"), spawnTransform.position, spawnTransform.rotation);
			JustSpawnedPiepl.GetComponent<PieplBehavior>().SpawnDelay= Random.Range(1,65+CurrentWave*2);
			JustSpawnedPiepl.GetComponent<PieplBehavior>().LifeTime= Random.Range(35,75+CurrentWave*2);
			/*
			if((Random.Range(0, CurrentWave +1) < 3)) {
				JustSpawnedPiepl.GetComponent<PieplBehavior>().isSpecial=true;
				print("SPECIAL PIG BORN!");
			}*/
			JustSpawnedPiepl.GetComponent<PieplBehavior>().StartForReal();
		}

	}
	int[] MakeIntList(GameObject[] go)
	{
		int l;
		int[] myList=new int[go.Length] ;

		for (l=0; l<go.Length; l++) {

			myList[l]=l;
		}
		return myList;
	}


	void Shuffle(int[] a)
	{
		// Loops through array
		for (int i = a.Length-1; i > 0; i--)
		{
			// Randomize a number between 0 and i (so that the range decreases each time)
			int rnd = Random.Range(0,i);
			
			// Save the value of the current i, otherwise it'll overright when we swap the values
			int temp = a[i];
			
			// Swap the new and old values
			a[i] = a[rnd];
			a[rnd] = temp;
		}	

	}


}