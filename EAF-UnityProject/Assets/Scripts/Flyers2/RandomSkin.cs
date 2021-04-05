using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RandomSkin : MonoBehaviour
{
    Animator anim;

    public int TypesNumber;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        int i = Random.Range(1, TypesNumber+1);

        anim.SetInteger("Type", i);
    }
}
