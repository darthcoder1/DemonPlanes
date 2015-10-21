using UnityEngine;
using System.Collections;

public class KongregateAPI : MonoBehaviour 
{
    private static KongregateAPI s_mgr = null;

    public bool Connected { get; private set; }
    public int UserId { get; private set; }
    public string Username { get; private set; }
    public string GameAuthToken { get; private set; }

    private System.Action<bool> ConnectionCallback;

    static public KongregateAPI Instance
    {
        get
        {
          return s_mgr;
        }
    }

	// Use this for initialization
	void Start ()
    {
        s_mgr = this;
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () 
    {
    }

	public void Connect(System.Action<bool> callback)
    {
        this.ConnectionCallback = callback;

        if (!Connected)
        {
            Application.ExternalEval(
                "if(typeof(kongregateUnitySupport) != 'undefined') {" +
                    "kongregateUnitySupport.initAPI('" + gameObject.name + "', 'OnKongregateAPILoaded');" +
                "}"
            );
        }
        else
        {
            Debug.LogWarning("You are attempting to connect to Kongregate's API multiple times. You only need to connect once.");
        }
	}

    public void Submit(string statisticName, int value)
    {
        if (Connected)
            Application.ExternalCall("kongregate.stats.submit", statisticName, value);
        else
            Debug.LogWarning("You are attempting to submit a statistic without being connected to Kongregate's API. Connect first, then submit.");
    }

    private void OnKongregateAPILoaded(string userInfoString)
    {
        if (!Connected)
        {
            Connected = true;
            string[] parameters = userInfoString.Split('|');
            UserId = System.Convert.ToInt32(parameters[0]);
            Username = parameters[1];
            GameAuthToken = parameters[2];

            if (ConnectionCallback != null)
            {
                ConnectionCallback(true);
            }
        }
    }
}
