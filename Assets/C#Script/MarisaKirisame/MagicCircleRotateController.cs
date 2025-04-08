using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleRotateController : MonoBehaviour
{
    [SerializeField]
    private bool inorout;
    [SerializeField]
    private float rotatespeed = 2.0f;
    // Update is called once per frame
    void Update()
    {
        if (!inorout)
        {
            transform.Rotate(new Vector3(rotatespeed * Time.deltaTime, 0.0f, 0.0f), Space.World);
        }
        if (inorout)
        {
            transform.Rotate(new Vector3(-rotatespeed * Time.deltaTime, 0.0f, 0.0f), Space.World);
        }
    }
}
