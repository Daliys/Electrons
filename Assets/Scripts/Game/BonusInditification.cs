using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusInditification : MonoBehaviour
{
    public static bool isX2Coin;

   public enum BonusType
    {
        Coin, X2Coins, X2Score, Shield
    }

    public BonusType bonus;


}
