using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarisaBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.Rotate(0.0f, 0.0f, -90.0f, Space.World);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Marisa") || other.gameObject.CompareTag("Border"))
            this.gameObject.SetActive(false);
        
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(2.0f);
            this.gameObject.SetActive(false);
        }
    }
}
