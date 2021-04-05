using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounter : MonoBehaviour
{
    public Text ScoreSign;

    private void Start()
    {
        ScoreSign.GetComponent<Text>();
    }

    // Update is called once per frame
    /*
    void Update()
    {
        ScoreSign.text = PlayerPegasus.CurrentAmmo+"";
    }
    */

    public void SetInt(int i)
    {
        ScoreSign.text = ("" + i);
    }
}
