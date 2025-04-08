using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBall2Controller : MonoBehaviour
{
    [SerializeField]
    private PerfectFreezeController perfectFreezeController;
    void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "iceball")
        {
            perfectFreezeController.icecount++;
            c.gameObject.SetActive(false);
        }
    }
}
