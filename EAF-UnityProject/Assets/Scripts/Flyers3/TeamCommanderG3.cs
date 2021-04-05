using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCommanderG3 : MonoBehaviour
{
    //This is the script to set collective behavior to multiple flyers in the controlable team;
    public string[] TeamsUnderControl;
    public string CurrentTactic;

    //AllTacticsNames
    /*
     * standart
     * fist
     * meeting
     * defense
     */

    protected GameObject[] flyersUnderControl;

    protected int enemyTeamPower;
    protected int ourTeamPower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
