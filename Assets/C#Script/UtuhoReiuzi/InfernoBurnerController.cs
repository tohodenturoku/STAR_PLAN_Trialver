using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// お空のスペカ1 獄炎「インフェルノバーナー」の軌道周りの処理を記述したクラス
/// </summary>
public class InfernoBurnerController : MonoBehaviour
{
    [HideInInspector]
    public bool setflag;
    [HideInInspector]
    public bool setbulletflag;
    [HideInInspector]
    public bool firebulletflag;
    [HideInInspector]
    public bool moveflag;
    [HideInInspector]
    public bool bomLRJudge;
    [HideInInspector]
    public bool ismove;
    [SerializeField]
    private GameObject Bom;
    [SerializeField]
    private GameObject Bom_bullet;
    [SerializeField]
    private float Bom_bullet_speed = 2.0f;
    [SerializeField]
    private float BomSpeed = 20.0f;
    [SerializeField]
    private float UtuhoSpeed = 20.0f;
    private int vecjudge;
    private int potentialjudge;
    private UtuhoReiuziBossController utuhoReiuziBossController;
    private List<GameObject> Boms;
    private List<GameObject> Bom_bullets;
    private float setTime;
    private float changeLRTime;
    private Animator animator;

    void Start()
    {
        ismove = true;
        animator = GetComponent<Animator>();
        bomLRJudge = true;
        changeLRTime = 0.0f;
        Boms = new List<GameObject>();
        Bom_bullets = new List<GameObject>();
        utuhoReiuziBossController = this.gameObject.GetComponent<UtuhoReiuziBossController>();
        setflag = false;
        setbulletflag = false;
        firebulletflag = false;
        moveflag = false;
        setTime = 0.0f;
    }

    // 大きい球を生成
    public void SetBom()
    {
        for (int i = -1; i <= 1; i += 2)
        {
            for (int j = 1; j <= 3; j++)
            {
                Boms.Add(Instantiate(Bom,
                                new Vector3(utuhoReiuziBossController.okuupos.x + (i * j * 2.0f),
                                utuhoReiuziBossController.okuupos.y, 0.0f),
                                Quaternion.identity));
            }
        }
        setflag = true;
    }

    public void DestroySpell()
    {
        foreach (var b in Boms)
            Destroy(b);
    }

    // 大きい球を設置
    public void MoveBom()
    {
        changeLRTime += Time.deltaTime;
        if (changeLRTime >= 12.0f)
        {
            bomLRJudge = !bomLRJudge;
            BomSpeed *= -1.0f;
            changeLRTime = 0.0f;
        }
        for (int i = 0; i < 6; i++)
        {
            Boms[i].GetComponent<Rigidbody2D>().velocity = new Vector2(BomSpeed * Time.deltaTime, 0.0f);
        }
    }

    // プレイヤーを攻撃する用の小さい球を設置
    public void SetBomBullet()
    {
        setTime += Time.deltaTime;
        if (setTime >= 0.6f)
        {
            firebulletflag = false;
            for (int i = 0; i < 6; i++)
            {
                Bom_bullets.Add(Instantiate(Bom_bullet,
                            new Vector3(Boms[i].transform.position.x,
                            Boms[i].transform.position.y, 0.0f),
                            Quaternion.identity));
            }
            setbulletflag = true;
            setTime = 0.0f;
        }
    }

    // プレイヤーを攻撃する用の小さい球を発射
    public void FireBomBullet()
    {
        foreach (var bb in Bom_bullets)
            bb.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f * Bom_bullet_speed);
        Bom_bullets.Clear();
        firebulletflag = true;
        setbulletflag = false;
    }

    // ランダムで移動する方向を決める
    public void JudgeArea()
    {
        if (potentialjudge == 0)
        {
            if (this.transform.position == new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f))
            {
                potentialjudge = 1;
                vecjudge = Random.Range(1, 3);
            }
            else if (this.transform.position == new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f))
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
                     new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 1)
        {
            float distance = Vector2.Distance(this.transform.position,
                             (new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f)));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 1 && potentialjudge == 2)
        {
            float distance = Vector2.Distance(this.transform.position,
                     (new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f)));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.center_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 2)
        {
            float distance = Vector2.Distance(this.transform.position,
                             new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 1 && potentialjudge == 3)
        {
            float distance = Vector2.Distance(this.transform.position,
                     new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.up_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
        else if (vecjudge == 2 && potentialjudge == 3)
        {
            float distance = Vector2.Distance(this.transform.position,
                             new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f));
            if (distance < 0.01f)
            {
                animator.SetInteger("EMove", -2);
                ismove = true;
                this.transform.position = new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f);
                potentialjudge = 0;
                vecjudge = 0;
                utuhoReiuziBossController.sp1_movetime = 0.0f;
            }
            else
            {
                animator.SetInteger("EMove", -1);
                Vector2 direction = (new Vector3(this.transform.position.x, utuhoReiuziBossController.under_r.transform.position.y, 0.0f) - this.transform.position).normalized;
                transform.Translate(direction * UtuhoSpeed * Time.deltaTime);
            }
        }
    }
}
