using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall2DamageController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerController>().TakeDamage(3.0f);
            this.gameObject.SetActive(false);
        }
    }
}
