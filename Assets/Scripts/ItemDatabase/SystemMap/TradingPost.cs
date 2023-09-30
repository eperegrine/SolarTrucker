using UnityEngine;

namespace SystemMap
{
    [CreateAssetMenu(menuName = "Map/TradingPost", fileName = "TradingPost")]
    public class TradingPost : ScriptableObject
    {
        public string Id;
        public string Name;
        public string Description;
    }
}