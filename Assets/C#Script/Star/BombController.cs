using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アクターの通常弾処理クラス
/// </summary>
public class BombController : MonoBehaviour
{
    // 各種変数
    [SerializeField] private GameObject ExplodedBomb;
    private bool LR = true;
    // 弾速
    [SerializeField] private float speed = 4.0f;
    // 角度(0-360)0で右・90で上      
    [SerializeField] private double angle = 0.0;
    [SerializeField] private float limitTime = 5.0f;


    // Update
    void Update()
    {
        // 移動ベクトル計算(1フレーム分の進行方向と距離を取得)
        double DeltaX = 5 * speed * Time.deltaTime * Math.Cos(angle / 180 * Math.PI);
        double DeltaY = speed * Time.deltaTime * Math.Sin(angle / 180 * Math.PI);
        float x = (float)DeltaX;
        float y = (float)DeltaY;
        Vector2 vecPuls = new Vector2(x, y);
        Vector2 vecMinus = new Vector2(-x, y);

        // 移動ベクトルをもとに移動
        if (!LR)
        {
            transform.Translate(vecMinus);
        }
        else
        {
            transform.Translate(vecPuls);
        }

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
        // 命中対象ごとの処理
        if (collision.gameObject.CompareTag("Enemy"))
        {// エネミーに命中
            GameObject EBCopy = Instantiate(ExplodedBomb) as GameObject;
            EBCopy.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {// 地面・壁に命中
            GameObject EBCopy = Instantiate(ExplodedBomb) as GameObject;
            EBCopy.transform.position = this.transform.position;
            Destroy(this.gameObject);
        }
    }

    public void direcition(bool direct)
    {
        LR = direct;
    }
}