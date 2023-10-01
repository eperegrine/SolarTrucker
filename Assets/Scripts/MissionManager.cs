using System.Collections.Generic;
using System.Linq;
using ItemDatabase;
using JetBrains.Annotations;
using UnityEngine;

public static class MissionManager
{
    public static MissionDatabase LoadDb()
    {
        var m = PlayerPrefs.GetString(SpaceTruckerConstants.MissionDbKey);
        if (!string.IsNullOrWhiteSpace(m))
        {
            return MissionDatabase.LoadFromString(m);
        }

        return new MissionDatabase();
    }

    public static void SetTargetTp(string tpId)
    {
        PlayerPrefs.SetString(SpaceTruckerConstants.TargetTradingPost, tpId);
    }

    public static void SaveDb(MissionDatabase db)
    {
        PlayerPrefs.SetString(SpaceTruckerConstants.MissionDbKey, db.SaveToString());
    }
    
    public static string AddMission(Mission newMission)
    {
        var db = LoadDb();
        var res = db.AddMission(newMission);
        SetTargetTp(newMission.TradingPostId);
        SaveDb(db);
        return res;
    }

    [CanBeNull]
    public static MissionProgress GetActiveMission()
    {
        var db = LoadDb();
        var res = db.GetActiveMission();
        return res;
    }

    public static void SetActiveMission(string id)
    {
        var db = LoadDb();
        var exists = db.CurrentMissions.Any(x => x.Id == id);
        if (exists)
        {
            db.ActiveMissionId = id;
            SaveDb(db);
            var mission = GetActiveMission();
            SetTargetTp(mission.Info.TradingPostId);
        }
        else
        {
            Debug.LogError("Tried to set active mission that doesn't exist");
        }
    }

    public static bool ProgressMission(string id, int amt)
    {
        var db = LoadDb();
        var res = db.ProgressMission(id, amt);
        SaveDb(db);
        return res;
    }
}