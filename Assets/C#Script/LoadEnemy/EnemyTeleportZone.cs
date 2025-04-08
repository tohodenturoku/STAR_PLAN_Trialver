using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵をテレポートさせるクラス
/// </summary>
public class EnemyTeleportZone : MonoBehaviour
{
    [SerializeField] private GameObject SpawnZone;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.transform.position = new Vector3(SpawnZone.transform.position.x, SpawnZone.transform.position.y, other.transform.position.z);
        }
    }
}
