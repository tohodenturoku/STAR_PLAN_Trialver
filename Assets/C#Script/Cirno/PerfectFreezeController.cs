using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チルノのスペカ3 氷符「パーフェクトフリーズ」の軌道周りの処理を記述したクラス
/// </summary>
public class PerfectFreezeController : MonoBehaviour
{
    [HideInInspector]
    public int icecount;
    [HideInInspector]
    public bool setflag;
    [HideInInspector]
    public bool freezeflag;
    [HideInInspector]
    public bool fallflag;
    // アイスボール単体
    [SerializeField]
    private GameObject iceball;
    // アイスボール停止場所
    [SerializeField]
    private List<GameObject> freeze = new List<GameObject>();
    // アイスボールの移動スピード
    [SerializeField]
    private float moveSpeed = 200.0f;
    // アイスボール達
    private List<GameObject> iceballs;
    private float falltime;
    // アイスボール落下スピード
    private float fallSpeed = 600.0f;
    private int rand;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        iceballs = new List<GameObject>();
        for (int i = 0; i < 12; i++)
        {
            iceballs.Add(Instantiate(iceball, new Vector3(100.0f, 100.0f, 0.0f), Quaternion.identity));
            iceballs[i].SetActive(false);
        }

        freezeflag = false;
        fallflag = false;
        setflag = false;
        falltime = 0.0f;
        icecount = 0;
    }

    void Update()
    {
        CountIce();
    }

    void CountIce()
    {
        if (icecount == 12)
        {
            setflag = false;
            fallflag = false;
            icecount = 0;
        }
    }


    // アイスボール設置
    public void SetIceball()
    {
        rand = Random.Range(-1, 1);
        foreach (var i in iceballs)
        {
            i.SetActive(true);
            i.transform.position = this.transform.position;
        }
        animator.SetTrigger("EAttack");
        animator.SetInteger("EMove", -1);
        setflag = true;
    }

    // アイスボール発射
    public void FireIceball()
    {
        for (int i = 0; i < 12; i++)
        {
            float distance = Vector2.Distance
            (new Vector2(iceballs[i].transform.position.x,
            iceballs[i].transform.position.y),
            new Vector2(freeze[i].transform.position.x,
            freeze[i].transform.position.y));
            if (distance < 0.01f && !fallflag)
            {
                iceballs[i].transform.position = freeze[i].transform.position;
                freezeflag = true;
            }
            else
            {
                Vector2 direction = (new Vector2(freeze[i].transform.position.x,
                freeze[i].transform.position.y)
                - new Vector2(iceballs[i].transform.position.x,
                iceballs[i].transform.position.y)).normalized;
                iceballs[i].transform.Translate((direction * moveSpeed * Time.deltaTime)
                - new Vector2(rand * 0.09f, 0.0f));
                animator.SetTrigger("EAttack");
                animator.SetInteger("EMove", -1);
            }
        }
    }

    // 落ちるのを待つ
    public void FallDelayIceball()
    {
        falltime += Time.deltaTime;
        if (falltime >= 3.5f)
        {
            freezeflag = false;
            fallflag = true;
            falltime = 0.0f;
        }
    }

    public void DestroyIceball()
    {
        for (int i = 0; i < iceballs.Count; i++)
            Destroy(iceballs[i]);
    }

    // アイスボールを落とす
    public void FallIceball()
    {
        for (int i = 0; i < 12; i++)
        {
            iceballs[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -fallSpeed) * Time.deltaTime;
        }
        animator.SetTrigger("EAttack");
        animator.SetInteger("EMove", -1);
    }
}