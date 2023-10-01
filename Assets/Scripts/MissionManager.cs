﻿using System.Linq;
using ItemDatabase;
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

    public static void SaveDb(MissionDatabase db)
    {
        PlayerPrefs.SetString(SpaceTruckerConstants.MissionDbKey, db.SaveToString());
    }
    
    public static string AddMission(Mission newMission)
    {
        var db = LoadDb();
        var res = db.AddMission(newMission);
        SaveDb(db);
        return res;
    }

    public static MissionProgress GetActiveMission()
    {
        var db = LoadDb();
        var res = db.GetActiveMission();
        return res;
    }

    public static void SetActiveMission(string id)
    {
        var db = LoadDb();
        var exists = db.ActiveMissions.Any(x => x.Id == id);
        if (exists)
        {
            db.ActiveMissionId = id;
            SaveDb(db);
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