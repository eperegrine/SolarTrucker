using ItemDatabase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SpaceSystem
{
    public class StationItemPanel : MonoBehaviour
    {
        public SystemMap.TradingPost StationInfo { get; private set; }
        public TextMeshProUGUI Name;
        public Button ActionButton;

        public void SetStation(SystemMap.TradingPost station, UnityAction onClick)
        {
            StationInfo = station;
            Name.text = station.Name;
            var targetTp = MissionManager.GetTargetTp();
            ActionButton.interactable = targetTp != station.Id;
            ActionButton.onClick.AddListener(OnButtonActivated);
            ActionButton.onClick.AddListener(onClick);
        }

        public void OnButtonActivated()
        {
            MissionManager.SetTargetTp(StationInfo.Id);
            SpaceSystemManager.Instance.SetTarget();
        }
    }
}