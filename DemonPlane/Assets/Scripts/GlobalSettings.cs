using UnityEngine;
using System.Collections;

public class GlobalSettings : ScriptableObject
{

    static GlobalSettings s_settings = null;

    public bool UseGameJolt = false;
    public bool UseKongregate = false;
    public bool GameJoltInitialized = false;

    static public GlobalSettings Instance
    {
        get
        {
            if (!s_settings)
            {
                s_settings = ScriptableObject.CreateInstance<GlobalSettings>();
            }
            return s_settings;
        }
    }
}
