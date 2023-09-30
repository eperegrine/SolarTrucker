using UnityEngine;

namespace SpaceSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class DockingPoint : MonoBehaviour
    {
        public SystemMap.TradingPost TradingPostInfo;

        public void Dock()
        {
            SpaceSystemManager.Instance.LoadTradingPost(TradingPostInfo);
        }
    }
}