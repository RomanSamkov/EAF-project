using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScriptG3 : MonoBehaviour
{
    GameObject DeathPanelUIObject;

    private GameObject playerGO;
    private FlyerG3 playerSC;

    public GameObject PegasusController;
    public GameObject ChangelingController;
    public GameObject GriffonController;

    private void Awake()
    {
        DeathPanelUIObject = GameObject.Find("Death panel");

        playerGO = GameObject.Find("Player");
        playerSC = playerGO.GetComponent<FlyerG3>();

        if (playerSC.Race == "pgs")
        {
            PegasusController.SetActive(true);
        }

        if (playerSC.Race == "chn")
        {
            ChangelingController.SetActive(true);
        }

        if(playerSC.Race == "grf")
        {
            GriffonController.SetActive(true);
        }
    }
    void Start()
    {
        DeathPanelUIObject.SetActive(false);
        Time.timeScale = 0f;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
    }

    public void NextMission()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
