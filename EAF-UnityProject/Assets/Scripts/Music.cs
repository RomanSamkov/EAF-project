using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioSource audioSource;
    public static float musicTime;

    
    void Awake()
    {
        audioSource.time = musicTime;
    }

    // Update is called once per frame
    void Update()
    {
        musicTime = audioSource.time;
    }
}
