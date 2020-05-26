using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackground : MonoBehaviour
{
    public AudioSource introMusic, loopMusic;

    // Start is called before the first frame update
    void Awake()
    {
        introMusic.Play();
        

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        loopMusic.PlayDelayed(introMusic.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
