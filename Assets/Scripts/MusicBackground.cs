using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
