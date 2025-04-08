using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinYangBallController : MonoBehaviour
{
    private float destroyTimer;
    private Vector2 lastvelocity = Vector2.zero;

    void Start()
    {
        destroyTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        DestroyYinYangBall();
        lastvelocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;
    }

    void DestroyYinYangBall()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer > 2.0f)
            Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ground")
        {
            Vector2 normalVector = new Vector2(c.gameObject.transform.position.y,
            c.gameObject.transform.position.x).normalized;
            Vector2 reflectVector = Vector2.Reflect(lastvelocity * new Vector2(1.0f, -1.0f), normalVector);
            this.gameObject.GetComponent<Rigidbody2D>().velocity = reflectVector;
        }
    }
}
