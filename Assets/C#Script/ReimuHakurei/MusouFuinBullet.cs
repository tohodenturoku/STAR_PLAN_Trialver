using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス戦博麗霊夢のスペカ霊符「夢想封印」の弾クラス
/// </summary>

public class MusouFuinBullet : MonoBehaviour
{
    [HideInInspector]
    public bool angleFlag;
    [SerializeField]
    private float speed = 2.0f; // 移動速度
    private GameObject player;  // 目的地
    private bool moving = false; // 動作開始フラグ
    private float upTimer;
    private bool evecflag;
    void Start()
    {
        evecflag = true;
        upTimer = 0.0f;
        player = GameObject.Find("Player");
        if (player != null)
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
                                                            - player.transform.position.x > 0 ? -1.0f : 1.0f) * 1.2f,
                                                            (angleFlag ? -1.0f : 1.0f) * 1.0f) * speed;
            }
            else if (upTimer >= 0.3 && upTimer < 0.6f)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2((this.transform.position.x
                                                            - player.transform.position.x > 0 ? -1.0f : 1.0f) * 1.2f
                                                            , -(angleFlag ? -1.0f : 1.0f) * 1.0f) * speed;
            }
            else
            {
                if (evecflag)
                {
                    Vector2 Evec = (Vector2)(player.transform.position - this.transform.position).normalized;
                    this.GetComponent<Rigidbody2D>().velocity = speed * Evec * 0.7f;
                    evecflag = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
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
