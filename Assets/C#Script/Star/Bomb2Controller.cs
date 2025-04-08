using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb2Controller : MonoBehaviour
{
    // 命中時のダメージ
    [SerializeField] private float damage = 5.0f;
    // 存在時間(秒)この時間が過ぎると消滅
    [SerializeField] private float limitTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 消滅判定
        limitTime -= Time.deltaTime;
        if (limitTime < 0.0f)
        {// 存在時間が0になったら消滅
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 命中対象ごとの処理
        if (collision.gameObject.CompareTag("Enemy"))
        {// エネミーに命中
            collision.gameObject.GetComponent<EnemyStatusController>().TakeDamage(damage);
        }
    }
}
