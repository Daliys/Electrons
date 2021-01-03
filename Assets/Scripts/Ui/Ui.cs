using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ui : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject StartPanel;
    public GameObject Electron;
    public GameObject ShopCloseBttn;
    public GameObject QuestBttn;
    public GameObject QuestPanel;

    [SerializeField]
    public GameObject[] QuestsPaneles;
    //public GameObject TwoQuest;

    public Text MoneyCount;
    public void shopPanel_ShowAndHide()
    {
        shopPanel.SetActive(!shopPanel.activeSelf);
        ShopCloseBttn.SetActive(!ShopCloseBttn.activeSelf);
        Electron.SetActive(!Electron.activeSelf);
    }
    public void StartGame()
    {
        StartPanel.SetActive(!StartPanel.activeSelf);
   
    }
    private void Start()
    {
        MoneyCount.text = Game.numCoin.ToString();
    }

    public void QuestPan()
    {
        QuestPanel.SetActive(!QuestPanel.activeSelf);
        ActivateQuestPanels();

    }

    private void ActivateQuestPanels()
    {
        for (int i = 0; i < QuestsPaneles.GetLength(0); i++)
        {
            QuestsPaneles[i].transform.GetChild(2).GetComponent<Text>().text = Quests.GetQuestConditionText(0) + "\n" + Quests.GetProgressText(i);
            if (Quests.IsQuestInProgress(i))
            {
                QuestsPaneles[i].transform.GetChild(3).GetComponent<Slider>().value = Quests.GetProgress(i);    // progressBar
            }
            else if (Quests.IsQuestCompletedAndNeedGetPrize(i))
            {
                QuestsPaneles[i].transform.GetChild(3).gameObject.SetActive(false);     // progressBar
                QuestsPaneles[i].transform.GetChild(4).gameObject.SetActive(true);      // Get Prize panel
                QuestsPaneles[i].transform.GetChild(4).GetChild(0).GetChild(0).GetComponent<Text>().text = Quests.GetQuestPrizeText(i);
            }
            else if (Quests.IsQuestCompleted(i))
            {
                QuestsPaneles[i].transform.GetChild(0).gameObject.SetActive(true);     // background Completed
                QuestsPaneles[i].transform.GetChild(1).gameObject.SetActive(false);     // background Progressing
                QuestsPaneles[i].transform.GetChild(3).gameObject.SetActive(false);     // progressBar
                QuestsPaneles[i].transform.GetChild(4).gameObject.SetActive(false);      // Get Prize panel
                QuestsPaneles[i].transform.GetChild(5).gameObject.SetActive(true);     // Image Completed
            }

        }
    }

    public void GetPrizeButton(int idQuest)
    {
        Quests.GetPrize(idQuest);
        ActivateQuestPanels();
    }
}
