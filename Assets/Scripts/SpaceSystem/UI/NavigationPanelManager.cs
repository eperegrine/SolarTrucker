using Missions;
using TradingPost;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace SpaceSystem
{
    public class NavigationPanelManager : MonoBehaviour
    {
        public Transform StationList;
        public Transform CurrentMissions;

        public GameObject StationItemPrefab;
        public GameObject CurrentPrefab;

        private void Awake()
        {
            UpdateUI();
        }

        public void PostAction()
        {
            SpaceSystemManager.Instance.SetTarget();
            UpdateUI();
        }

        public void UpdateUI()
        {
            StationList.DestroyChildren();
            CurrentMissions.DestroyChildren();
            
            var db = MissionManager.LoadDb();
            var current = db.CurrentMissions;

            foreach (var m in current)
            {
                var go = Instantiate(CurrentPrefab, CurrentMissions);
                var panel = go.GetComponent<CurrentMissionItemPanel>();
                panel.SetMission(m, PostAction);
            }

            var tradingPosts = SpaceSystemManager.Instance.SystemRegistry.TradingPosts;
            foreach (var m in tradingPosts)
            {
                var go = Instantiate(StationItemPrefab, StationList);
                var panel = go.GetComponent<StationItemPanel>();
                panel.SetStation(m, PostAction);
            }
        }
    }
}