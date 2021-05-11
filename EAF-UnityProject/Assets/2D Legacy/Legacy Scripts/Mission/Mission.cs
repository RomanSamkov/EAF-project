using UnityEngine;

public class Mission : MonoBehaviour
{
    public static int KillCounter = 0;

    public bool ReachPointMission;
    public float ToPointDist;

    public bool KillAllEnemies;
    public string EnemyTeam;
    public int EnemyTeamCurrentNumber;
    public int EnemyTeamStartNumber;


    public bool SaveAllFriends;
    public string OurTeam;
    public int OurTeamCurrentNumber;
    public int OurTeamStartNumber;

    public static bool MissionFailed = false;

    private GameObject playerGO;
    private Transform playerTR;
    private Flyer playerSC;
    public GameObject MissionCompletePlane;
    public GameObject MissionFailedPlane;

    private float distance;

    private void Awake()
    {
        KillCounter = 0;
        MissionFailed = false;
    }

    void Start()
    {
        playerGO = GameObject.Find("Player");
        playerTR = playerGO.transform;
        playerSC = playerGO.GetComponent<Flyer>();

        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");
        foreach (GameObject flyer in flyers)
        {
            TeamSeparator team = flyer.GetComponent<TeamSeparator>();
            if (team.Team == EnemyTeam)
            {
                EnemyTeamStartNumber++;
            }
            if (team.Team == OurTeam)
            {
                OurTeamStartNumber++;
            }
        }
    }

    void Update()
    {
        EnemyTeamCurrentNumber = 0;
        OurTeamCurrentNumber = 0;

        GameObject[] flyers = GameObject.FindGameObjectsWithTag("Flyers");
        foreach (GameObject flyer in flyers)
        {
            Flyer flyerSC = flyer.GetComponent<Flyer>();
            if (flyerSC.Alive)
            {
                TeamSeparator team = flyer.GetComponent<TeamSeparator>();
                if (team.Team == EnemyTeam)
                {
                    EnemyTeamCurrentNumber++;
                }
                if (team.Team == OurTeam)
                {
                    OurTeamCurrentNumber++;
                }
            }
        }

        if (KillAllEnemies && EnemyTeamCurrentNumber == 0 && playerSC.Alive)
        {
            Time.timeScale = 0f;
            MissionCompletePlane.SetActive(true);
        }

        if(SaveAllFriends && OurTeamCurrentNumber != OurTeamStartNumber && playerSC.Alive)
        {
            SetMissionFailed();
        }

        if (ReachPointMission)
        {
            distance = Vector3.Distance(playerTR.position, transform.position);
            if (distance <= ToPointDist && playerSC.Alive)
            {
                Time.timeScale = 0f;
                MissionCompletePlane.SetActive(true);
            }
        }

        if (MissionFailed)
        {
            SetMissionFailed();
        }
    }

    public void SetMissionFailed()
    {
        Time.timeScale = 0f;
        MissionFailedPlane.SetActive(true);
    }
}
