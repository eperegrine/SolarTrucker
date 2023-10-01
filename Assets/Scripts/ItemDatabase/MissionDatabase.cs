using System;
using System.Collections.Generic;
using System.Linq;
using CargoManagement;
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
        public List<MissionProgress> ActiveMissions { get; set; }
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
            ActiveMissions.Add(progress);
            return id;
        }

        public MissionProgress GetActiveMission()
        {
            return string.IsNullOrWhiteSpace(ActiveMissionId) ? null : ActiveMissions.First(x => x.Id == ActiveMissionId);
        }

        public bool ProgressMission(string id, int amt)
        {
            var mission = ActiveMissions.First(x => x.Id == id);
            if (mission == null) return false;
            mission.ItemsDelivered += amt;

            var complete = mission.ItemsDelivered >= mission.Info.Quantity;
            if (complete)
            {
                ActiveMissions.Remove(mission);
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