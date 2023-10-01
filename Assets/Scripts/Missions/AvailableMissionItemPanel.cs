using ItemDatabase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Missions
{
    public class AvailableMissionItemPanel : MonoBehaviour
    {
        public Mission MissionInfo { get; private set; }
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        public Button ActionButton;

        public void SetMission(Mission mission, UnityAction onClick)
        {
            MissionInfo = mission;
            Name.text = mission.Title;
            Description.text = mission.Description;
            ActionButton.onClick.AddListener(OnButtonActivated);
            ActionButton.onClick.AddListener(onClick);
        }

        public void OnButtonActivated()
        {
            MissionManager.AddMission(MissionInfo);
        }
    }
}
