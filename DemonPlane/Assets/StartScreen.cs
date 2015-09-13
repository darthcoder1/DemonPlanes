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
        float pressedBtn = Input.GetAxis("AltClimb");
        if (pressedBtn > 0)
        {
            Application.LoadLevel(NextSceneName);
        }
	}
}
