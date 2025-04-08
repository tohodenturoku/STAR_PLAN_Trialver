using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEntityChecker : MonoBehaviour
{
    [SerializeField] private int Num = 0;
    [HideInInspector] public bool P = false;
    [HideInInspector] public bool E = false;
    public float f1(){
        return P?Num:0;
    }
    public float f2(){
        return E?Num:0;
    }
}
