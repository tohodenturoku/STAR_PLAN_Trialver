using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRightBom : MonoBehaviour
{
    // ワープ先のゲームオブジェクト
    [SerializeField]
    private GameObject toObject;
    [SerializeField]
    private InfernoBurnerController infernoBurnerController;
    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bom" && !infernoBurnerController.bomLRJudge)
        {
            other.gameObject.transform.position = new Vector3(toObject.transform.position.x,
                                                  other.gameObject.transform.position.y);
        }
    }
}
