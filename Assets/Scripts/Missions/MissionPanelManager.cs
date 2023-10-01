using System;
using TradingPost;
using UnityEngine;
using Utils;

namespace Missions
{
    public class MissionPanelManager : MonoBehaviour
    {
        public Transform AvailableMissions;
        public Transform CurrentMissions;

        public GameObject AvailablePrefab;
        public GameObject CurrentPrefab;

        private void Awake()
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            AvailableMissions.DestroyChildren();
            CurrentMissions.DestroyChildren();
            
            var db = MissionManager.LoadDb();
            var current = db.CurrentMissions;

            foreach (var m in current)
            {
                var go = Instantiate(CurrentPrefab, CurrentMissions);
                var panel = go.GetComponent<CurrentMissionItemPanel>();
                panel.SetMission(m, UpdateUI);
            }

            var available = TradingPostGameManager.Instance.AvailableMissions;
            foreach (var m in available)
            {
                var go = Instantiate(AvailablePrefab, AvailableMissions);
                var panel = go.GetComponent<AvailableMissionItemPanel>();
                panel.SetMission(m, UpdateUI);
            }
        }
    }
}