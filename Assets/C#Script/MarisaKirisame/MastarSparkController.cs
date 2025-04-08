using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔理沙のスペカ3 恋符「マスタースパーク」の軌道周りの処理を記述したクラス
/// </summary>

public class MastarSparkController : MonoBehaviour
{
    // ビームの寿命
    [HideInInspector]
    public float beamlifespan;
    // 星の寿命
    [HideInInspector]
    public float starlifesapn;
    // 魔理沙を動かして良いか否かを表すフラグ
    [HideInInspector]
    public bool ismove;
    // 弾幕を撃っているという状態を記録するフラグ
    [HideInInspector]
    public bool isfire;
    // 弾幕チャージ中であるというフラグ
    [HideInInspector]
    public bool bulletcharge;
    // 魔理沙の速さ
    [SerializeField]
    private float marisaspeed = 5.0f;
    [SerializeField]
    private GameObject beam;
    [SerializeField]
    private GameObject StarBullet;
    // 魔理沙本体クラスのインスタンス
    private MarisaKirisameBossController MKBC;
    // 現在の高さを判断するための数字
    private int potentialjudge;
    // どこへ移動するかを判断するための数字
    private int vecjudge;
    private GameObject beamclone;
    private List<GameObject> StarBulletClones;
    // 魔理沙の現在地
    private Vector2 marisapos;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        StarBulletClones = new List<GameObject>();
        potentialjudge = 0;
        MKBC = GetComponent<MarisaKirisameBossController>();
        isfire = true;
        ismove = false;
        bulletcharge = true;
        beamlifespan = 0.0f;
        starlifesapn = 0.0f;
    }

    // ビームを設置&発射
    public void SetBeam()
    {
        animator.SetTrigger("EAttack");
        animator.SetInteger("EMove", -1);
        beamclone = Instantiate(beam,
                                new Vector3(marisapos.x - 1.0f,
                                marisapos.y + 0.5f, 0.0f),
                                Quaternion.identity);
        beamclone.transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
        for (int i = -5; i < 2; i++)
        {
            StarBulletClones.Add(Instantiate(StarBullet,
                        new Vector3(marisapos.x - 3.0f,
                        marisapos.y + (i * 1.8f), 0.0f),
                        Quaternion.identity));
            StarBulletClones[i + 5].transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
            StarBulletClones[i + 5].GetComponent<Rigidbody2D>().velocity = new Vector2(-3.0f, 0);
        }
        bulletcharge = false;
    }

    // 魔理沙の位置
    public void GetMarisaPos()
    {
        marisapos = new Vector2(this.transform.position.x, this.transform.position.y);
    }

    // ビームを消滅させる
    public void DestroyBeam()
    {
        beamlifespan += Time.deltaTime;
        if (beamlifespan >= 5.0f)
        {
            Destroy(beamclone);
            beamlifespan = 0.0f;
        }
    }

    // 星を消滅させる
    public void DestroyStar()
    {
        starlifesapn += Time.deltaTime;
        if (starlifesapn >= 5.0f)
        {
            foreach (var s in StarBulletClones)
                Destroy(s);
            StarBulletClones.Clear();
            starlifesapn = 0.0f;
            ismove = true;
            isfire = false;

        }
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
