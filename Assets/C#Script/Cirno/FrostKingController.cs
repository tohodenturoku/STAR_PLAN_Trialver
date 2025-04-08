using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チルノのスペカ1 氷王「フロストキング」の軌道周りの処理を記述したクラス
/// </summary>
public class FrostKingController : MonoBehaviour
{
    [HideInInspector]
    public bool isfamiliar;
    [HideInInspector]
    public List<GameObject> familiars;
    [SerializeField]
    private GameObject familiar;
    [SerializeField]
    private GameObject iceball;
    private CirnoBossController cirnoBossController;
    private float timer;
    private List<Vector2> tanhvec_plus;
    private List<Vector2> tanhvec_minus;
    private List<GameObject> iceballs;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        isfamiliar = false;
        timer = 0.0f;
        familiars = new List<GameObject>();
        tanhvec_plus = new List<Vector2>();
        tanhvec_minus = new List<Vector2>();
        cirnoBossController = GetComponent<CirnoBossController>();
        JudgeVector2();
    }

    // 使い魔召喚
    public void SetFamiliar()
    {
        animator.SetTrigger("EAttack");
        animator.SetInteger("EMove", -1);
        for (int i = -1; i < 3; i += 2)
        {
            familiars.Add(Instantiate(familiar,
                        new Vector3(cirnoBossController.cirnopos.x + (i * 6.0f),
                        cirnoBossController.cirnopos.y + 5.2f, 0.0f),
                        Quaternion.identity));
        }
        isfamiliar = true;
    }

    // 使い魔消滅
    public void DestroyFamiliar()
    {
        for (int i = 0; i < 2; i++)
            Destroy(familiars[i]);
    }

    // 単位ベクトル取得
    void JudgeVector2()
    {
        for (float i = 1.0f; i < 5.0f; i++)
        {
            tanhvec_plus.Add((new Vector2(i, tanh(i))).normalized);
            tanhvec_minus.Add((new Vector2(-i, tanh(-i))).normalized);
        }
    }

    // 発射
    public void FireIceball(GameObject familiar)
    {
        timer += Time.deltaTime;
        if (timer < 0.07f)
        {
            foreach (Vector2 direction in tanhvec_plus)
            {
                GameObject newIceball = Instantiate(iceball, familiar.transform.position, Quaternion.identity);
                newIceball.SetActive(false);
                if (familiar.transform.position.x < this.transform.position.x)
                {
                    newIceball.SetActive(true);
                    newIceball.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -1.0f) * direction * 5.0f; // 速度を設定
                }
                else
                {
                    newIceball.SetActive(true);
                    newIceball.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f, -1.0f) * direction * 5.0f;
                }
            }
            foreach (Vector2 direction in tanhvec_minus)
            {
                GameObject newIceball = Instantiate(iceball, familiar.transform.position, Quaternion.identity);
                newIceball.SetActive(false);
                if (familiar.transform.position.x > this.transform.position.x)
                {
                    newIceball.SetActive(true);
                    newIceball.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 1.0f) * direction * 5.0f; // 速度を設定
                }
                else
                {
                    newIceball.SetActive(true);
                    newIceball.GetComponent<Rigidbody2D>().velocity = new Vector2(-1.0f, 1.0f) * direction * 5.0f;
                }
            }
        }
        else
        {
            if (timer > 3.0f)
                timer = 0.0f;
        }
    }

    // 双曲線関数tanh(ハイパボリックタンジェント)
    float tanh(float x)
    {
        float ans = (Mathf.Exp(x) - Mathf.Exp(-x)) / (Mathf.Exp(x) + Mathf.Exp(-x));
        return ans;
    }
}
