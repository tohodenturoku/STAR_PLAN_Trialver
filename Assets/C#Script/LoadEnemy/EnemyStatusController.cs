using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// <summary>
/// 弾幕テスト用敵クラス
/// </summary>
public class EnemyStatusController : MonoBehaviour
{
    [SerializeField] private float HealthPoint = 20f;
    [SerializeField] private float AttackPoint = 5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(float damage)
    {
        HealthPoint -= damage;

        // もしHPが0以下になったら、エネミーを破棄
        if (HealthPoint <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().TakeDamage(AttackPoint);
        }
    }
}
