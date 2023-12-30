using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    public List<SoundSlider> soundSliders;
    public List<Sprite> muteSprites;

    public void OptionSwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
        SoundManager.instance.Play("UI/Eff_Button_Positive", SoundManager.Sound.Effect);
    }

    public void ChangeFill(int i)
    {
        soundSliders[i].fillImage.fillAmount = soundSliders[i].scrollBar.value;
    }

    public void MuteButton(int i)
    {
        soundSliders[i].mute = !soundSliders[i].mute;

        if (soundSliders[i].mute)
            soundSliders[i].muteImage.sprite = muteSprites[1];
        else
            soundSliders[i].muteImage.sprite = muteSprites[0];
    }
}

[System.Serializable]
public class SoundSlider
{
    public Image fillImage;
    public Scrollbar scrollBar;
    public Image muteImage;
    public Button muteButton;
    public bool mute;
}