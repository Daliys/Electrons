using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPlay : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject GameOverPanel;
    public GameObject PausePanel;
    public GameObject PauseBt;
    public GameObject Timer;
    public static int money = 0;
    public Text Money;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Game.isGameOver == true && !GameOverPanel.activeSelf)
        {

            GameOverPanel.SetActive(true);
            PauseBt.SetActive(false);
            Money.text = money.ToString();
            Game.numCoin += money;
            Game.SaveAll();
            Quests.QuestsWin();
        }
    }

    public void PauseBttn()
    {
        PausePanel.SetActive(true);
        Game.isGamePause = true;
    }

    public void PauseBttnBack()
    {
        PausePanel.SetActive(false);
        StartCoroutine(PauseCoroutine());
    }

    IEnumerator PauseCoroutine()
    {
        Timer.SetActive(true);
        yield return new WaitForSeconds(2.98f);
        Timer.SetActive(false);
        Game.isGamePause = false;
    }

    public void RestartButton()
    {
        Game.isGameOver = false;
        Game.ElectronHP = 3;
        Game.GameSpeed= 7;
        Game.score= 0;
        SceneManager.LoadScene(0);
        PauseBt.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }


}
