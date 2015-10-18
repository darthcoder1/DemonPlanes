using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievmentManager : ScriptableObject {

    public const int kAchievement_SaveThePig_1 = 43242;
    public const int kAchievement_SaveThePig_5 = 43241;
    public const int kAchievement_SaveThePig_20 = 43243;
    public const int kAchievement_SaveThePig_50 = 43245;

    static AchievmentManager s_mgr;

    private List<int> UnlockedAchievements = new List<int>();

    static public AchievmentManager Instance
    {
        get
        {
            if (!s_mgr)
            {
                s_mgr = ScriptableObject.CreateInstance<AchievmentManager>();
            }
            return s_mgr;
        }
    }

    public void UnlockAchievement(int trophyID)
    {
        if (UnlockedAchievements.Contains(trophyID)) { return; }

        if (GlobalSettings.Instance.UseGameJolt)
        {
            GameJolt.API.Trophies.Unlock(trophyID, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("Unlocked Trophy: " + trophyID);
                    UnlockedAchievements.Add(trophyID);
                }
            });
        }
    }
}
