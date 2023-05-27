using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static GameUtils Instance;
    public string PlanePreFix;
    public string DamagePatchPrefix;
    public string PerforatedSteelPrefabName;
    public string PerforatedAluminumPrefabName;
    public string CementPrefabName;
    public string CompactSoilPrefabName;
    public string blastPrefabName;
    private void Awake()
    {
        Instance = this;
    }
   
}
