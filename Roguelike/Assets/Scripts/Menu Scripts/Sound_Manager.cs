using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    private static Sound_Manager instance;
    public AudioSource[] musicSources;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (AudioSource source in musicSources)
            {
                DontDestroyOnLoad(source.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
