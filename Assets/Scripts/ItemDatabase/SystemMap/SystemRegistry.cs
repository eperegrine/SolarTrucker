using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SystemMap
{
    [CreateAssetMenu(menuName = "Map/SystemRegistry", fileName = "SystemRegistry")]
    public class SystemRegistry : ScriptableObject
    {
        public List<TradingPost> TradingPosts;

        public TradingPost FindTradingPost(string id) => TradingPosts.First(x => id == x.Id);
    }
}