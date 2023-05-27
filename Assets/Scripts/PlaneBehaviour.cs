using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
using System;
using DG.Tweening;
public class PlaneBehaviour : MonoBehaviour
{

    public Strength strenghtOfAirCraft;
    public splineMove movmentBehaviour;
    public bool isCrashed;
    public bool colidedOnetime;
    public AudioSource planeAudio;
    private Vector3 tempScale;
    // Start is called before the first frame update
    
    void Start()
    {
        tempScale = transform.localScale;
        transform.DOScale(new Vector3(transform.localScale.x + 0.5f, transform.localScale.x + 0.5f, transform.localScale.z), 0.2f);
        GameManager.Instance.GameStateChange.AddListener(StateChange);
    }
    private void OnEnable()
    {
        planeAudio.Play();

        transform.rotation = Quaternion.Euler(0,0,-180);
        Debug.Log(transform.rotation);
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
    public void CameraShake()
    {
        Camera.main.transform.DOShakePosition(1f,0.3f);
        //transform.DOShakePosition(1f);
        //movmentBehaviour.Stop();
        isCrashed = true;
        colidedOnetime = true;
        //StartCoroutine(PlaneDisableRoutine());
        //SpawnManager.Instance.invokeSpawnDamage();

    }
    IEnumerator PlaneDisableRoutine()
    {

        yield return new WaitForSeconds(2f);
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
    void CheckPlane(DamagePatch dp)
    {
        dp.noOfPlanePassedAfterRepair++;
        switch (strenghtOfAirCraft)
        {
            
            case Strength.Max:
                if (dp.patchRepaired != PatchToCarry.PerforatedSteel)
                {
                    LevelManager.Instance.ReduceLife();
                    CameraShake();
                    Debug.Log("Heavy");
                }
                break;
            case Strength.Medium:
                if (dp.patchRepaired == PatchToCarry.CompactedSoil)
                {
                    LevelManager.Instance.ReduceLife();
                    CameraShake();
                    Debug.Log("Heavy");

                }
                break;
            case Strength.Low:

                break;
        }
        if (dp.noOfPlanePassedAfterRepair >= dp.noOfPlaneCanPassedAfterRepair)
        {
            dp.Damage();
        }
    }
    private void OnDisable()
    {
        isCrashed = false;
        colidedOnetime = false;
        transform.DOScale(new Vector3(transform.localScale.x + 0.2f, transform.localScale.x + 0.2f, transform.localScale.z), 0.2f);

        planeAudio.Stop();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Border"))
        {
            SpawnManager.Instance.invokeSpawnDamage();
            LevelManager.Instance.TotalAircraftCounter();
            this.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("DamagePatch"))
        {
            if (colidedOnetime)
                return;
            DamagePatch dp = collision.gameObject.GetComponent<DamagePatch>();
            if (!dp.Repaired)
            {
                
                LevelManager.Instance.ReduceLife();
                CameraShake();
            }
            else
            {
                Debug.Log("Check Repaired");
                CheckPlane(dp);

            }

        }
        if (collision.gameObject.CompareTag("PlaneSensor"))
        {
            transform.DOScale(tempScale, 6f);
        }

    }
}
