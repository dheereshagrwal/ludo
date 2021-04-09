using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Redeem : MonoBehaviour {

    public Text coinsText;
    public Text priceText;
    public GameObject selectPayout;
    public GameObject LessCoinsError;
    public RedeemManager redeemManagerS;
    public void RedeemMoney()
    {
        if (Convert.ToInt32(GameManager.Instance.myPlayerData.GetCoins()) >= Convert.ToInt32(coinsText.text))
        {
            selectPayout.SetActive(true);

            redeemManagerS.PriceFeed(priceText);
            redeemManagerS.ReduceCoins(coinsText);
        }
        else{
            LessCoinsError.SetActive(true);
        }
        
    }

}
