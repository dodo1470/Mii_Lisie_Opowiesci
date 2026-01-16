using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using System;

public class flameaudio : MonoBehaviour
{
    [Header("FLAME")]
    [SerializeField] private AudioClip flame;



    private AudioSource source;



    void Awake()
    {
        source = GetComponent<AudioSource>();
        source.clip = flame;
        source.volume = 0.1f;
        source.Play();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


}
