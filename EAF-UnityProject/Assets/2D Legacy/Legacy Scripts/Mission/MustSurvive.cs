using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MustSurvive : MonoBehaviour
{
    Flyer flyerSC;

    void Start()
    {
        flyerSC = GetComponent<Flyer>();
    }

    void Update()
    {
        if (!flyerSC.Alive)
        {
            Mission.MissionFailed = true;
        }
    }
}
