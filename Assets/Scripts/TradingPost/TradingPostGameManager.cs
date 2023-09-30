using CargoManagement;
using SystemMap;
using UnityEngine;

namespace TradingPost
{
    public class TradingPostGameManager : MonoBehaviour
    {
        private static TradingPostGameManager _instance;
        public static TradingPostGameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TradingPostGameManager>();
                }

                return _instance;
            }
        }
    
        public CargoRegistry CargoRegistry;
        public SystemRegistry SystemRegistry;

        private void Start()
        {
            var currentPost = PlayerPrefs.GetString(SpaceTruckerConstants.TradingPostKey);
            if (!string.IsNullOrEmpty(currentPost))
            {
                var info = SystemRegistry.FindTradingPost(currentPost);

                Debug.Log($"At Trading Post: {info.name}\n{info.Description}");
            }
        }
    
        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }
    }
}