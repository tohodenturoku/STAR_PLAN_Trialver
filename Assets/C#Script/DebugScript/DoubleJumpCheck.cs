using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpCheck : MonoBehaviour
{
    [SerializeField] private PlayerController PC;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<SpriteRenderer>().enabled = !PC.isJumping;
    }
}
