using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPhrasesList : MonoBehaviour
{
    public Text txt;

    public string[] stringArray;

    // Start is called before the first frame update
    void Start()
    {
        txt.text = stringArray[Random.Range(0, stringArray.Length-1)];
    }
}
