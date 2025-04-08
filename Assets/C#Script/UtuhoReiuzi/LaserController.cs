using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private Rigidbody2D Laser_rb;
    private Vector2 lastvelocity = Vector2.zero;
    void Start()
    {
        Laser_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        lastvelocity = Laser_rb.velocity;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Bom")
        {
            Vector2 normalVector = new Vector2(c.gameObject.transform.position.y,
            c.gameObject.transform.position.x).normalized;
            Vector2 reflectVector = Vector2.Reflect(lastvelocity, normalVector);
            Laser_rb.velocity = reflectVector;
        }
    }
}