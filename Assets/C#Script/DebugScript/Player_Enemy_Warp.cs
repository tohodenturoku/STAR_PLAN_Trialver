using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Enemy_Warp : MonoBehaviour
{
    [SerializeField] private GameObject P;
    [SerializeField] private GameObject C;
    [SerializeField] private GameObject E;
    [SerializeField] private bool PlayerWarp = true;
    [SerializeField] private bool EnemyWarp = true;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerWarp)
        {
            P.transform.localPosition = new Vector3(this.transform.localPosition.x + P.transform.localPosition.x, P.transform.localPosition.y, this.transform.localPosition.z + P.transform.localPosition.z);
            C.transform.localPosition = new Vector3(this.transform.localPosition.x + C.transform.localPosition.x, C.transform.localPosition.y, this.transform.localPosition.z + C.transform.localPosition.z);
        }
        if(EnemyWarp)
        {
            E.transform.localPosition = new Vector3(this.transform.localPosition.x + E.transform.localPosition.x, E.transform.localPosition.y, this.transform.localPosition.z + E.transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
