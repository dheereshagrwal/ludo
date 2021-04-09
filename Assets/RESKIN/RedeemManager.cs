using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedeemManager : MonoBehaviour
{
    [HideInInspector]
    public string priceforMail;
    [HideInInspector]
    public string coinsforMail;

    public void PriceFeed(Text price)
    {
        priceforMail = price.text;
    }

    public void ReduceCoins(Text coins)
    {
        coinsforMail = coins.text;
    }
}
