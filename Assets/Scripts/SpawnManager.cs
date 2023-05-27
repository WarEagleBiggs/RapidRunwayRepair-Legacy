using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
using DG.Tweening;
public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;
    [SerializeField] private List<PathManager> paths;
    private PathManager pathSelcted;
    public DamagePatch patchSelected;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChange.AddListener(StateChange);

    }

    private void StateChange(GameStates state)
    {
        switch (state)
        {
            case GameStates.Playing:
                CancelInvoke();
                Invoke(nameof(SpawnDamagePatch), 3f);
                break;
            case GameStates.Pause:
                break;
            case GameStates.Failed:
                break;
            case GameStates.Win:
                break;
        }
    }
    public void invokeSpawnDamage()
    {
        Invoke(nameof(SpawnDamagePatch), 1f);

    }
    void CameraShake()
    {
        Camera.main.transform.DOShakePosition(1f);

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnDamagePatch()
    {

        int maxNpc = PoolingSystem.Instance.GetMatchingCount(GameUtils.Instance.DamagePatchPrefix);
        int randomNpc = Random.Range(1, maxNpc + 1);
        var go = PoolingSystem.Instance.GetPooledItem($"{GameUtils.Instance.DamagePatchPrefix}{randomNpc}");
        //var go = PoolingSystem.Instance.GetPooledItem("DamagePatch_8");
        int randomNumber = Random.Range(0, paths.Count);
        PathManager pm = paths[randomNumber];
        int waypointIndex = pm.GetWaypointCount();
        go.transform.position = pm.waypoints[waypointIndex - 1].transform.position;
        go.SetActive(true);
        go.GetComponent<DamagePatch>().AssignPath(pm);

        CameraShake();
        GameObject blast = PoolingSystem.Instance.GetPooledItem(GameUtils.Instance.blastPrefabName);
        blast.transform.position = go.transform.position;
        blast.SetActive(true);
        paths.Remove(pm);
    }
    public void PathSelected(PathManager path)
    {
        pathSelcted = path;
    }
    public void PerforatedSteel()
    {
        Debug.Log("ButtonPressed Steel");

        SendHelp(GameUtils.Instance.PerforatedSteelPrefabName, pathSelcted);
    }
    public void PerforatedAluminium()
    {
        Debug.Log("ButtonPressed Aluminum");

        SendHelp(GameUtils.Instance.PerforatedAluminumPrefabName, pathSelcted);
    }
    public void Cement()
    {
        Debug.Log("ButtonPressed Cement");

        SendHelp(GameUtils.Instance.CementPrefabName, pathSelcted);
    }
    public void CompactSoil()
    {
        Debug.Log("ButtonPressed Soil");
        SendHelp(GameUtils.Instance.CompactSoilPrefabName, pathSelcted);
    }
    public void SendHelp(string PatchName, PathManager path )
    {
        GameObject go = PoolingSystem.Instance.GetPooledItem(PatchName);
        JeepBehaviour jb = go.GetComponent<JeepBehaviour>();
        jb.movementBehaviour.pathContainer = path;
        go.transform.position = path.waypoints[0].transform.position;
        go.SetActive(true);
        jb.movementBehaviour.StartMove(); 
        jb.PLayAudio();
        jb.path = path;
        jb.isRuning = true;
        jb.patchtoBeRepaired = patchSelected;
        Invoke(nameof(PatchSlectionPanelStop), 0.5f);
        UIManager.Instance.UNinteractableWrapperMethod();
    }
    void PatchSlectionPanelStop()
    {
        UIManager.Instance.PatchSelectionPanel.SetActive(false);

    }
    public void CrossButton()
    {
        patchSelected.Cancel();
        patchSelected = null;
    }
}
public enum PatchToCarry
{
    PerforatedSteel,
    PerforatedAluminum,
    Cement,
    CompactedSoil,
    None
}