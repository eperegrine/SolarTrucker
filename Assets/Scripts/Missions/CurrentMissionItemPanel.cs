using ItemDatabase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Missions
{
    public class CurrentMissionItemPanel : MonoBehaviour
    {
        public MissionProgress MissionInfo { get; private set; }
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        public Button ActionButton;

        public void SetMission(MissionProgress mission, UnityAction onClick)
        {
            Debug.Log(mission.Id);
            MissionInfo = mission;
            var activeMission = MissionManager.GetActiveMission();
            var isActiveMission = activeMission != null && activeMission.Id == mission.Id;
            Name.text = mission.Info.Title + (isActiveMission ? "*" : "");
            Description.text = mission.Info.Description;
            ActionButton.interactable = !isActiveMission;
            ActionButton.onClick.AddListener(OnButtonActivated);
            ActionButton.onClick.AddListener(onClick);
        }

        private void OnButtonActivated()
        {
            MissionManager.SetActiveMission(MissionInfo.Id);
        }
    }
}
