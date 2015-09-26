using UnityEngine;
using System.Collections;

public class WaterShot : MonoBehaviour {

	public Vector3 Direction;
	public float currentSpeed;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position += Direction * currentSpeed * Time.deltaTime;


	
	}
}
