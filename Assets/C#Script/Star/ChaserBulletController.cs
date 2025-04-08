using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 斜方投射の要領で考える（velocity使う）
/// </summary>

public class ChaserBulletController : MonoBehaviour
{
    [HideInInspector]
    public bool angleFlag;
    [SerializeField]
    private float speed = 2.0f; // 移動速度
    private GameObject enemy;  // 目的地
    private bool moving = false; // 動作開始フラグ
    private float upTimer;
    void Start()
    {
        upTimer = 0.0f;
        enemy = GameObject.Find("Enemy");
        if (enemy != null)
        {
            moving = true;
        }
    }

    void Update()
    {
        if (!moving) return;
        else
        {
            upTimer += Time.deltaTime;
            if (upTimer < 0.3f)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2((this.transform.position.x
                                                            - enemy.transform.position.x > 0 ? -1.0f : 1.0f) * 3.0f,
                                                            (angleFlag ? -1.0f : 1.0f) * 1.0f) * speed;
            }
            else if (upTimer >= 0.3 && upTimer < 0.6f)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2((this.transform.position.x
                                                            - enemy.transform.position.x > 0 ? -1.0f : 1.0f) * 3.0f
                                                            , -(angleFlag ? -1.0f : 1.0f) * 1.0f) * speed;
            }
            else
            {
                Vector2 Evec = (Vector2)(enemy.transform.position - this.transform.position).normalized;
                this.GetComponent<Rigidbody2D>().velocity = speed * Evec * 1.8f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            StartCoroutine(DelayDestroy());
        }
    }

    IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}
