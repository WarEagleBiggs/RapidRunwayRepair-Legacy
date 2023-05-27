using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int _lives;
    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            _lives = value;
        }
    }
    private GameStates _gameStates;
    public GameStates gameStates
    {
        get
        {
            return _gameStates;
        }
        set
        {
            _gameStates = value;
            GameStateChange?.Invoke(_gameStates);
        }
    }
    public UnityEvent<GameStates> GameStateChange = new UnityEvent<GameStates>();
    public bool isFastForward;
    private void Awake()
    {
        gameStates = GameStates.Pause;
        Instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        Lives = 3;
        if (PlayerPrefs.HasKey("Reset"))
        {
            UIManager.Instance.MainMenu.SetActive(false);
            UIManager.Instance.GamePlayControls.SetActive(true);
            Play();
            PlayerPrefs.DeleteKey("Reset");
        }
    }

    public void FastForward()
    {
        isFastForward = !isFastForward;
        if (isFastForward)
        {
            Time.timeScale = 2f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void Play()
    {
        gameStates = GameStates.Playing;
    }
}
public enum GameStates
{
    Playing,
    Pause,
    Failed,
    Win
}