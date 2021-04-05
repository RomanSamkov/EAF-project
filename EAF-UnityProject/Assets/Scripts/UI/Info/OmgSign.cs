using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OmgSign : MonoBehaviour
{
    public bool SetInactiveOnStart;
    public bool IfKillAllOnWonderbolt;

    void Start()
    {
        if (SetInactiveOnStart) gameObject.SetActive(false);
        if (IfKillAllOnWonderbolt)
        {
            GameObject missionGO = GameObject.Find("Mission");
            Mission missionSC = missionGO.GetComponent<Mission>();
            if(missionSC.EnemyTeamCurrentNumber == 0 && DifficultyLevels.DifficultyLevel == 3)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
    }
}
