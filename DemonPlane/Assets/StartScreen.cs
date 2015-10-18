using UnityEngine;
using System.Collections;
using GameJolt;

public class StartScreen : MonoBehaviour {

    public string NextSceneName;
    public bool WithGameJolt;

    private bool SignInFinished = false;

	// Use this for initialization
	void Start ()
    {
        SignInFinished = !WithGameJolt;

        if (!GlobalSettings.Instance.GameJoltInitialized)
        {
            GlobalSettings.Instance.GameJoltInitialized = true;
            GlobalSettings.Instance.UseGameJolt = WithGameJolt;
            if (WithGameJolt)
            {
                GameJolt.UI.Manager.Instance.ShowSignIn(
                    (bool success) =>
                    {
                        SignInFinished = true;
                    });
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (SignInFinished)
        {
            if (Input.GetButton("AltClimb") || Input.GetButton("ReleaseWater")|| Input.GetButton("Shoot"))
            {
                Application.LoadLevel(NextSceneName);
            }
        }
        
	}
}
