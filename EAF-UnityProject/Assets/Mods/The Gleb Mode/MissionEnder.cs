using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionEnder : MonoBehaviour
{
    public int EnemiesNum = 0;

    public GameObject EndSreen;

    void ChechForAnyEnemiesLeft()
    {
        EnemiesNum = 0;

        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");

        foreach (GameObject flyer in flyers)
        {
            TeamSeparatorG3 teamSeparator = flyer.GetComponent<TeamSeparatorG3>();

            if (teamSeparator.Team == "enemy")
            {
                EnemiesNum++;
            } 
        }

        if (EnemiesNum == 0)
        {
            EndSreen.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ChechForAnyEnemiesLeft();
    }
}
