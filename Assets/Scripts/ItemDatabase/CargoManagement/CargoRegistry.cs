using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CargoManagement
{
    [CreateAssetMenu(fileName = "Cargo Registry", menuName = "Cargo/Registry")]
    public class CargoRegistry : ScriptableObject
    {
        public List<CargoObject> CargoOptions;

        public CargoObject FindCargoById(string id)
        {
            return CargoOptions.First(x => x.Info.Id == id);
        }
    }
}