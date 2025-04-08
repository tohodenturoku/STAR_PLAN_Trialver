using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔理沙のスペカ2通常弾幕攻撃 マジックミサイルの軌道周りの処理を記述したクラス
/// </summary>
public class MarisaNormalController : MonoBehaviour
{
    // 弾幕を撃っているという状態を記録するフラグ
    [HideInInspector]
    public bool isfire;
    // 弾幕チャージ中であるというフラグ
    [HideInInspector]
    public bool bulletcharge;
    // 魔理沙を動かして良いか否かを表すフラグ
    [HideInInspector]
    public bool ismove;
    // ミサイルをこの世から消し去ってしまえぇ
    [HideInInspector]
    public bool misailedestroyflag;
    // 弾幕の寿命
    [HideInInspector]
    public float lifespan;
    // 魔理沙の速さ
    [SerializeField]
    private float marisaspeed = 5.0f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject bullet;
    // 弾幕の速さ
    [SerializeField]
    private float bulletspeed = 4.0f;
    // 魔理沙の現在地
    private Vector2 marisapos;
    private GameObject[][] bulletclones;
    // 弾幕の行数
    private int bullet_row;
    // 弾幕の列数
    private int bullet_col;
    // イラストを動かす
    private Animator animator;
    // 魔理沙からプレイヤへのベクトル
    private Vector2 direction;
    // 追尾する制限時間
    private float limittime = 0.4f;
    // 追尾時間カウント用
    private float deltatime = 0.0f;
    // 魔理沙本体クラスのインスタンス
    private MarisaKirisameBossController MKBC;
    // 現在の高さを判断するための数字
    private int potentialjudge;
    // どこへ移動するかを判断するための数字
    private int vecjudge;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isfire = true;
        potentialjudge = 0;
        lifespan = 0.0f;
        MKBC = GetComponent<MarisaKirisameBossController>();
        bullet_row = 3;
        bullet_col = 2;
        bulletclones = new GameObject[bullet_row][];
        for (int i = 0; i < bullet_row; i++)
            bulletclones[i] = new GameObject[bullet_col];
        bulletcharge = true;
        ismove = false;
        misailedestroyflag = false;
    }

    // 弾幕を配置する
    public void SetBullet()
    {
        for (int i = 0; i < bullet_row; i++)
        {
            for (int j = 0; j < bullet_col; j++)
            {
                bulletclones[i][j] = Instantiate(bullet,
                                     new Vector3(marisapos.x - (j + 1),
                                     (marisapos.y + 1) - i, 0.0f),
                                     Quaternion.identity);
            }
        }
        bulletcharge = false;
    }

    // 弾幕発射
    public void FireBullet()
    {
        deltatime += Time.deltaTime;
        if (deltatime <= limittime)
            GetEVec();
        else
        {
            animator.SetTrigger("EAttack");
            animator.SetInteger("EMove", -1);
            for (int i = 0; i < bullet_row; i++)
            {
                for (int j = 0; j < bullet_col; j++)
                    bulletclones[i][j].transform.Translate(direction * bulletspeed * Time.deltaTime);
            }
        }
    }

    // 弾幕消去
    public void DestroyBullet()
    {
        lifespan += Time.deltaTime;
        if (lifespan >= 5.0f)
        {
            for (int i = 0; i < bullet_row; i++)
            {
                for (int j = 0; j < bullet_col; j++)
                    Destroy(bulletclones[i][j]);
            }
            ismove = true;
            isfire = false;
            lifespan = 0.0f;
            deltatime = 0.0f;
        }
    }

    // 魔理沙の位置
    public void GetMarisaPos()
    {
        marisapos = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    // 魔理沙からプレイヤーへの単位ベクトルを出す
    void GetEVec()
    {
        direction = (new Vector2(player.transform.position.x,
                    player.transform.position.y) - marisapos).normalized;
    }
    // ランダムで移動する方向を決める
    public void JudgeArea()
    {
        if (potentialjudge == 0)
        {
            if (this.transform.position == (MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)))
            {
                potentialjudge = 1;
                vecjudge = Random.Range(1, 3);
            }
            else if (this.transform.position == (MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)))
            {
                potentialjudge = 2;
                vecjudge = Random.Range(1, 3);
            }
            else
            {
                potentialjudge = 3;
                vecjudge = Random.Range(1, 3);
            }
        }
    }

    //　JudgeArea内で決めた方向に行く
    public void MoveArea()
    {
        if (vecjudge == 1 && potentialjudge == 1)
        {
            float distance = Vector2.Distance(this.transform.position,
                     (MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 1)
        {
            float distance = Vector2.Distance(this.transform.position,
                             (MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 1 && potentialjudge == 2)
        {
            float distance = Vector2.Distance(this.transform.position,
                     (MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.center_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 2)
        {
            float distance = Vector2.Distance(this.transform.position,
                             (MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 1 && potentialjudge == 3)
        {
            float distance = Vector2.Distance(this.transform.position,
                     (MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.up_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 3)
        {
            float distance = Vector2.Distance(this.transform.position,
                             (MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                bulletcharge = true;
                isfire = true;
                this.transform.position = MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = ((MKBC.under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
                transform.Translate(direction * marisaspeed * Time.deltaTime);
            }
        }
    }
}
