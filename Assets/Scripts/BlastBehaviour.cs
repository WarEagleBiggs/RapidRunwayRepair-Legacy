using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        StartCoroutine(turnoffRoutine());
    }
    IEnumerator turnoffRoutine()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
