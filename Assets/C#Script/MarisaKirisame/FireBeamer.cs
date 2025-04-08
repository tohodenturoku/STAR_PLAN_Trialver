using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBeamer : MonoBehaviour
{
    private float stretchTime;
    private Vector3 initBeamScale;
    // Start is called before the first frame update
    void Start()
    {
        stretchTime = 0.0f;
        this.transform.localScale = this.transform.localScale = new Vector3(this.transform.localScale.x * 1.2f, this.transform.localScale.y + stretchTime, 0.0f);
        initBeamScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (stretchTime <= 5.0f)
            stretchTime += Time.deltaTime * 3.0f;
        this.transform.localScale = initBeamScale + new Vector3(0.0f, stretchTime, 0.0f);
    }
}
