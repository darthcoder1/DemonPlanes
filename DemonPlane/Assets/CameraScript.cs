using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float DampTime = 0.15f;
	private GameObject[] Players;
	private Vector3 CurrentCamVelocity = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		Players = GameObject.FindGameObjectsWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 PlayerCenterPos = Vector3.zero;

		foreach (GameObject Player in Players) 
		{
			PlayerCenterPos = Player.transform.position;
		}

		PlayerCenterPos /= Players.Length;
		PlayerCenterPos.z = transform.position.z;

		transform.position = Vector3.SmoothDamp(transform.position, PlayerCenterPos, ref CurrentCamVelocity, DampTime);
	}
}
