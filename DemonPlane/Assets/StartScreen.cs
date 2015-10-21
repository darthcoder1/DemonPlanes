using UnityEngine;
using System.Collections;
using GameJolt;

public class StartScreen : MonoBehaviour {

    public string NextSceneName;
    
    public bool WithGameJolt;
    public bool WithKongregate;

    private bool SignInFinished = false;

	void Start ()
    {
        SignInFinished = false;

        if (WithGameJolt)
        {
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
        else if (WithKongregate)
        {
            GlobalSettings.Instance.UseKongregate = true;
            if (!KongregateAPI.Instance.Connected)
            {
                KongregateAPI.Instance.Connect(
                    (bool success) =>
                    {
                        SignInFinished = true;
                    });
            }
        }
        else
        {
            SignInFinished = true;
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
