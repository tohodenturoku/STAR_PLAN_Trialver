using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShotController : MonoBehaviour
{
    [SerializeField]
    private IcicleFallController icicleFallController;
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "iceshot")
            icicleFallController.destorycount++;
    }
}
