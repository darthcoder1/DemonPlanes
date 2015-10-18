using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	public float DampTime = 0.15f;
	private GameObject Player;
	private Vector3 CurrentCamVelocity = Vector3.zero;
    private float CurrentZoomVelocity = 0;
    public float ZoomDamp = 0.2f;

    public float NormalSize = 10.0f;
    public float FastSize = 20.0f;

	// Use this for initialization
	void Start () 
	{
		Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 PlayerCenterPos = Vector3.zero;

        ControllerScript ctrl = Player.GetComponent<ControllerScript>();
        PlayerCenterPos = Player.transform.position;
        float speed = ctrl.currentSpeed;
		
        speed -= ctrl.NormalSpeed;
        if (speed > 0)
        {
            float range = ctrl.MaxSpeed - ctrl.NormalSpeed;
            float T = speed / range;
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(gameObject.GetComponent<Camera>().orthographicSize, Mathf.Lerp(NormalSize, FastSize, T), ref CurrentZoomVelocity, ZoomDamp);
            //gameObject.GetComponent<Camera>().orthographicSize = Mathf.Lerp(NormalSize, FastSize, T);
        }
        else
        {
            gameObject.GetComponent<Camera>().orthographicSize = Mathf.SmoothDamp(gameObject.GetComponent<Camera>().orthographicSize, NormalSize, ref CurrentZoomVelocity, ZoomDamp);
            //gameObject.GetComponent<Camera>().orthographicSize = NormalSize;
        }

		PlayerCenterPos = Player.transform.position;
		PlayerCenterPos.z = transform.position.z;
		transform.position = Vector3.SmoothDamp(transform.position, PlayerCenterPos, ref CurrentCamVelocity, DampTime);
	}
}
