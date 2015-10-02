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
        bool pressedBtn = Input.GetButton("AltClimb");
        if (pressedBtn)
        {
            Application.LoadLevel(NextSceneName);
        }
	}
}
