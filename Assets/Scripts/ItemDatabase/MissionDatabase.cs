using System;
using System.Collections.Generic;
using System.Linq;
using CargoManagement;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace ItemDatabase
{
    [Serializable]
    public class MissionProgress
    {
        public string Id { get; set; }
        public int ItemsDelivered { get; set; }
        public Mission Info { get; set; }
    }
    
    public class MissionDatabase
    {
        public List<MissionProgress> CurrentMissions { get; set; } = new();
        public string ActiveMissionId { get; set; }

        public string AddMission(Mission newMission)
        {
            var id = Guid.NewGuid().ToString();
            var progress = new MissionProgress()
            {
                Id = id,
                Info = newMission,
                ItemsDelivered = 0
            };
            CurrentMissions.Add(progress);
            return id;
        }

        [CanBeNull]
        public MissionProgress GetActiveMission()
        {
            return string.IsNullOrWhiteSpace(ActiveMissionId) ? null : CurrentMissions.First(x => x.Id == ActiveMissionId);
        }

        public bool ProgressMission(string id, int amt)
        {
            var mission = CurrentMissions.First(x => x.Id == id);
            if (mission == null) return false;
            mission.ItemsDelivered += amt;

            var complete = mission.ItemsDelivered >= mission.Info.Quantity;
            if (complete)
            {
                CurrentMissions.Remove(mission);
            }

            return complete;
        }
        
        public string SaveToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static MissionDatabase LoadFromString(string str)
        {
            return JsonConvert.DeserializeObject<MissionDatabase>(str);
        }
        
    }
}