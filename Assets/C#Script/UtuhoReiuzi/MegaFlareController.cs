using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// お空のスペカ3 爆符「メガフレア」の軌道周りの処理を記述したクラス
/// </summary>
public class MegaFlareController : MonoBehaviour
{
    [HideInInspector]
    public bool ismove;
    [HideInInspector]
    public bool isfire;
    [SerializeField]
    private float UtuhoSpeed = 20.0f;
    [SerializeField]
    private GameObject MegalareBom;
    private UtuhoReiuziBossController utuhoReiuziBossController;
    private int potentialjudge;
    private int vecjudge;
    private Vector2 BulletPosition;
    private Animator animator;
    private float timer;
    private int fireCount = 0;
    private float delayTimer;
    // Start is called before the first frame update
    void Start()
    {
        delayTimer = 0.0f;
        timer = 0.0f;
        BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        ismove = true;
        isfire = false;
        animator = GetComponent<Animator>();
        utuhoReiuziBossController = this.gameObject.GetComponent<UtuhoReiuziBossController>();
    }

    void Update()
    {
        BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
    }

    // Update is called once per frame    // ランダムで移動する方向を決める
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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
            if (distance < 0.5f)
            {
                animator.SetInteger("EMove", -2);
                ismove = false;
                isfire = true;
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

    void DelayMove()
    {
        delayTimer += Time.deltaTime;
        if (delayTimer > 1.0f)
        {
            StopBom();
        }
    }

    public void FireBom()
    {
        if (fireCount >= 3)
            DelayMove();
        else if (timer == 0.0f)
        {
            GameObject player = GameObject.Find("Player");
            int direction_to_player = this.gameObject.transform.position.x
            - player.transform.position.x > 0 ? -1 : 1;
            animator.SetTrigger("EAttack");
            animator.SetInteger("EMove", direction_to_player);
            GameObject BombCopy = Instantiate(MegalareBom) as GameObject;
            BombCopy.GetComponent<MegaFlareBom>().direcition(false);
            BombCopy.transform.position = BulletPosition;
        }
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            fireCount++;
            timer = 0.0f;
        }
    }

    public void StopBom()
    {
        fireCount = 0;
        isfire = false;
        ismove = true;
        delayTimer = 0.0f;
        timer = 0.0f;
    }
}
