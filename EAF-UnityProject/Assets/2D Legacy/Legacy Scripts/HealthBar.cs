using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    private GameObject playerGO;
    private Flyer playerSC;

    public Image FillSC;

    private void Awake()
    {
        if (DifficultyLevels.DifficultyLevel <= 2) { gameObject.SetActive(true); }
        if (DifficultyLevels.DifficultyLevel == 3) { gameObject.SetActive(false); }
    }

    private void Start()
    {
        playerGO = GameObject.Find("Player");
        playerSC = playerGO.GetComponent<Flyer>();

        if(playerSC.Race == "pgs")
        {
            FillSC.color = Color.red;
        }

        if (playerSC.Race == "chn")
        {
            FillSC.color = Color.green;
        }
    }

    public void SetMaxHealth(int Health)
    {
        slider.maxValue = Health;
        slider.value = Health;
    }

    public void SetHealth(int Health)
    {
        slider.value = Health;
    }
}
