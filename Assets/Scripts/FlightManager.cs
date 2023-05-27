using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
public class FlightManager : MonoBehaviour
{
    public static FlightManager Instance;
    [SerializeField] private List<PathManager> flightPaths;
     private float _timeRemaing;
    public bool startUpdate;
    private string NextPLaneString;
    public Sprite[] planeSprites;
    public float TimeRemaining
    {
        get
        {
            return _timeRemaing;
        }
        set
        {
            _timeRemaing = value;
            UIManager.Instance.timerTExt.text = Mathf.Floor(_timeRemaing).ToString() + " SEC";
            UIManager.Instance.timerTExt1.text = Mathf.Floor(_timeRemaing).ToString() + " SEC";
        }
    }
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        TimeRemaining = 30;
        GameManager.Instance.GameStateChange.AddListener(StateChange);
    }

    private void StateChange(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                startUpdate = true;
                NextPlane();
                break;
            case GameStates.Pause:
                startUpdate = false;
                break;
            case GameStates.Failed:
                startUpdate = false;
                break;
            case GameStates.Win:
                startUpdate = false;
                break;
        }
    }
    private void Update()
    {
        if (!startUpdate)
            return;
        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0)
        {
            TimeRemaining = 30;
            SpawnPlane();
        }
    }
    void SpawnPlane()
    {
        
        var go = PoolingSystem.Instance.GetPooledItem(NextPLaneString);


        PlaneBehaviour pb = go.GetComponent<PlaneBehaviour>();
        pb.movmentBehaviour.pathContainer = flightPaths[Random.Range(0, flightPaths.Count)];
        go.transform.position = pb.movmentBehaviour.pathContainer.waypoints[0].transform.position;
        
        go.SetActive(true);
        pb.movmentBehaviour.StartMove();
        NextPlane();
    }
    private void NextPlane()
    {
        int maxNpc = PoolingSystem.Instance.GetMatchingCount(GameUtils.Instance.PlanePreFix);
        int randomNpc = Random.Range(1, maxNpc + 1);
        var go = PoolingSystem.Instance.GetPooledItem($"{GameUtils.Instance.PlanePreFix}{randomNpc}");
        NextPLaneString= $"{GameUtils.Instance.PlanePreFix}{randomNpc}";
        switch (NextPLaneString)
        {
            case "Plane_1":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[0]);
                break;
            case "Plane_2":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[1]);

                break;
            case "Plane_3":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[2]);

                break;
            case "Plane_4":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[3]);

                break;
            case "Plane_5":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[4]);

                break;
            case "Plane_6":
                UIManager.Instance.NextPLaneSpriteChange(planeSprites[5]);

                break;
        }

    }
}

public enum Strength
{
    Max,
    Medium,
    Low
}