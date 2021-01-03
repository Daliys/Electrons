using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quests : MonoBehaviour
{
    public static int OneRoundMoney = 0;
    private static string[] questsStrings = {
        "25 монет за один забег",
        "накопить суммарно 100 монет"
    };



    public static List<QuestInformation> activeQuest = new List<QuestInformation>();

    private void Start()
    {
    
        activeQuest.Add(new QuestInformation(0, false, false));
        activeQuest.Add(new QuestInformation(1, false, false));
        QuestsWin();
    }

    public static void QuestsWin()
    {
        OneRoundMoney = CanvasPlay.money;
        for (int i = 0; i < activeQuest.Count; i++)
        {
            switch (activeQuest[i].id)
            {
                case 0:
                    if (OneRoundMoney >= 25 && !activeQuest[i].isQuestComplited)
                    {
                        activeQuest[i].isQuestComplited = true;
                       // Game.numCoin += 5;
                    }
                    break; 

                case 1:
                    if (Game.numCoin >= 100 && !activeQuest[i].isQuestComplited)
                    {
                        activeQuest[i].isQuestComplited=true; 
                       //Game.numCoin += 5;
                    }
                    break;
            }
        }
    }


    public static string GetProgressText(int idQuest)
    {
        switch (idQuest)
        {
            case 0: return "[" + Mathf.Clamp(OneRoundMoney, 0, 25) + " / " + "25] " + (activeQuest[0].isQuestComplited? "Выполнен": "Не выполнен");
            case 1: return "[" + Mathf.Clamp(Game.numCoin, 0, 100) + " / " + "100] " + (activeQuest[1].isQuestComplited? "Выполнен": "Не выполнен"); 
        }
        return null;
    }

    public static float GetProgress(int idQuest)
    {
        switch (idQuest)
        {
            case 0: return Mathf.Clamp(OneRoundMoney, 0, 25) / 25f;
            case 1: return Mathf.Clamp(Game.numCoin, 0, 100) / 100f;
        }
        return 0f;
    }

    public static string GetQuestConditionText(int idQuest)
    {
        return questsStrings[idQuest];
    }

    public static string GetQuestPrizeText(int idQuest)
    {
        return "50";
    }

    public static bool IsQuestCompletedAndNeedGetPrize(int idQuest)
    {
        return activeQuest[idQuest].isQuestComplited && !activeQuest[idQuest].isPrizeTaken;
    }
    public static bool IsQuestInProgress(int idQuest)
    {
        return !activeQuest[idQuest].isQuestComplited;
    }

    public static void GetPrize(int idQuest)
    {
        switch (idQuest)
        {
            case 0: Game.numCoin += 50; activeQuest[idQuest].isPrizeTaken = true; break;
            case 1: Game.numCoin += 50; activeQuest[idQuest].isPrizeTaken = true; break;
        }
    }

    public static bool IsQuestCompleted(int idQuest)
    {
        return activeQuest[idQuest].isQuestComplited && activeQuest[idQuest].isPrizeTaken;
    }

    public class QuestInformation
    {
        public int id;
        public bool isQuestComplited;
        public bool isPrizeTaken;
        public QuestInformation(int id, bool isComplited, bool isPrizeTaken)
        {
            this.id = id;
            isQuestComplited = isComplited;
            this.isPrizeTaken = isPrizeTaken;
        }
      }
}
