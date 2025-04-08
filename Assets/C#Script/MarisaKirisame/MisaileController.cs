using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisaileController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(5.0f);
            this.gameObject.SetActive(false);
        }
    }
}
