using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject P;
    Vector3 Cposi;
    private float Rad;
    private float c;
    // Start is called before the first frame update
    void Start()
    {
        Cposi = new Vector3(transform.position.x - P.transform.position.x, transform.position.y - P.transform.position.y, transform.position.z);
        c = Mathf.Sqrt(Cposi.x*Cposi.x + Cposi.y*Cposi.y);
        Rad = Mathf.Acos(Cposi.x / c);
        if(Cposi.y < 0) Rad = 2f*Mathf.PI - Rad;
    }
    void Update()
    {
        float Deg = P.transform.localEulerAngles.z;
        float S = Mathf.Sin(Deg * Mathf.Deg2Rad), C = Mathf.Cos(Deg * Mathf.Deg2Rad);
        this.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, P.transform.localEulerAngles.z);
        this.transform.position =  new Vector3(P.transform.position.x + c * Mathf.Cos(Deg * Mathf.Deg2Rad + Rad), P.transform.position.y + c * Mathf.Sin(Deg * Mathf.Deg2Rad + Rad), Cposi.z);
    }
}
