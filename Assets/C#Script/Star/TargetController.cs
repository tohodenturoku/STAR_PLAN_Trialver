using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour
{
    [SerializeField] private float limitTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        limitTime -= Time.deltaTime;
        if (limitTime < 0.0f)
        {// 存在時間が0になったら消滅
            Destroy(this.gameObject);
        }
    }
}
