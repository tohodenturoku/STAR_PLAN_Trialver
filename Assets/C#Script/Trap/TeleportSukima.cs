using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportSukima : MonoBehaviour
{
    [SerializeField]
    private GameObject tosukima;
    [SerializeField]
    private GameObject sukimaSE;

    void Start()
    {
        sukimaSE.GetComponent<AudioSource>().mute = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            sukimaSE.GetComponent<AudioSource>().mute = false;
            sukimaSE.GetComponent<AudioSource>().Play();
            sukimaSE.GetComponent<AudioSource>().mute = true; // 再びミュート
            other.gameObject.transform.position = tosukima.transform.position +
                                      (tosukima.transform.position.x - other.transform.position.x > 0 ? 1.0f : -1.0f)
                                      * new Vector3(3.0f, 0.0f, 0.0f);
        }
    }
}
