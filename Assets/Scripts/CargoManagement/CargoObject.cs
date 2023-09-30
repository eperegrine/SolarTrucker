using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CargoInfo
{
    public string Id;
    public int CreditValue;
}
    
[CreateAssetMenu(fileName = "New Cargo", menuName = "Cargo/CargoObject")]
public class CargoObject : ScriptableObject
{
    public CargoInfo Info; 
    public GameObject Prefab;
}