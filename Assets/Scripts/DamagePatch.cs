using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using SWS;
public class DamagePatch : MonoBehaviour
{
    public PathManager path;
    public bool isCallForHelp;
    public Sprite StartSprite;
    public SpriteRenderer renderer;
    public bool Repaired;
    public PatchToCarry patchRepaired = PatchToCarry.None;
    public Transform[] RepairPoints;
    public GameObject DustParticle;
    public Image fillerImage;
    public Image fillerImage1;
    public GameObject CanvasObject;
    public GameObject CanvasObject1;
    public AudioSource audioSource;
    public AudioClip blastAudio;
    public AudioClip ContructionAudio;
    public GameObject Indicator;
    public int noOfPlanePassedAfterRepair;
    public int noOfPlaneCanPassedAfterRepair;
    public int noOFRepairCAnBEMAde;
    private Tween fillerTween;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.Instance.GameStateChange.AddListener(StateChange);
        DustParticle.SetActive(false);
        CanvasObject.SetActive(false);
        CanvasObject1.SetActive(false);
        fillerImage.fillAmount = 0;
        fillerImage1.fillAmount = 1;
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

    // Update is called once per frame
    void Update()
    {

    }
    public void AssignPath(PathManager _path)
    {
        audioSource.PlayOneShot(blastAudio);
        path = _path;
    }
    public void UnAssignPath()
    {
        path = null;
    }
    private void OnMouseDown()
    {
        if (!isCallForHelp)
        {
            if (path != null)
            {
                if (patchRepaired == PatchToCarry.None)
                {
                    isCallForHelp = true;
                    Debug.Log("Pressed");
                    SpawnManager.Instance.PathSelected(path);
                    SpawnManager.Instance.patchSelected = this;
                    UIManager.Instance.PatchSelection();
                    UIManager.Instance.interactableWrapperMethod();
                    Indicator.SetActive(false);
                }
                else
                {
                    isCallForHelp = true;
                    Debug.Log("Pressed");
                    SpawnManager.Instance.PathSelected(path);
                    SpawnManager.Instance.patchSelected = this;
                    
                    Indicator.SetActive(false);
                    switch (patchRepaired)
                    {
                        case (PatchToCarry.Cement):
                            SpawnManager.Instance.Cement();
                                break;
                        case (PatchToCarry.PerforatedSteel):
                            SpawnManager.Instance.PerforatedSteel();
                            break;
                        case (PatchToCarry.PerforatedAluminum):

                            SpawnManager.Instance.PerforatedAluminium();
                            break;
                        case (PatchToCarry.CompactedSoil):
                            SpawnManager.Instance.CompactSoil();
                            break;
                    }
                }

            }
            
        }

    }
    public void Cancel()
    {
        Indicator.SetActive(true);
        isCallForHelp = false;

    }
    public void PlayDustParticle(float time)
    {
        DustParticle.SetActive(true);
        CanvasObject.SetActive(true);
        fillerImage.DOFillAmount(1, time).OnComplete(() =>
        {

            CanvasObject.SetActive(false);
            fillerImage.fillAmount = 0;

        });
        audioSource.PlayOneShot(ContructionAudio);
    }
    void CameraShake()
    {
        Camera.main.transform.DOShakePosition(1f);
        audioSource.PlayOneShot(blastAudio);

    }
    public void _reDamagePatch()
    {
        StartCoroutine(ReDamagePatch());
    }
    IEnumerator ReDamagePatch()
    {
       
        switch (patchRepaired)
        {
            case PatchToCarry.PerforatedSteel:

                break;
            case PatchToCarry.Cement:
                CanvasObject1.SetActive(true);
             fillerTween=   fillerImage1.DOFillAmount(0, 10f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    CanvasObject1.SetActive(false);
                    fillerImage1.fillAmount = 1;
                    renderer.sprite = StartSprite;

                });
                yield return new WaitForSeconds(10f);
                noOFRepairCAnBEMAde--;
                if (noOFRepairCAnBEMAde == 0)
                {
                    patchRepaired = PatchToCarry.None;
                }
                Repaired = false;
                isCallForHelp = false;
                Indicator.SetActive(true);
                if (noOFRepairCAnBEMAde == 0)
                {
                    patchRepaired = PatchToCarry.None;
                }
                //GameObject blast = PoolingSystem.Instance.GetPooledItem(GameUtils.Instance.blastPrefabName);
                //blast.transform.position = transform.position;
                //blast.SetActive(true);
                //CameraShake();
                break;

            case PatchToCarry.PerforatedAluminum:
                CanvasObject1.SetActive(true);
                fillerTween = fillerImage1.DOFillAmount(0, 25f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    CanvasObject1.SetActive(false);
                    fillerImage1.fillAmount = 1;
                    renderer.sprite = StartSprite;

                });
                yield return new WaitForSeconds(25f);
                noOFRepairCAnBEMAde--;

                Repaired = false;
                isCallForHelp = false;
                Indicator.SetActive(true);
                if (noOFRepairCAnBEMAde == 0)
                {
                    patchRepaired = PatchToCarry.None;
                }
                //GameObject blast1 = PoolingSystem.Instance.GetPooledItem(GameUtils.Instance.blastPrefabName);
                //blast1.transform.position = transform.position;
                //blast1.SetActive(true);
                //CameraShake();
                break;
            case PatchToCarry.CompactedSoil:
                CanvasObject1.SetActive(true);
                fillerTween = fillerImage1.DOFillAmount(0, 5f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    CanvasObject1.SetActive(false);
                    fillerImage1.fillAmount = 1;
                    renderer.sprite = StartSprite;
                });
                yield return new WaitForSeconds(5f);
                noOFRepairCAnBEMAde--;

                Repaired = false;
                isCallForHelp = false;
                renderer.sprite = StartSprite;
                Indicator.SetActive(true);
                if (noOFRepairCAnBEMAde == 0)
                {
                    patchRepaired = PatchToCarry.None;
                }
                //GameObject blast2 = PoolingSystem.Instance.GetPooledItem(GameUtils.Instance.blastPrefabName);
                //blast2.transform.position = transform.position;
                //blast2.SetActive(true);
                //CameraShake();
                break;
                
        }
        

    }
    public void StopDustParticle()
    {
        DustParticle.SetActive(false);
        //isCallForHelp = false;
    }
    private void OnDisable()
    {
        //SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        //renderer.sprite = StartSprite;
    }
    public void Damage()
    {
        if (fillerTween != null)
            fillerTween.Kill();
        Repaired = false;
        isCallForHelp = false;
        renderer.sprite = StartSprite;
        Indicator.SetActive(true);
        CanvasObject1.SetActive(false);
        fillerImage1.fillAmount = 1;
        noOFRepairCAnBEMAde--;
        if (noOFRepairCAnBEMAde == 0)
        {
            patchRepaired = PatchToCarry.None;
        }
    }

}
