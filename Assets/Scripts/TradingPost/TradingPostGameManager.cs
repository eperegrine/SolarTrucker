using CargoManagement;
using SystemMap;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        public ShipLoadingController ShipLoadingController;
        
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

            var currentCargo = PlayerPrefs.GetString(SpaceTruckerConstants.CargoKey);
            if (!string.IsNullOrEmpty(currentCargo))
            {
                ShipLoadingController.LoadCargo(currentCargo);
            }
        }

        public void GotoSpace()
        {
            var cargo = ShipLoadingController.SaveCargoToJson();
            PlayerPrefs.SetString(SpaceTruckerConstants.CargoKey, cargo);
            SceneManager.LoadScene(SpaceTruckerConstants.SpaceScene);
        }
    
        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }
    }
}