using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWS;
using DG.Tweening;
public class JeepBehaviour : MonoBehaviour
{
    public splineMove movementBehaviour;
    public PatchToCarry patch;
    [SerializeField] private Sprite[] spriteToChange;
    public GameObject Soldier;
    private List<GameObject> Soldiers = new List<GameObject>();
    public float timeToRepair;
    public AudioSource JeepAudio;
    public bool AfterRepair;
    public PathManager path;
    public bool isRuning;
    public DamagePatch repairingPatch;
    public DamagePatch patchtoBeRepaired;
    public GameObject DamageCar;
    public int noOfPlaneCanPassedAfterRepair;
    public int NoOfRepairsCanBeMade;
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
                break;
            case GameStates.Pause:
                break;
            case GameStates.Failed:
                break;
            case GameStates.Win:
                break;
        }
    }
    public void PLayAudio()
    {
        JeepAudio.Play();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("DamagePatch"))
        {
            DamagePatch dp = collision.gameObject.GetComponent<DamagePatch>();
            if (dp.isCallForHelp && !dp.Repaired && dp.path == path)
            {
                isRuning = false;
                movementBehaviour.Pause();
                JeepAudio.Stop();
                foreach (Transform repairPoint in dp.RepairPoints)
                {
                    GameObject go = Instantiate(Soldier, transform.position, transform.rotation);
                    Soldiers.Add(go);
                    go.transform.DOMove(repairPoint.position, 1f);

                }
                patchtoBeRepaired = null;
                dp.PlayDustParticle(timeToRepair);
                StartCoroutine(ChangePatch(collision.gameObject.GetComponent<SpriteRenderer>()));
                if (dp.patchRepaired != patch)
                {

                    dp.noOFRepairCAnBEMAde = NoOfRepairsCanBeMade;
                 }
                dp.noOfPlaneCanPassedAfterRepair = noOfPlaneCanPassedAfterRepair;

            }
        }
        if (collision.gameObject.CompareTag("Border") && AfterRepair)
        {
            Invoke("TurnOff", 3f);
        }

    }
    public void Destroy()
    {
        GameObject blast = PoolingSystem.Instance.GetPooledItem(GameUtils.Instance.blastPrefabName);
        blast.transform.position = transform.position;
        blast.SetActive(true);
        movementBehaviour.Pause();
        JeepAudio.Stop();
        GameObject DC = Instantiate(DamageCar, transform.position, transform.rotation);
        foreach (GameObject go in Soldiers)
        {
            Destroy(go);
        }
        if (repairingPatch != null)
        {
          
            repairingPatch.Repaired = true;
            repairingPatch.patchRepaired = patch;
            repairingPatch.StopDustParticle();
            foreach (Sprite DamagerPatch in spriteToChange)
            {
                if (DamagerPatch.name == repairingPatch.gameObject.GetComponent<SpriteRenderer>().sprite.name)
                {
                    repairingPatch.gameObject.GetComponent<SpriteRenderer>().sprite = DamagerPatch;
                }
            }
            repairingPatch._reDamagePatch();
            repairingPatch.fillerImage.gameObject.SetActive(false); ;
        }
        if (patchtoBeRepaired != null)
        {
            if (isRuning)
            {
                patchtoBeRepaired.Cancel();
            }
            patchtoBeRepaired = null;
        }
        this.gameObject.SetActive(false);
        
        
    }
    private void OnDisable()
    {
        Soldiers.Clear();
        movementBehaviour.pathContainer = null;

    }
    // Update is called once per frame
    void Update()
    {

    }
    void TurnOff()
    {
        AfterRepair = false;
        movementBehaviour.reverse = false;
        this.gameObject.SetActive(false);
        foreach (GameObject go in Soldiers)
        {
            Destroy(go);
        }
        Soldiers.Clear();
    }
    
    public IEnumerator ChangePatch(SpriteRenderer spriteRenderer)
    {
        repairingPatch = spriteRenderer.gameObject.GetComponent<DamagePatch>();
        yield return new WaitForSeconds(timeToRepair);
        foreach (Sprite DamagerPatch in spriteToChange)
        {
            if (DamagerPatch.name == spriteRenderer.sprite.name)
            {
                spriteRenderer.gameObject.GetComponent<DamagePatch>().Repaired = true;
                spriteRenderer.gameObject.GetComponent<DamagePatch>().patchRepaired = patch;
                spriteRenderer.gameObject.GetComponent<DamagePatch>().StopDustParticle();
                spriteRenderer.sprite = DamagerPatch;
                foreach (GameObject go in Soldiers)
                {
                    go.transform.DOMove(transform.position, 1f);
                }
                yield return new WaitForSeconds(1f);
               
                spriteRenderer.gameObject.GetComponent<DamagePatch>()._reDamagePatch();
                foreach (GameObject go in Soldiers)
                {
                    Destroy(go);
                }
                repairingPatch = null;
                Soldiers.Clear();
                JeepAudio.Play();
                movementBehaviour.Reverse();
                AfterRepair = true;
            }
        }
    }
}
