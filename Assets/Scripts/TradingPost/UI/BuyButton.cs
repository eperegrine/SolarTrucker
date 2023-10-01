using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TradingPost;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public TextMeshProUGUI NameLabel;
    public TextMeshProUGUI CreditLabel;
    public Image Icon;
    public CargoObject Selling;
    
    public Button btn;
    [HideInInspector]
    public bool slotsAvailable = true;
    
    public void SetSelling(CargoObject selling)
    {
        Selling = selling;
        btn.interactable = MoneyManager.HasCredits(selling.Info.BuyValue);
        btn.onClick.AddListener(() =>
        {
            TradingPostGameManager.Instance.Buy(selling.Info);
        });
        NameLabel.text = selling.Info.Name;
        CreditLabel.text = $"B: {selling.Info.BuyValue} | S: {selling.Info.SellValue}";
        Icon.sprite = selling.Icon;
    }

    private void Update()
    {
        var hasMoney = MoneyManager.HasCredits(Selling.Info.BuyValue);
        Debug.Log($"Slots {slotsAvailable}, Money {hasMoney}");
        btn.interactable = slotsAvailable && hasMoney;
    }
}
