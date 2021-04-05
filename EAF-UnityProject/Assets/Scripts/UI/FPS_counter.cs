using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_counter : MonoBehaviour
{
    public int avgFrameRate;
    public Text display_Text;

    private int currentSecondFrame = 0;
    private float currentSecondFracture = 0;

    public void Update()
    {
        currentSecondFracture += Time.deltaTime;
        if (currentSecondFracture > 1f)
        {
            currentSecondFracture = 0f;
            
        }

        currentSecondFrame++;


        if (currentSecondFracture == 0f)
        {
            display_Text.text = currentSecondFrame.ToString();
            currentSecondFrame = 0;
        }
    }

    /*
    float current = 0;
    current = Time.frameCount / Time.time;
    avgFrameRate = (int)current;
    display_Text.text = avgFrameRate.ToString() + " FPS";
    */
}
