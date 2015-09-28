using UnityEngine;
using System.Collections;

public class DieAfterGivenTime : MonoBehaviour {

	public float TimeToDie;

	// Use this for initialization
	void Start () {

		Invoke ("Die", TimeToDie);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Die()
	{
		GameObject.Destroy(gameObject);
	}
}
