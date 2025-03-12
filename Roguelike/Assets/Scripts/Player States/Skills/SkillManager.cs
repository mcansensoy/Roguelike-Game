using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public DashSkill dash;
    public BasicSkill basicSkill;
    public UltimateSkill ultimateSkill;

    public AudioSource audioSource;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        dash = GetComponent<DashSkill>();
        basicSkill = GetComponent<BasicSkill>();
        ultimateSkill = GetComponent<UltimateSkill>();
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(AudioClip clip, float _volume)
    {
        if (audioSource != null && clip != null)
        {
            // Set random pitch between 0.9 and 1.1
            audioSource.volume = _volume;
            audioSource.pitch = Random.Range(0.85f, 1.15f);
            audioSource.PlayOneShot(clip);
        }
    }
}
