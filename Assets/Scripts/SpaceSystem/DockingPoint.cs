using UnityEngine;

namespace SpaceSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class DockingPoint : MonoBehaviour
    {
        public SystemMap.TradingPost TradingPostInfo;
        private int DockerIndex = 0;
        
        public void Dock()
        {
            PlayerPrefs.SetInt(SpaceTruckerConstants.DockingIndex, DockerIndex);
            SpaceSystemManager.Instance.LoadTradingPost(TradingPostInfo);
        }

        public void SetTradingPost(SystemMap.TradingPost tp, int index)
        {
            TradingPostInfo = tp;
            DockerIndex = index;
        }
    }
}