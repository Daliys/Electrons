using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator menu;
    private Animator Anim;
    public Slider SLIDER_FOR_TEST2;
    public bool soindA = true;
    public bool musicA = true;
    public bool langA = true;
    public void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void Sound()
    {
        if (soindA == true)
        {
            menu.Play("SoundOff");
            soindA = false;
        }
        else {
            menu.Play("SoundOn");
            soindA = true;
        }
    }

    public void Music()
    {
        if (musicA == true)
        {
            menu.Play("MusicOff");
            musicA = false;
        }
        else { menu.Play("MusicOn");
            musicA = true;
        }
    }

    public void Lang()
    {
        if (langA == true)
        {
            menu.Play("DoEng");
            langA = false;
        } else { menu.Play("DoRus");
            langA = true;
        }
    }

    public void ButtonSetting()
    {
        Anim.Play("SettingAnim");
    }
    public void QuestButton()
    {
        //c
        Anim.Play("QuestAnim");
        //Anim.SetInteger("State", 6);
    }

    public void ButtonElectron()
    {
        Anim.Play("ElectronsAnim");
    }

    public void AchievementButton()
    {
        Anim.Play("AchievementAnim");
    }

    public void ButtonElectronsBack()
    {
        Anim.Play("ElectronsBack");
    }

    public void ButtonSettingBack()
    {
        Anim.Play("SettingBack");
    }

    public void ButtonPlay()
    {
        //print("33333333333333334");
        Game.GameSpeed *= SLIDER_FOR_TEST2.value;
        Anim.Play("PlayButton");
      
    }

    public void CanvasClose()
    {
        Game.isGamePause = false;

        transform.parent.gameObject.SetActive(false);
        
    }
}
