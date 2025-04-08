using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 霊夢のスペカ2 珠符「明珠暗投」の軌道周りの処理を記述したクラス
/// </summary>
public class MeisyuAntouController : MonoBehaviour
{
    [HideInInspector]
    public bool moveflag;
    [HideInInspector]
    public bool freezeflag;
    [HideInInspector]
    public bool setflag;
    [SerializeField]
    private GameObject YinYangBall;
    [SerializeField]
    private List<GameObject> freeze = new List<GameObject>();
    [SerializeField]
    private float movespeed = 9.0f;
    private GameObject YinYangBall_clone;
    private int MoveVecJudge;
    private Animator animator;
    private int FireCount;
    private float setTimer;
    private float recastTimer;
    // Start is called before the first frame update
    void Start()
    {
        FireCount = 0;
        recastTimer = 0.0f;
        animator = this.gameObject.GetComponent<Animator>();
        moveflag = false;
        freezeflag = false;
        setflag = false;
        setTimer = 0.0f;
    }

    public void MoveJudge()
    {
        MoveVecJudge = Random.Range(0, 9);
        moveflag = true;
    }

    public void MoveArea()
    {
        float distance = Vector2.Distance(freeze[MoveVecJudge].transform.position,
        this.transform.position);
        if (distance < 0.5f)
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

    public void SetYinYangBall()
    {
        GameObject player = GameObject.Find("Player");
        float direction_to_player = this.gameObject.transform.position.x
        - player.transform.position.x > 0 ? -1.0f : 1.0f;
        YinYangBall_clone = Instantiate(YinYangBall,
        this.transform.position + new Vector3(direction_to_player * 1.5f, 0.0f, 0.0f),
        Quaternion.identity);
        YinYangBall_clone.transform.localScale = new Vector3(0.01f, 0.01f, 0.0f);
        FireCount++;
        setflag = true;
    }

    public void FireYinYangBall()
    {
        GameObject player = GameObject.Find("Player");
        setTimer += Time.deltaTime;
        if (FireCount >= 3.0f)
        {
            recastTimer += Time.deltaTime;
            if (recastTimer > 2.3f)
            {
                recastTimer = 0.0f;
                FireCount = 0;
                setTimer = 0.0f;
                moveflag = false;
                freezeflag = false;
                setflag = false;
            }
        }
        else if (setTimer >= 0.6f)
        {
            setflag = false;
            setTimer = 0.0f;
        }
        else
            YinYangBall_clone.GetComponent<Rigidbody2D>().velocity = new Vector2(this.gameObject.transform.position.x
        - player.transform.position.x > 0 ? -1.0f : 1.0f * 2.0f, -3.0f);
    }

    Vector2 MoveEVec(Vector3 current, Vector3 target)
    {
        Vector2 vec = (new Vector2(target.x, target.y)
            - new Vector2(current.x, current.y)).normalized;
        return vec;
    }

}
