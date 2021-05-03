using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerG3 : MonoBehaviour
{
    //Ётот класс создан дл€ того что бы хранить стандартные функции дл€ всех подконтрольных летунов
    private FlyerG3 flyer;

    public void Awake()
    {
        flyer = GetComponent<FlyerG3>();
    }

    protected List<GameObject> flyersListPreviousState = new List<GameObject>();
    protected List<GameObject> arrowOwners = new List<GameObject>();
    protected List<GameObject> arrows = new List<GameObject>();

    public virtual void StartInstantiateArrows(TeamSeparatorG3 teamSeparator, GameObject arrowPF)
    {
        GameObject[] flyersArray = GameObject.FindGameObjectsWithTag("Flyers");

        foreach (GameObject flyer in flyersArray)
        {
            if (flyer != gameObject)
            {
                arrowOwners.Add(flyer);
                var newArrow = Instantiate(arrowPF, gameObject.transform.position, gameObject.transform.rotation);
                newArrow.transform.parent = gameObject.transform;
                arrows.Add(newArrow);
                ArrowG3 arrowG3 = newArrow.GetComponent<ArrowG3>();
                arrowG3.Target = flyer;

                TeamSeparatorG3 selectedFlyerTeam = flyer.GetComponent<TeamSeparatorG3>();
                if (teamSeparator.EnemyTeamList.Contains(selectedFlyerTeam.Team))
                {
                    arrowG3.TargetRelation = "enemy";
                }
                else if (teamSeparator.Team == selectedFlyerTeam.Team || teamSeparator.FriendTeamList.Contains(selectedFlyerTeam.Team))
                {
                    arrowG3.TargetRelation = "friend";
                }
                else
                {
                    arrowG3.TargetRelation = "neutral";
                }

                arrowG3.DetectDistance = 50;
            }
        }

        List<GameObject> flyers = new List<GameObject>(flyersArray);
        flyersListPreviousState = flyers;
    }

    public virtual void RecalculateMoveSpeedChangeSpeed()
    {
        flyer.MoveSpeed = (flyer.MaxMoveSpeed + flyer.MinMoveSpeed) / 2;
        flyer.ChangeSpeed = (flyer.MaxMoveSpeed - flyer.MinMoveSpeed) / 2;
    }
}
