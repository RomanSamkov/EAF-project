using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScore : MonoBehaviour
{
    public Text timer_text;

    private void Update()
    {
        timer_text.text = "Время выживания: " + Mathf.Round(SurvivalWaveSpawner.timer * 10.0f) * 0.1f + " с";
    }
}
