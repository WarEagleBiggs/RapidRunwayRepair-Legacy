using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;

public class LevelSelectionButton : MonoBehaviour
{
    public StateOfButton buttonState;
    public Button button;
    public Image image;
    public GameObject lockedSprite;
    public Sprite Selecte;
    public void CheckImage()
    {
        if (PlayerPrefs.HasKey("TotalNoOfLevelPlayed"))
        {
            int level = PlayerPrefs.GetInt("TotalNoOfLevelPlayed");
            int buttonName =Int32.Parse(gameObject.name);
            if (level >= buttonName)
            {
                lockedSprite.SetActive(false);
                buttonState = StateOfButton.Unloacked;
            }
        }
        else
        {
            if (gameObject.name == "0")
            {
                lockedSprite.SetActive(false);
                buttonState = StateOfButton.Unloacked;
                button.image.sprite = Selecte;
            }
        }
      
    }
    
}

public enum StateOfButton
{
    Locked,
    Unloacked
    
}