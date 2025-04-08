using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの位置を示すスライダークラス
/// </summary>
public class MapNaviController : MonoBehaviour
{
    [SerializeField]
    private GameObject _Start;
    [SerializeField]
    private GameObject _Goal;
    [SerializeField]
    private GameObject MapNavi;
    [SerializeField]
    public GameObject player;
    [SerializeField]
    private TalkWindowController talkWindowController;
    // ステージの距離
    private float mapdis;
    private Slider mapnavi;

    // Start is called before the first frame update
    void Start()
    {
        mapnavi = MapNavi.GetComponent<Slider>();
        mapdis = _Goal.transform.position.x - _Start.transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        mapnavi.value = (player.transform.position.x - _Start.transform.position.x) / mapdis;
    }
}
