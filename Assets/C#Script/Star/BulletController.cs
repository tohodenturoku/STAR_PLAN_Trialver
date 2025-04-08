using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの通常弾処理クラス
/// </summary>
public class BulletController : MonoBehaviour
{
    // 存在時間(秒)この時間が過ぎると消滅
    public float limitTime = 0.7f;
    public bool LR = true;
    // 弾速
    [SerializeField] private float speed = 3.0f;
    // 角度(0-360)0で右・90で上
    [SerializeField] private double angle = 0.0;
    // 命中時のダメージ
    [SerializeField] private float damage = 1.0f;


    // Update
    void Update()
    {
        // 移動ベクトル計算(1フレーム分の進行方向と距離を取得)
        double DeltaX = speed * Time.deltaTime * Math.Cos(angle / 180 * Math.PI);
        double DeltaY = speed * Time.deltaTime * Math.Sin(angle / 180 * Math.PI);
        float x = (float)DeltaX;
        float y = (float)DeltaY;
        Vector2 vecPuls = new Vector2(x, y);
        Vector2 vecMinus = new Vector2(-x, y);

        if (!LR)
        {
            transform.Translate(vecPuls);
        }
        else
        {
            transform.Translate(vecPuls);
        }

        // 移動ベクトルをもとに移動
        // 消滅判定
        limitTime -= Time.deltaTime;
        if (limitTime < 0.0f)
        {// 存在時間が0になったら消滅
            Destroy(this.gameObject);
        }
    }

    // 各トリガー呼び出し処理
    // トリガー進入時に呼出
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {// 地面・壁に命中
            Destroy(this.gameObject);
        }
    }

    public void direcition(bool direct)
    {
        LR = direct;
    }
}