using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class CargoInfo
{
    public string Id;
    public string Name;
    public int BuyValue = 50;
    public int SellValue = 70;
}
    
[CreateAssetMenu(fileName = "New Cargo", menuName = "Cargo/CargoObject")]
public class CargoObject : ScriptableObject
{
    public CargoInfo Info; 
    public GameObject Prefab;
    public Sprite Icon;
}