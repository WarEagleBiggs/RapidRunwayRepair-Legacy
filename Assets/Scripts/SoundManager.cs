using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioClip buttonSound;
    public AudioSource Buttonsource;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void PlayButtonAudio()
    {
        Buttonsource.PlayOneShot(buttonSound);
    }
}
