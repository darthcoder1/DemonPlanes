using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievmentManager : ScriptableObject {

    public const int kAchievement_SaveThePig_1 = 43242;
    public const int kAchievement_SaveThePig_5 = 43241;
    public const int kAchievement_SaveThePig_20 = 43243;
    public const int kAchievement_SaveThePig_50 = 43245;

    public const int kAchievement_SaveThePig_Rainbow = 43252;
    public const int kAchievement_SaveThePig_Dark = 43253;
    public const int kAchievement_SaveThePig_Zombie = 43254;

    public const int kAchievement_KillDemon_1 = 43249;
    public const int kAchievement_KillDemon_5 = 43248;
    public const int kAchievement_KillDemon_20 = 43250;
    public const int kAchievement_KillDemon_100 = 43251;

    public const int kAchievement_Skill_Sniper_1 = 43256;
    public const int kAchievement_Skill_Sniper_2 = 43259;
    public const int kAchievement_Skill_Sniper_3 = 43262;
    public const int kAchievement_Skill_Sniper_5 = 43263;

    public const int kAchievement_Skill_Whale_1 = 43257;
    public const int kAchievement_Skill_Whale_2 = 43260;
    public const int kAchievement_Skill_Whale_3 = 43264;
    public const int kAchievement_Skill_Whale_5 = 43266;

    public const int kAchievement_Skill_Hasty_1 = 43258;
    public const int kAchievement_Skill_Hasty_2 = 43261;
    public const int kAchievement_Skill_Hasty_3 = 43265;
    public const int kAchievement_Skill_Hasty_5 = 43267;

    public const int kAchievement_Rank_Frog = 43268;
    public const int kAchievement_Rank_DragonFly = 43269;
    public const int kAchievement_Rank_KingFisher = 43270;
    public const int kAchievement_Rank_AmericanEagle = 43271;

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
            GameJolt.API.Trophies.Get(trophyID, (GameJolt.API.Objects.Trophy trophy) =>
            {
                if (!trophy.Unlocked)
                {
                    GameJolt.API.Trophies.Unlock(trophy);
                    UnlockedAchievements.Add(trophyID);
                }
            });
        }
    }
}
