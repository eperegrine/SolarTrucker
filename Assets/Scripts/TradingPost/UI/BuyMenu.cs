using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace TradingPost.UI
{
    public class BuyMenu : MonoBehaviour
    {
        public GameObject ButtonGrid;
        public GameObject BuyButtonPrefab;

        private List<BuyButton> buttons;
        
        private void Start()
        {
            //Kill all children
            foreach (Transform child in ButtonGrid.transform)
            {
                Destroy(child.gameObject);
            }

            var inv = TradingPostGameManager.Instance.TradingPostInfo.Selling;
            buttons = new List<BuyButton>(inv.Count);
            foreach (var cargoObject in inv)
            {
                var go= Instantiate(BuyButtonPrefab, ButtonGrid.transform);
                var btn = go.GetComponent<BuyButton>();
                btn.SetSelling(cargoObject);
                buttons.Add(btn);
            }
        }

        private bool lastAvailableSlots = false;
        private bool availableSlots = true;

        private void Update()
        {
            availableSlots = TradingPostGameManager.Instance.ShipLoadingController.availableBuySlots.Any();
            Debug.Log($"MENU {availableSlots}");
            if (availableSlots != lastAvailableSlots)
            {
                buttons.ForEach(b => b.slotsAvailable = availableSlots);
            }
            
            lastAvailableSlots = availableSlots;
        }
    }
}