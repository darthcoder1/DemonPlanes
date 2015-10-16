using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

    public string NextSceneName;

	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        
		if (Input.GetButton("AltClimb") || Input.GetButton("ReleaseWater")|| Input.GetButton("Shoot"))
        {
            Application.LoadLevel(NextSceneName);
        }
	}
}
