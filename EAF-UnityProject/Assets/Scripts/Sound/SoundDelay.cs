using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundDelay : MonoBehaviour
{
    private float speedOfSound = 300;

    private Transform listenerTR;

    private AudioSource AudioSource;
    public float DistancePassed;
    public float DistanceToListener;
    public bool havePlayed;

    // Start is called before the first frame update
    void Start()
    {
        GameObject listenerGO = GameObject.Find("Camera1");
        listenerTR = listenerGO.transform;
        AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        SoundPassedStep(); 
    }

    void SoundPassedStep()
    {
        if (!havePlayed)
        {
            DistanceToListener = Vector3.Distance(listenerTR.position, transform.position);
            if (DistanceToListener - (DistancePassed + speedOfSound * Time.deltaTime) < 0)
            {
                AudioSource.Play();
                havePlayed = true;
            }
            else
            {
                DistancePassed += speedOfSound * Time.deltaTime;
            }
        }
    }
}

