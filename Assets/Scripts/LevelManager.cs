using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public List<LevelInfo> Levels = new List<LevelInfo>();
    public static LevelManager Instance;

    public int TotalNoOfLives;
    public int NoOfLives;
    private int _noOfPlanesLeft;
    private int NoOfPlanes
    {
        get
        {
            return _noOfPlanesLeft;
        } 
        set
        {
            _noOfPlanesLeft = value;
            UIManager.Instance.NoofAirCraft.text = _noOfPlanesLeft.ToString();
        }
    }
    private int _levelno;
    private int TotalNoOFLevelPlayed;
    public int LevelNo
    {
        get
        {
            return _levelno;
        }
        set
        {
            _levelno = value;
            
            UIManager.Instance.LevelText.text =  (_levelno+1).ToString();

        }
    }
    private void Awake()
    {
        Instance = this;
        if (PlayerPrefs.HasKey("TotalNoOfLevelPlayed"))
        {
            TotalNoOFLevelPlayed = PlayerPrefs.GetInt("TotalNoOfLevelPlayed");
        }
        else
        {
            PlayerPrefs.SetInt("TotalNoOfLevelPlayed", 0);
            TotalNoOFLevelPlayed = 0;
        }
    }
    private void StateChange(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                
                if (PlayerPrefs.HasKey("Level"))
                {
                    LevelNo = PlayerPrefs.GetInt("Level");

                }
                else
                {
                    PlayerPrefs.SetInt("Level", 0);
                    LevelNo = 0;

                }
                NoOfPlanes = Levels[LevelNo].NoOfPlanesToCome;
                NoOfLives = Levels[LevelNo].NoOfLives;
                TotalNoOfLives = NoOfLives;
                UIManager.Instance.LivesSprite(NoOfLives, TotalNoOfLives);

                break;
            case GameStates.Pause:
                break;
            case GameStates.Failed:
                break;
            case GameStates.Win:
                break;
        }
    }
    public void ReduceLife()
    {
        NoOfLives--;
        UIManager.Instance.LivesSprite(NoOfLives, TotalNoOfLives);
        if (NoOfLives <= 0)
        {
            UIManager.Instance.GamePlayControls.SetActive(false);
            UIManager.Instance.LoosePanel.SetActive(true);
            GameManager.Instance.gameStates = GameStates.Failed;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(StateChange);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TotalAircraftCounter()
    {
        NoOfPlanes--;
        LevelNo++;
        if (NoOfPlanes <= 0)
        {
            UIManager.Instance.LeveLEndStar(NoOfLives,TotalNoOfLives);
            UIManager.Instance.GamePlayControls.SetActive(false);
            UIManager.Instance.WinPanel.SetActive(true);
            GameManager.Instance.gameStates = GameStates.Win;
            if (LevelNo > TotalNoOFLevelPlayed)
            {
                PlayerPrefs.SetInt("TotalNoOfLevelPlayed", LevelNo);
            }
        }
    }

}
[System.Serializable]
public class LevelInfo
{
    public int NoOfLives;
    public int NoOfPlanesToCome;
}