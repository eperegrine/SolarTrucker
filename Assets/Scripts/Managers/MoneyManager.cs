using UnityEngine;

public static class MoneyManager
{
    public static int GetCredits()
    {
        return PlayerPrefs.GetInt(SpaceTruckerConstants.MoneyKey, 200);
    }

    public static void AddCredits(int amt)
    {
        var current = GetCredits();
        current += amt;
        PlayerPrefs.SetInt(SpaceTruckerConstants.MoneyKey, current);
    }

    public static bool HasCredits(int amt)
    {
        return GetCredits() >= amt;
    }
}