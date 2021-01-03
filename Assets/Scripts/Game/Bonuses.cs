using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonuses : MonoBehaviour
{
    public static bool IsBonusShield;
    public static bool isBonusX2Coins;
    public static bool isBonusX2Score;

    public static float bonusesTime = 5f;

    private static BonusInditification.BonusType needActive;
    private static bool isNeedActivation;
    void Start()
    {
       // print("Bonus " + needActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (isNeedActivation)
        {
            switch (needActive)
            {
                case BonusInditification.BonusType.Shield:
                    StartCoroutine(BonusLive(res => IsBonusShield = res, bonusesTime, transform.GetChild(2).gameObject));
                    break;
                case BonusInditification.BonusType.X2Coins:
                    StartCoroutine(BonusLive(res => isBonusX2Coins = res, bonusesTime, transform.GetChild(1).gameObject));
                    break;
                case BonusInditification.BonusType.X2Score:
                    StartCoroutine(BonusLive(res => isBonusX2Score = res, bonusesTime, transform.GetChild(0).gameObject));
                    break;
            }
            isNeedActivation = false;
        }
    }

    public static void ActiveBonus(BonusInditification.BonusType bonusType)
    {
        if (bonusType == BonusInditification.BonusType.Coin)
        {
            //if (isBonusX2Coins) CanvasPlay.money++;
            CanvasPlay.money++;

        }
        else
        {
            needActive = bonusType;
            isNeedActivation = true;
        }

    }

    public IEnumerator BonusLive(Action<bool> bonus ,float time, GameObject Ui)
    {
        bonus(true);
        Ui.SetActive(true);
        yield return new WaitForSeconds(time);
        bonus(false);
        Ui.SetActive(false);  
    }

}
