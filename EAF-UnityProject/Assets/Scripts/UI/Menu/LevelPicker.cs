using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelPicker : MonoBehaviour
{
    public void PlayLvl1()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void PlayLvL2()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void PlayLvL3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void PlayLvL4()
    {
        SceneManager.LoadScene("Level 4");
    }

    public void PlayLvL5()
    {
        SceneManager.LoadScene("Level 5");
    }

    public void PlaySurvival()
    {
        SceneManager.LoadScene("Survival");
    }
}
