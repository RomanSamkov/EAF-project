using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultyBtn : MonoBehaviour
{
    public Button btn;
    public Text difficulty_name;
    public Text difficulty_discr;

    public string EasyName;
    public string MidName;
    public string DifName;

    public string EasyDiscr;
    public string MidDiscr;
    public string DifDiscr;

    // Start is called before the first frame update
    void Start()
    {
        UpdateInfo();
    }

    public void ChangeDifficult()
    {
        switch (DifficultyLevels.DifficultyLevel)
        {
            case (3):
                {
                    DifficultyLevels.DifficultyLevel = 1;
                    break;
                }
            case (2):
                {
                    DifficultyLevels.DifficultyLevel = 3;
                    break;
                }
            case (1):
                {
                    DifficultyLevels.DifficultyLevel = 2;
                    break;
                }
        }
        UpdateInfo();
    }

    void UpdateInfo()
    {
        if (DifficultyLevels.DifficultyLevel == 3)
        {
            difficulty_name.text = DifName;
            difficulty_name.color = Color.red;
            difficulty_discr.text = DifDiscr;
        }
        if (DifficultyLevels.DifficultyLevel == 2)
        {
            difficulty_name.text = MidName;
            difficulty_name.color = Color.yellow;
            difficulty_discr.text = MidDiscr;
        }
        if (DifficultyLevels.DifficultyLevel == 1)
        {
            difficulty_name.text = EasyName;
            difficulty_name.color = Color.green;
            difficulty_discr.text = EasyDiscr;
        }
    }
}
