using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ForestScript : MonoBehaviour 
{
	private GameObject[] FireObjects;
	private List<GameObject> TrackedDemons;


	// Use this for initialization
	void Start () 
	{
		List<GameObject> objs = new List<GameObject>();

		for(int i=0; i < transform.childCount;++i)
		{
			Transform childTrans = transform.GetChild(i);

			ParticleSystem PSComp = childTrans.gameObject.GetComponent<ParticleSystem>();
			if (PSComp)
			{
				PSComp.enableEmission = false;
				objs.Add (childTrans.gameObject);
			}
		}

		FireObjects = objs.ToArray();
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTrackedDemons();
	}

	void UpdateTrackedDemons()
	{

	}

	void OnTriggerEnter2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			TrackedDemons.Add (collision.gameObject);
		}
	}
	
	void OnTriggerExit2D(Collider2D collision) 
	{
		if (collision.gameObject.tag == "enemy")
		{
			TrackedDemons.Remove (collision.gameObject);
		}
	}
}
