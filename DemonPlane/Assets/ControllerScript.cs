using UnityEngine;
using System.Collections;

public class ControllerScript : MonoBehaviour {

	private Vector3 Direction;
	public float MinSpeed;
	public float NormalSpeed;
	public float MaxSpeed;
	public float RotationSpeed;

	// Use this for initialization
	void Start () {

		Direction = new Vector3 (0, 1, 0);
	}
	
	// Update is called once per frame
	void Update () {

		float hAxis = Input.GetAxis ("Horizontal");
		float vAxis = Input.GetAxis ("Vertical");

		// Rotation
		float rotateBy = RotationSpeed * Time.deltaTime * hAxis;

		Quaternion rotateByQuat = Quaternion.AngleAxis (rotateBy, Vector3.back);
		Vector3 newDirection = rotateByQuat * Direction;

		Vector3 position = transform.position;
		Debug.DrawLine( position, position + Direction * 100, Color.red );

		transform.rotation = rotateByQuat * transform.rotation;

		float currentSpeed = NormalSpeed;
		// Movement
		if (vAxis < 0)
		{
			currentSpeed = Mathf.Lerp(MinSpeed, NormalSpeed,  1.0f + vAxis);
		}
		else if (vAxis > 0)
		{
			currentSpeed = Mathf.Lerp(NormalSpeed, MaxSpeed,  vAxis);
		}

		transform.position += Direction * currentSpeed * Time.deltaTime;

		Direction = newDirection;

	}
}
