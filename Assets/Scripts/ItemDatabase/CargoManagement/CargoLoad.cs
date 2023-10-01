using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TradingPost;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;

namespace CargoManagement
{
    public class CargoLoadItem
    {
        public SerializableVector RelativePosition { get; set; }
        public float Rotation { get; set; }
        public string CargoId { get; set; }
        public bool PlayerOwned { get; set; } = true;
    }

    [Serializable]
    public class CargoLoad
    {
        public List<CargoLoadItem> Items = new();

        public void LoadFromMovableCargo(Vector3 storagePos, IEnumerable<MovableCargo> cargo)
        {
            foreach (var movableCargo in cargo)
            {
                var transform = movableCargo.transform;
                Items.Add(new CargoLoadItem()
                {
                    RelativePosition = (SerializableVector)transform.position - storagePos,
                    Rotation = transform.rotation.eulerAngles.z,
                    CargoId = movableCargo.CargoInfo.Info.Id,
                    PlayerOwned = movableCargo.playerOwned
                });
            }
        } 
        
        public string SaveToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static CargoLoad LoadFromString(string str)
        {
            return JsonConvert.DeserializeObject<CargoLoad>(str);
        }

        public bool IsEmpty => Items.Count == 0;
        
        public List<MovableCargo> Instantiate(Vector3 core, CargoRegistry registry)
        {
            List<MovableCargo> newCargo = new List<MovableCargo>(Items.Count);
            foreach (var cargoLoadItem in Items)
            {
                var cargoObj = registry.FindCargoById(cargoLoadItem.CargoId);
                var obj = Object.Instantiate(
                    cargoObj.Prefab, 
                    core + cargoLoadItem.RelativePosition,
                    Quaternion.Euler(0,0,cargoLoadItem.Rotation));
                var mc = obj.GetComponent<MovableCargo>();
                mc.playerOwned = cargoLoadItem.PlayerOwned;
                newCargo.Add(mc);
            }

            return newCargo;
        }
    }
}