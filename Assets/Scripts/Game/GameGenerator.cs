using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameGenerator : MonoBehaviour
{
    public GameObject cablePrefab;
    public GameObject electronPrefab;
    public GameObject NodePrefab;
    public GameObject CoinBonus;
    public GameObject X2CoinBonus;
    public GameObject X2ScoreBonus;
    public GameObject ShieldBonus;
    public GameObject DoubleCoinBonus;
  

    public Vector2[] cablePosition;
    private List<GameObject> objectsCables;

    public Slider SLIDER_FOR_TEST;
    

    private int electronPosition;

    void Start()
    {
        objectsCables = new List<GameObject>();
        Initialization();
        StartCoroutine(NodeGenerator());
    }

    public void Initialization()
    {
       

        for (int i = 0; i < cablePosition.Length; i++)
        {
            objectsCables.Add( Instantiate(cablePrefab, cablePosition[i], Quaternion.Euler(0,0,0), gameObject.transform));
            cablePosition[i].y = objectsCables[electronPosition].transform.GetChild(0).transform.position.y;
        }

        electronPosition = Random.Range(0, 4);
        Instantiate(electronPrefab, cablePosition[electronPosition], Quaternion.Euler(0, 0, 0), gameObject.transform);
    }


    void Update()
    {
        
    }


    bool[] cableAvailable = { true, true, true, true };

    IEnumerator NodeGenerator()
    {
        while (true)
        {
            if (Game.isGamePause)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }
            float value = SLIDER_FOR_TEST.value;

            yield return new WaitForSeconds(Random.Range(0.3f * (1 / value), 2 * (1 / value)));
            int cableID = Random.Range(0, 4);
            // Vector2 pos = new Vector2(cablePosition[cableID].x, 6);
            //Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);

            if (Random.value < 0.3 && !Bonuses.IsBonusShield)
            {
                Vector2 pos = new Vector2(cablePosition[cableID].x, 6);
                Instantiate(ShieldBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            }
            else if (Random.value < 0.4 && !Bonuses.isBonusX2Coins)
            {
                Vector2 pos = new Vector2(cablePosition[cableID].x, 6);
                Instantiate(X2CoinBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            }
            else if (Random.value < 0.5 && !Bonuses.isBonusX2Score)
            {
                Vector2 pos = new Vector2(cablePosition[cableID].x, 6);
                Instantiate(X2ScoreBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            }
            else
            {

                if (cableAvailable[cableID])
                {
                    if (!isJ1) StartCoroutine(J1(cableID));
                    else if (!isJ2) StartCoroutine(J2(cableID));
                }
            }
               
        }
    }
    bool isJ1 = false;
    bool isJ2 = false;
    IEnumerator J1(int cableID)
    {
        isJ1 = true;
        cableAvailable[cableID] = false;
        int id = 0;
        if (cableID == 3)
        {
            cableAvailable[2] = false;
            id = 2;
        }
        else if (cableID == 0)
        {
            cableAvailable[1] = false;
            id = 1;
        }
        else
        {
            if (Random.value > 0.5)
            {
                cableAvailable[cableID - 1] = false;
                id = cableID - 1;
            }
            else
            {
                cableAvailable[cableID + 1] = false;
                id = cableID + 1;
            }
        }

        Vector2 pos = new Vector2(cablePosition[cableID].x, 6);

        Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);

        yield return new WaitForSeconds(0.15f);
        for(int i = 0; i < 5; i++)
        {
            if (Bonuses.isBonusX2Coins)
            {
                Instantiate(DoubleCoinBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            }
            else
            {
                Instantiate(CoinBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            }
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.08f);
        Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
        yield return new WaitForSeconds(0.10f);
        cableAvailable[cableID] = true;

        isJ1 = false;

        yield return new WaitForSeconds(0.25f);
        cableAvailable[id] = true;
    }


    IEnumerator J2(int cableID)
    {
        isJ2 = true;
        cableAvailable[cableID] = false;
        int id = 0;
        if (cableID == 3)
        {
            cableAvailable[2] = false;
            id = 2;
        }
        else if (cableID == 0)
        {
            cableAvailable[1] = false;
            id = 1;
        }
        else
        {
            if (Random.value > 0.5)
            {
                cableAvailable[cableID - 1] = false;
                id = cableID - 1;
            }
            else
            {
                cableAvailable[cableID + 1] = false;
                id = cableID + 1;
            }
        }

        Vector2 pos = new Vector2(cablePosition[cableID].x, 6);

        Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);

        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(CoinBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.08f);
        Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
        yield return new WaitForSeconds(0.15f);
        for (int i = 0; i < 5; i++)
        {
            Instantiate(CoinBonus, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
            yield return new WaitForSeconds(0.07f);
        }
        yield return new WaitForSeconds(0.08f);
        Instantiate(NodePrefab, pos, Quaternion.Euler(0, 0, 0), objectsCables[cableID].transform);
        yield return new WaitForSeconds(0.10f);
        cableAvailable[cableID] = true;

        isJ2 = false;

        yield return new WaitForSeconds(0.25f);
        cableAvailable[id] = true;
    }


    public Vector2 GetRightCablePosition()
    {
        if (electronPosition + 1 >= cablePosition.Length) return cablePosition[electronPosition];
        else
        {
            objectsCables[electronPosition].transform.GetComponent<Cable>().PlayRepultionAnimation(1f);
            electronPosition++;

            return cablePosition[electronPosition];
        }
    }

    public Vector2 GetLeftCablePosition()
    {
        if (electronPosition - 1 < 0) return cablePosition[electronPosition];
        else
        {
            objectsCables[electronPosition].transform.GetComponent<Cable>().PlayRepultionAnimation(-1f);
            electronPosition--;

            return cablePosition[electronPosition];
        }
    }

    public Vector2 GetCurrentCablePosition()
    {
        return objectsCables[electronPosition].transform.GetChild(0).transform.position;
    }

    public void PlayAnimationLanding(float sign)
    {

        objectsCables[electronPosition].transform.GetComponent<Cable>().PlayLandingAnimation(sign);
    }


}
