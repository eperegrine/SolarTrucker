using CargoManagement;
using SystemMap;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        public SystemMap.TradingPost TradingPostInfo;
        
        public CargoRegistry CargoRegistry;
        public SystemRegistry SystemRegistry;

        public TextMeshProUGUI MoneyLabel;
        public TextMeshProUGUI BuyPanelMoneyLabel;

        private void Start()
        {
            var currentPost = PlayerPrefs.GetString(SpaceTruckerConstants.TradingPostKey);
            if (!string.IsNullOrEmpty(currentPost))
            {
                var info = SystemRegistry.FindTradingPost(currentPost);
                TradingPostInfo = info;
                Debug.Log($"At Trading Post: {info.name}\n{info.Description}");
            }

            var currentCargo = PlayerPrefs.GetString(SpaceTruckerConstants.CargoKey);
            if (!string.IsNullOrEmpty(currentCargo))
            {
                ShipLoadingController.LoadCargo(currentCargo);
            }
            
            UpdateMoneyLabel();
        }

        public void UpdateMoneyLabel()
        {
            var credits = MoneyManager.GetCredits();
            var text = $"Credits: {credits:0000}";
            MoneyLabel.text = text;
            BuyPanelMoneyLabel.text = text;
        }

        /// <summary>
        /// Handle the financial/mission aspect of selling
        /// The calling class handles deleting the cargo
        /// </summary>
        public void SellCargo(CargoObject cargo)
        {
            var amt = cargo.Info.SellValue;
            MoneyManager.AddCredits(amt);
            UpdateMoneyLabel();
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

        public void Buy(CargoInfo buying)
        {
            MoneyManager.AddCredits(-buying.BuyValue);
            ShipLoadingController.SpawnBoughtCargo(buying);
            UpdateMoneyLabel();
        }
    }
}