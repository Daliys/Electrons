using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpPanel : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject hpPanel;
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Game.ElectronHP != transform.childCount)
        {
            if(Game.ElectronHP < transform.childCount)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            else
            {
                Instantiate(hpPanel, transform);
            }
        }
    }
}
