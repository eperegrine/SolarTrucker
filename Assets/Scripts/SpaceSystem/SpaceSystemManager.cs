using SystemMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSystem
{
    public class SpaceSystemManager : MonoBehaviour
    {
        private static SpaceSystemManager _instance;
        public static SpaceSystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpaceSystemManager>();
                }

                return _instance;
            }
        }

        public SystemRegistry SystemRegistry;
        
        public void LoadTradingPost(SystemMap.TradingPost tp)
        {
            PlayerPrefs.SetString(SpaceTruckerConstants.TradingPostKey, tp.Id);
            SceneManager.LoadScene(SpaceTruckerConstants.TradingPostScene);
        }
        

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }
    }
}