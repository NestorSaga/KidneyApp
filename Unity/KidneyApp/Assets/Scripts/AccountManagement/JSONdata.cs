using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class JSONdata
{
    public string _id;
    public string username;
    public AchievementData[] userAchievements;
    public string[] seenVideos;
    public string[] doneTests;
    public string[] doneMentalTests;
    public string[] doneMenus;

    [System.Serializable]
    public class AchievementData
    {
        public string _Id;
        public string userId;
        public string achievementId;
        public string date;
        public int completion;
    }
    
}