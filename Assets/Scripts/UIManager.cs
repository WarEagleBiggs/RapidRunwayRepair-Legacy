using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject PatchSelectionPanel;
    public TextMeshProUGUI timerTExt;
    public TextMeshProUGUI timerTExt1;
    public TextMeshProUGUI NoofAirCraft;
    public Image PlayerHealthFillerImage;
    public GameObject WinPanel;
    public GameObject LoosePanel;
    public GameObject GamePlayControls;
    public GameObject MainMenu;
    public List<LevelSelectionButton> buttons;
    public Sprite Selected;
    public Sprite UnSelected;
    public Image[] LevelCompleteStars;
    public Sprite LevelCompleteStarGoldSprite;
    public Image nextPlaneSprite;
    public Button[] SelectionButtons;
    public TextMeshProUGUI LevelText;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(StateChange);
        checkLevel();
    }

    private void StateChange(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                break;
            case GameStates.Pause:
                break;
            case GameStates.Failed:
                break;
            case GameStates.Win:
                break;
        }
    }
    public void LivesSprite(int noOfLivesRemaining, int TotalNumberOFLives)
    {
        float fillAmount = (float) noOfLivesRemaining / TotalNumberOFLives;
        PlayerHealthFillerImage.fillAmount = fillAmount;
    }
    public void PatchSelection()
    {
        PatchSelectionPanel.SetActive(true);
    }
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void checkLevel()
    {
        foreach (LevelSelectionButton lsb in buttons)
        {
            lsb.CheckImage();
        }
    }
    public void LevelSelection()
    {
        if (EventSystem.current.currentSelectedGameObject.transform.parent.gameObject.GetComponent<LevelSelectionButton>().buttonState == StateOfButton.Unloacked)
        {
            string LevelNo = EventSystem.current.currentSelectedGameObject.transform.parent.name;
            int level = Int32.Parse(LevelNo);
            PlayerPrefs.SetInt("Level", level);
        }
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i == PlayerPrefs.GetInt("Level"))
            {
                buttons[i].button.image.sprite = Selected;
            }
            else
            {
                buttons[i].button.image.sprite = UnSelected;

            }
        }
        ResetButton();

    }
    public void ResetButton()
    {
        PlayerPrefs.SetInt("Reset", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LeveLEndStar(float noOfRemainingLives, float NoOfTotalLives)
    {
        float answer = noOfRemainingLives / NoOfTotalLives;
        if (answer< 0.3f)
        {
            LevelCompleteStars[0].sprite = LevelCompleteStarGoldSprite;
        }
        else if (answer<0.7f)
        {
            LevelCompleteStars[0].sprite = LevelCompleteStarGoldSprite;
            LevelCompleteStars[1].sprite = LevelCompleteStarGoldSprite;
        }
        else
        {
            LevelCompleteStars[0].sprite = LevelCompleteStarGoldSprite;
            LevelCompleteStars[1].sprite = LevelCompleteStarGoldSprite;
            LevelCompleteStars[2].sprite = LevelCompleteStarGoldSprite;
        }
        
    }
    public void NextPLaneSpriteChange(Sprite plane)
    {
        nextPlaneSprite.sprite = plane;
    }
    public void interactableWrapperMethod()
    {
        Invoke(nameof(Interactable), 0.2f);
    }
    public void UNinteractableWrapperMethod()
    {
        Invoke(nameof(UnInteractable), 0.2f);
    }
    public void Interactable()
    {
        foreach (Button b in SelectionButtons)
        {
            b.interactable = true;
        }
    }
    public void UnInteractable()
    {
        foreach (Button b in SelectionButtons)
        {
            b.interactable = false;
        }
    }
    public void NextLevelButton()
    {
        int levelPlaying = PlayerPrefs.GetInt("Level");
        if (levelPlaying < LevelManager.Instance.Levels.Count - 1)
        {
            levelPlaying++;
            PlayerPrefs.SetInt("Level", levelPlaying);
        }
        else
        {
            levelPlaying = 0;
            PlayerPrefs.SetInt("Level", levelPlaying);
        }
        ResetButton();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
