using UnityEngine;
using System.Collections;

public class DieAfterGivenTime : MonoBehaviour {

	public float TimeToDie;
	public GameObject Parent;

	// Use this for initialization
	void Start () {

		//Parent = null;
		Invoke ("Die", TimeToDie);
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Parent != null) {
		

			transform.position=Parent.transform.position;
		}


	
	}

	void Die()
	{
		GameObject.Destroy(gameObject);
	}
}
