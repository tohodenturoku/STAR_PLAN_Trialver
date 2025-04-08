using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBallController : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D c) 
    {
        if (c.gameObject.tag == "EnemyBullet")
            Destroy(c.gameObject);
    }
}
