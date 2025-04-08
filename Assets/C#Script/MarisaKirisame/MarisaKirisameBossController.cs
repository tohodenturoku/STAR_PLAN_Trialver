using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス戦霧雨魔理沙のAIクラス
/// 動きやmain部分はここで管理
/// </summary>
public class MarisaKirisameBossController : MonoBehaviour
{
    // 右端の上である印
    public GameObject up_r;
    // 右端の下である印
    public GameObject under_r;
    // 右端の上と下の中間である印
    public GameObject center_r;
    // spellのフラグ
    public bool[] spellflags;
    // ボス部屋の真ん中x座標
    [HideInInspector]
    public Vector3 BossZoneCenter;
    // ボス戦終了のフラグ
    [HideInInspector]
    public bool endbossflag;
    [SerializeField]
    private ShakeController shakeController;
    // ボス部屋の左端側x座標
    [SerializeField]
    private GameObject BorderLine1;
    // ボス部屋の右端側x座標
    [SerializeField]
    private GameObject BorderLine2;
    // 魔理沙の速さ
    [SerializeField]
    private float marisaspeed = 5.0f;
    // spellの個数
    [SerializeField]
    private int spellnum = 3;
    [SerializeField]
    private BOSSHPController bOSSHPController;
    // ボス部屋の真ん中x座標
    [SerializeField]
    private PlayerController playerController;
    //タイマー
    private float timer = 0;
    // 魔理沙が真ん中にいるというフラグ
    private bool centerFlag;
    // 魔理沙が右端にいるというフラグ
    private bool rightFlag;
    private bool MagicCircleFlag;
    // spell1関係クラスのインスタンス
    private OrrerysSolerSystemController OSSC;
    // spell2関係クラスのインスタンス
    private MarisaNormalController MNC;
    // spell3関係クラスのインスタンス
    private MastarSparkController MSC;
    private Animator animator;
    private float theworld_time;
    private float bosskilltime;

    // Start is called before the first frame update
    void Start()
    {
        theworld_time = 4.0f;
        // 初期化
        centerFlag = false;
        rightFlag = false;
        OSSC = GetComponent<OrrerysSolerSystemController>();
        MNC = GetComponent<MarisaNormalController>();
        MSC = GetComponent<MastarSparkController>();
        spellflags = new bool[spellnum];
        for (int i = 0; i < spellnum; i++)
            spellflags[i] = false;
        // 真ん中の座標を取得
        BossZoneCenterJudge();
        MagicCircleFlag = false;
        animator = GetComponent<Animator>();
        spellflags[0] = true;
        endbossflag = false;
        animator.SetInteger("ELR", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.theworld_time < -0.5f || !playerController.theworld_flag)
        {
            NextSpell();
            if (spellflags[0])
            {
                spell1();
                if (timer > 3)
                {
                    OSSC.forMarisa = 1 - OSSC.forMarisa;
                    timer = 0;
                }
                timer += Time.deltaTime;
            }
            if (!spellflags[0] && !MagicCircleFlag && !OSSC.destoryflag)
            {
                OSSC.destoryflag = true;
                MagicCircleFlag = true;
            }
            if (OSSC.destoryflag)
            {
                foreach (var m in OSSC.MagicCircleClones)
                    Destroy(m);
                OSSC.destoryflag = false;
            }
            if (spellflags[1])
                spell2();
            if (!spellflags[1] && spellflags[2] && MNC.misailedestroyflag)
            {
                MNC.lifespan = 5.0f;
                MNC.DestroyBullet();
                MNC.misailedestroyflag = false;
            }
            if (spellflags[2])
                spell3();
            if (!spellflags[0] && !spellflags[1] && !spellflags[2])
            {
                MSC.beamlifespan = 5.0f;
                MSC.starlifesapn = 5.0f;
                MSC.DestroyBeam();
                MSC.DestroyStar();
                bosskilltime += Time.deltaTime;
                if (bosskilltime > 1.0f)
                {
                    endbossflag = true;
                }
                else
                {
                    shakeController.StartShake(1.0f, 0.1f, 5.0f);
                }
            }
        }
    }

    void spell1()
    {
        Move_To_Center();
        if (centerFlag)
        {
            OSSC.RotateMagicCircle();
            foreach (var m in OSSC.MagicCircleClones)
                OSSC.Marisa_FireBullet(m);
        }
    }

    void spell2()
    {
        if (rightFlag)
        {
            MNC.GetMarisaPos();
            if (MNC.bulletcharge)
                MNC.SetBullet();
            if (MNC.isfire)
                MNC.FireBullet();
            if (!MNC.ismove && MNC.isfire)
                MNC.DestroyBullet();
            if (MNC.ismove)
                MNC.JudgeArea();
            MNC.MoveArea();
        }
        else
            Move_To_Right();
    }

    void spell3()
    {
        if (rightFlag)
        {
            MSC.GetMarisaPos();
            if (MSC.bulletcharge)
                MSC.SetBeam();
            if (MSC.isfire && !MSC.ismove)
            {
                MSC.DestroyBeam();
                MSC.DestroyStar();
            }
            if (MSC.ismove)
                MSC.JudgeArea();
            MSC.MoveArea();
        }
        else
            Move_To_Right();
    }

    // 真ん中の座標を取得
    void BossZoneCenterJudge()
    {
        BossZoneCenter = new Vector3(0.0f, this.transform.position.y, this.transform.position.z);
        if ((BorderLine1.transform.position.x <= 0 && BorderLine2.transform.position.x <= 0) || (BorderLine1.transform.position.x >= 0 && BorderLine2.transform.position.x >= 0))
            BossZoneCenter.x = (BorderLine2.transform.position.x - BorderLine1.transform.position.x) / 2;
        else
        {
            float half = (BorderLine2.transform.position.x - BorderLine1.transform.position.x) / 2;
            BossZoneCenter.x = BorderLine2.transform.position.x - half;
        }
    }

    // 真ん中へ移動
    void Move_To_Center()
    {
        float distance = Vector2.Distance(this.transform.position, BossZoneCenter);
        if (distance < 0.5f)
        {
            animator.SetInteger("EMove", -2);
            this.transform.position = BossZoneCenter;
            centerFlag = true;
        }
        else
        {
            Vector2 direction = (BossZoneCenter - this.transform.position).normalized;
            if (direction.x < 0)
            {
                animator.SetInteger("EMove", -1);
            }
            else
            {
                animator.SetInteger("EMove", 1);
            }
            this.transform.Translate(direction * marisaspeed * Time.deltaTime);
        }
    }

    // 右端へ移動
    void Move_To_Right()
    {
        float distance = Vector2.Distance(this.transform.position,
                         (under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
        if (distance < 0.5f)
        {
            this.transform.position = under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
            animator.SetInteger("EMove", -2);
            rightFlag = true;
        }
        else
        {
            Vector2 direction = ((under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
            if (direction.x < 0)
            {
                animator.SetInteger("EMove", -1);
            }
            else
            {
                animator.SetInteger("EMove", 1);
            }
            transform.Translate(direction * marisaspeed * Time.deltaTime);
        }
    }

    void NextSpell()
    {
        if (bOSSHPController.HP_BOSS.value > (2.0f / 3.0f) && bOSSHPController.HP_BOSS.value <= 1.0f)
            spellflags[0] = true;
        if (bOSSHPController.HP_BOSS.value > (1.0f / 3.0f) && bOSSHPController.HP_BOSS.value <= (2.0f / 3.0f))
        {
            spellflags[0] = false;
            spellflags[1] = true;
            MNC.misailedestroyflag = true;
        }
        if (bOSSHPController.HP_BOSS.value > 0.0f && bOSSHPController.HP_BOSS.value <= (1.0f / 3.0f))
        {
            spellflags[1] = false;
            spellflags[2] = true;
        }
        if (bOSSHPController.HP_BOSS.value == 0.0f)
        {
            spellflags[2] = false;
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Star_Bullet")
        {
            bOSSHPController.TakeDamage(playerController.attackpower);
        }
    }
}