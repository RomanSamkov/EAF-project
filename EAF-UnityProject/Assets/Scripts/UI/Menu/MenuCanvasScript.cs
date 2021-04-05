using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuCanvasScript : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayEafWBLvl1()
    {
        SceneManager.LoadScene("EAF WB lvl1");
    }

    public void PlayEafWBLvL2()
    {
        SceneManager.LoadScene("EAF WB lvl2");
    }

    public void PlayEafWBLvL3()
    {
        SceneManager.LoadScene("EAF WB lvl3");
    }

    public void PlayEafWBLvL4()
    {
        SceneManager.LoadScene("EAF WB lvl4");
    }

    public void PlayEafWBLvL5()
    {
        SceneManager.LoadScene("EAF WB lvl5");
    }

    public void PlayEafWBLvL6()
    {
        SceneManager.LoadScene("EAF WB lvl6");
    }


    public void PlayCafFLvl1()
    {
        SceneManager.LoadScene("CAF F lvl1");
    }

    public void PlayCafFLvl2()
    {
        SceneManager.LoadScene("CAF F lvl2");
    }

    public void PlayCafFLvl3()
    {
        SceneManager.LoadScene("CAF F lvl3");
    }

    public void PlayCafFLvl4()
    {
        SceneManager.LoadScene("CAF F lvl4");
    }

    public void PlayCafFLvl5()
    {
        SceneManager.LoadScene("CAF F lvl5");
    }


    public void PlaySurvival()
    {
        SceneManager.LoadScene("Survival");
    }
}
