using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 敵の位置を示すスライダークラス
/// </summary>
public class EnemyposNaviController : MonoBehaviour
{
    [SerializeField]
    private GameObject _Start;
    [SerializeField]
    private GameObject _Goal;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private Slider EnemyposNavi;
    // ステージの距離
    private float mapdis;
    // Start is called before the first frame update
    void Start()
    {
        mapdis = _Goal.transform.position.x - _Start.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyposNavi.value = (enemy.transform.position.x - _Start.transform.position.x) / mapdis;
    }
}
