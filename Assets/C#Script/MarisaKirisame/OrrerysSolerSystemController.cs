using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔理沙のスペカ1 天儀「オーレリーズソーラーシステム」の軌道周りの処理を記述したクラス
/// </summary>
public class OrrerysSolerSystemController : MonoBehaviour
{
    // 魔法陣が格納されるList
    [HideInInspector]
    public List<GameObject> MagicCircleClones;
    // 魔法陣のオブジェクトを消滅させるフラグ
    [HideInInspector]
    public bool destoryflag;

    [HideInInspector]
    public int forMarisa = 1;
    // 弾幕が生成される間隔
    [SerializeField]
    private float bulletclone_time = 0.5f;
    // 敵キャラ
    [SerializeField]
    private GameObject MarisaKirisame;
    // 魔法陣単体
    [SerializeField]
    private GameObject MagicCircle;
    [SerializeField]
    private GameObject Player;
    // 楕円回転のスピード
    [SerializeField]
    private float rotate_speed = 5f;
    // 回転角度
    [SerializeField]
    private List<float> nDegrees;
    // 楕円の半径
    [SerializeField]
    private Vector2 nRadius;
    // 弾幕単体
    [SerializeField]
    private GameObject Bullet;
    // 弾幕の発射スピード
    [SerializeField]
    private float bulletspeed = 1.5f;
    // 楕円運動の中心座標
    private Vector2 nCenter;
    // 弾幕のクローン
    private GameObject BulletClone;
    // ラジアン
    private float nRadian;
    // 時計
    private float limitTime = 0.0f;
    private int forPlayer = 0;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nDegrees = new List<float>();
        MagicCircleClones = new List<GameObject>();
        for (int i = 0; i < 3; i++)
        {
            nDegrees.Add(110 * (i + 1));
            MagicCircleClones.Add(Instantiate(MagicCircle,
                new Vector3(100.0f, 100.0f, 0.0f),
                Quaternion.identity));
        }
    }

    void Update()
    {
        forPlayer = 1 - forMarisa;
    }

    // 楕円回転
    public void RotateMagicCircle()
    {
        for (int i = 0; i < 3; i++)
        {
            nCenter = new Vector2(MarisaKirisame.transform.position.x,
                                  MarisaKirisame.transform.position.y);
            nDegrees[i] += rotate_speed * Time.deltaTime;
            nDegrees[i] = nDegrees[i] % 360;
            nRadian = nDegrees[i] * Mathf.PI / 180.0f;
            MagicCircleClones[i].transform.position = new Vector3(
                                     nCenter.x + Mathf.Cos(nRadian) * nRadius.x
                                     , nCenter.y + Mathf.Sin(nRadian) * nRadius.y
                                     , 0.0f);
        }
    }

    // 弾幕発射
    public void Marisa_FireBullet(GameObject MagicCircleClone)
    {
        limitTime += Time.deltaTime;
        if (bulletclone_time <= limitTime)
        {
            animator.SetTrigger("EAttack");
            if (Player.transform.position.x < this.transform.position.x)
                animator.SetInteger("EMove", -1);
            else
                animator.SetInteger("EMove", 1);
            animator.SetInteger("ELR", animator.GetInteger("EMove"));
            BulletClone = Instantiate(Bullet,
                                      new Vector3(MagicCircleClone.transform.position.x
                                      , MagicCircleClone.transform.position.y, 0.0f)
                                      , Quaternion.identity);
            Rigidbody2D bullet_rb = BulletClone.GetComponent<Rigidbody2D>();
            if (bullet_rb != null)
            {
                float directionToMarisa = Mathf.Atan2(MarisaKirisame.transform.position.y
                                  - MagicCircleClone.transform.position.y,
                                  MarisaKirisame.transform.position.x
                                  - MagicCircleClone.transform.position.x) * Mathf.Rad2Deg;
                float directionToPlayer = Mathf.Atan2(Player.transform.position.y
                                  - MagicCircleClone.transform.position.y,
                                  Player.transform.position.x
                                  - MagicCircleClone.transform.position.x) * Mathf.Rad2Deg;
                bullet_rb.velocity = new Vector2(Mathf.Cos((directionToMarisa * forMarisa + directionToPlayer * forPlayer) * Mathf.Deg2Rad),
                                     Mathf.Sin((directionToMarisa * forMarisa + directionToPlayer * forPlayer) * Mathf.Deg2Rad));
                BulletClone.transform.eulerAngles = new Vector3(0, 0, (directionToMarisa * forMarisa + directionToPlayer * forPlayer));
            }
            limitTime = 0.0f;
        }
    }
}
