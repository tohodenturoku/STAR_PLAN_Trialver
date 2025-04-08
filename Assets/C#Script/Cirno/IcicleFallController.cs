using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チルノのスペカ2 氷符「アイシクルフォール」の軌道周りの処理を記述したクラス
/// </summary>
public class IcicleFallController : MonoBehaviour
{
    // 動く準備に入ったかどうか判定するフラグ
    [HideInInspector]
    public bool moveflag;
    // 止まっているかどうか判定するフラグ
    [HideInInspector]
    public bool freezeflag;
    // iceshotが設置できているか判定するフラグ
    [HideInInspector]
    public bool setflag;
    // チャージが終わったか判定するフラグ
    [HideInInspector]
    public bool chargeflag;
    [HideInInspector]
    public int destorycount;
    // アイスショット単体
    [SerializeField]
    private GameObject iceshot;
    [SerializeField]
    private List<GameObject> freeze = new List<GameObject>();
    [SerializeField]
    private float movespeed = 100.0f;
    [SerializeField]
    private GameObject player;
    // 弾幕の速さ
    [SerializeField]
    private float bulletspeed = 7.0f;
    private int MoveVecJudge;
    private float chargetime;
    private List<Vector2> firedirections;
    private List<GameObject> iceshots;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        iceshots = new List<GameObject>();
        firedirections = new List<Vector2>();
        for (int i = 0; i < 4; i++)
            firedirections.Add(new Vector2(0, 0));
        moveflag = false;
        MoveVecJudge = 0;
        freezeflag = false;
        setflag = false;
        chargetime = 0.0f;
        chargeflag = false;
        destorycount = 0;
        for (int i = 0; i < 4; i++)
        {
            iceshots.Add(Instantiate(iceshot,
            new Vector3(100f, 100f, 100f),
            Quaternion.identity));
        }
    }

    public void MoveJudge()
    {
        MoveVecJudge = Random.Range(0, 11);
        moveflag = true;
    }

    public void MoveArea()
    {
        float distance = Vector2.Distance(freeze[MoveVecJudge].transform.position,
        this.transform.position);
        if (distance < 0.1f)
        {
            animator.SetInteger("EMove", 0);
            this.transform.position = freeze[MoveVecJudge].transform.position;
            freezeflag = true;
        }
        else
        {
            Vector2 direction = MoveEVec(this.transform.position,
            freeze[MoveVecJudge].transform.position);
            if (direction.x < 0)
                animator.SetInteger("EMove", -1);
            else
                animator.SetInteger("EMove", 1);
            this.transform.Translate(direction * Time.deltaTime * movespeed);
        }
    }

    public void SetIceShot()
    {
        for (int i = -2; i < 2; i++)
        {
            if (i != 0)
            {
                iceshots[i + 2].SetActive(true);
                iceshots[i + 2].transform.position = new Vector3(player.transform.position.x + i,
                player.transform.position.y + 2.0f, 0.0f);
            }
            else
            {
                iceshots[2].SetActive(true);
                iceshots[2].transform.position = new Vector3(player.transform.position.x + 1,
                player.transform.position.y + 2.0f, 0.0f);
            }
            animator.SetTrigger("EAttack");
            if (this.transform.position.x < player.transform.position.x)
            {
                animator.SetInteger("ELR", 1);
                animator.SetInteger("EMove", 1);
            }
            else
            {
                animator.SetInteger("ELR", 1);
                animator.SetInteger("EMove", 1);
            }
        }
        setflag = true;
    }

    public void DestroyIceShot()
    {
        for (int i = 0; i < 4; i++)
            Destroy(iceshots[i]);
        iceshots.Clear();
    }

    public void ChargeIceShot()
    {
        chargetime += Time.deltaTime;
        Debug.Log(chargetime);
        if (chargetime < 0.6f)
        {
            foreach (var i in iceshots)
            {
                float directiontoPlayer = Mathf.Atan2(player.transform.position.y
                                          - i.transform.position.y,
                                          player.transform.position.x
                                          - i.transform.position.x) * Mathf.Rad2Deg;
                i.transform.eulerAngles = new Vector3(0.0f, 0.0f, -directiontoPlayer);
            }
            for (int i = 0; i < 4; i++)
            {
                firedirections[i] = MoveEVec(iceshots[i].transform.position
                                    , player.transform.position);
                int _i = i - 2;
                if (_i < 0)
                {
                    iceshots[i].transform.position = new Vector3(player.transform.position.x + _i,
                                    player.transform.position.y + 3.0f, 0.0f);
                }
                else if (_i >= 0)
                {
                    _i = (i - 2) + 1;
                    iceshots[i].transform.position = new Vector3(player.transform.position.x + _i,
                    player.transform.position.y + 3.0f, 0.0f);
                }
            }
        }
        else
        {
            chargeflag = true;
            chargetime = 0.0f;
        }
    }

    public void FireIceShot()
    {
        if (chargeflag && destorycount != 4)
        {
            for (int i = 0; i < 4; i++)
                iceshots[i].GetComponent<Rigidbody2D>().velocity = (firedirections[i] * bulletspeed * Time.deltaTime);
            animator.SetTrigger("EAttack");
            if (this.transform.position.x < player.transform.position.x)
            {
                animator.SetInteger("ELR", 1);
                animator.SetInteger("EMove", 1);
            }
            else
            {
                animator.SetInteger("ELR", -1);
                animator.SetInteger("EMove", -1);
            }
        }
        else if (destorycount == 4)
        {
            foreach (var i in iceshots)
                i.GetComponent<Rigidbody2D>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
            moveflag = false;
            MoveVecJudge = 0;
            freezeflag = false;
            setflag = false;
            chargeflag = false;
            destorycount = 0;
        }
    }
    Vector2 MoveEVec(Vector3 current, Vector3 target)
    {
        Vector2 vec = (new Vector2(target.x, target.y)
            - new Vector2(current.x, current.y)).normalized;
        return vec;
    }
}
