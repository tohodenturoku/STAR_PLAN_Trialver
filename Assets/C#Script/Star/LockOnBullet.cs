using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの通常弾処理クラス
/// </summary>
public class LockOnBullet : MonoBehaviour
{
    // 各種変数
    [SerializeField] private float speed = 1.0f;      // 弾速
    [SerializeField] private float angle = 0.0f;      // 角度(0-360)0で右・90で上
    [SerializeField] private float damage = 0.5f;       // 命中時のダメージ
    [SerializeField] private float limitTime = 3.0f;  // 存在時間(秒)この時間が過ぎると消滅


    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        // 移動ベクトル計算(1フレーム分の進行方向と距離を取得)
        double DeltaX = speed * Mathf.Sqrt(Time.deltaTime);
        float x = (float)DeltaX;
        Vector3 vec = new Vector2(x, 0);

        // 移動ベクトルをもとに移動
        transform.Translate(vec);

        // 消滅判定
        limitTime -= Time.deltaTime;
        if (limitTime < 0.0f)
        {// 存在時間が0になったら消滅
            Destroy(this.gameObject);
        }
    }

    // 各トリガー呼び出し処理
    // トリガー進入時に呼出
    private void OnTriggerEnter2D(Collider2D collider)
    {

        // 命中対象ごとの処理
        if (collider.gameObject.CompareTag("Enemy"))
        {// エネミーに命中
            collider.gameObject.GetComponent<EnemyStatusController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (collider.gameObject.CompareTag("Ground"))
        {// 地面・壁に命中
            Destroy(this.gameObject);
        }
    }
    public void DecideAngle(float InputAngle)
    {
        angle = InputAngle;
    }
}