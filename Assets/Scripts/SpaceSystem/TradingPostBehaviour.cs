using System.Collections.Generic;
using UnityEngine;

namespace SpaceSystem
{
    public class TradingPostBehaviour : MonoBehaviour
    {
        public SystemMap.TradingPost Info;
        public List<DockingPoint> DockingPoints;

        private void Start()
        {
            var counter = 0;
            DockingPoints.ForEach(x => x.SetTradingPost(Info, counter++));
        }
    }
}