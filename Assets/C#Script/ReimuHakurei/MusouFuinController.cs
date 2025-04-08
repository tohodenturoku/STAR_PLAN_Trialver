using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 霊夢のスペカ3 霊符「夢想封印」の軌道周りの処理を記述したクラス
/// </summary>
public class MusouFuinController : MonoBehaviour
{
    [HideInInspector]
    public bool fireflag;
    [HideInInspector]
    public bool moveflag;
    [HideInInspector]
    public bool freezeflag;
    [HideInInspector]
    public bool returnflag;
    [SerializeField]
    private GameObject ChaserBullet;
    [SerializeField]
    private float movespeed;
    [SerializeField]
    private List<GameObject> freeze = new List<GameObject>();
    private Animator animator;
    private Vector2 BulletPosition;
    private int fireCount;
    private float fireTimer;
    private int MoveVecJudge;
    private float recastTimer;

    // Start is called before the first frame update
    void Start()
    {
        recastTimer = 0.0f;
        fireCount = 0;
        fireflag = false;
        moveflag = false;
        returnflag = false;
        animator = this.gameObject.GetComponent<Animator>();
        BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
    }

    void Update()
    {
        BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
    }

    public void ReturnSpell()
    {
        recastTimer += Time.deltaTime;
        if (recastTimer > 2.15f)
        {
            fireflag = false;
            moveflag = false;
            returnflag = false;
            freezeflag = false;
            recastTimer = 0.0f;
        }
    }

    public void FireBullet()
    {
        fireTimer += Time.deltaTime;
        if (fireCount >= 6)
        {
            fireCount = 0;
            fireflag = false;
            returnflag = true;
        }
        else if (fireTimer > 1.2f || fireCount == 0)
        {
            animator.SetTrigger("EAttack");
            fireCount++;
            GameObject CBulletCopy = Instantiate(ChaserBullet) as GameObject;
            CBulletCopy.GetComponent<MusouFuinBullet>().angleFlag = fireCount % 2 == 0 ? true : false;
            CBulletCopy.transform.position = BulletPosition;
            fireTimer = 0.0f;
        }
    }

    public void MoveJudge()
    {
        MoveVecJudge = Random.Range(0, 10);
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
            fireflag = true;
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

    Vector2 MoveEVec(Vector3 current, Vector3 target)
    {
        Vector2 vec = (new Vector2(target.x, target.y)
            - new Vector2(current.x, current.y)).normalized;
        return vec;
    }
}
