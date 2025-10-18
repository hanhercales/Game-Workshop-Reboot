using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance{ get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpFx;
    [SerializeField] private AudioClip attackFx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void RunJumpFx()
    {
        audioSource.PlayOneShot(jumpFx);
    }

    public void RunAttackFx()
    {
        audioSource.PlayOneShot(attackFx);
    }
}
