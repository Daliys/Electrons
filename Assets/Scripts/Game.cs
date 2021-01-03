using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    
    public static float GameSpeed = 7f;
    public static bool isGamePause = false;
    public static bool isGameOver = false;
    public static float timeInvulnerable = 2f; // время неуязвимости
    public static float score = 0;
    public static int numCoin = 120;

    
    public static int ElectronHP {
        get { return electronHP; }
        set
        {
            electronHP = value;
            if (electronHP <= 0)
            {
                electronHP = 0;
                GameOver();
            }
        }
    }

    private static int electronHP = 3;

    private static void GameOver()
    {
        print("GameOver");
        isGamePause = true;
        isGameOver = true;
    }

    private void Awake()
    {
        isGamePause = true;
        numCoin = Preservation("NumCoin", 120);

    }

    public void Update()
    {

    }

    public static void SaveAll()
    {
        PreservationSave("NumCoin", numCoin);
    }

    static int  Preservation(string keyName, int value)
    {
        if (!PlayerPrefs.HasKey(keyName)) PlayerPrefs.SetInt(keyName, value);
        else value = PlayerPrefs.GetInt(keyName);
        return value;
    }
    static void PreservationSave(string keyName, int value)
    {
        PlayerPrefs.SetInt(keyName, value);
    }

    private void OnApplicationQuit()
    {
        SaveAll();
    }
}
