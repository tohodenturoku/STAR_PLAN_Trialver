using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス戦霊烏路空のAIクラス
/// </summary>
public class UtuhoReiuziBossController : MonoBehaviour
{
    // 右端の上である印
    public GameObject up_r;
    // 右端の下である印
    public GameObject under_r;
    // 右端の上と下の中間である印
    public GameObject center_r;
    // spellのフラグ
    public bool[] spellflags;
    // ボス部屋の左端側x座標
    public GameObject BorderLine1;
    // ボス部屋の右端側x座標
    public GameObject BorderLine2;
    // ボス戦終了のフラグ
    [HideInInspector]
    public bool endbossflag;
    [HideInInspector]
    public Vector2 okuupos;
    [SerializeField]
    private ShakeController shakeController;
    [HideInInspector]
    public float sp1_movetime;
    // ボス部屋の真ん中x座標
    [HideInInspector]
    public Vector3 BossZoneCenter;
    [SerializeField]
    private float move_upspeed = 20.0f;
    // お空の速さ
    [SerializeField]
    private float okuuspeed = 5.0f;
    // spellの個数
    [SerializeField]
    private int spellnum = 3;
    [SerializeField]
    private BOSSHPController bOSSHPController;
    [SerializeField]
    private PlayerController playerController;
    // チルノが真ん中にいるというフラグ
    private bool centerFlag;
    // チルノが右端にいるというフラグ
    private bool rightFlag;
    // 少し浮いたことを表すフラグ
    private bool upflag;
    private Animator animator;
    private float theworld_time;
    private float bosskilltime;
    private InfernoBurnerController infernoBurnerController;
    private DiffuseHellFireController diffuseHellFireController;
    private MegaFlareController megaFlareController;

    // Start is called before the first frame update
    void Start()
    {
        infernoBurnerController = this.gameObject.GetComponent<InfernoBurnerController>();
        diffuseHellFireController = this.gameObject.GetComponent<DiffuseHellFireController>();
        megaFlareController = this.gameObject.GetComponent<MegaFlareController>();
        theworld_time = 4.0f;
        sp1_movetime = 0.0f;
        spellflags = new bool[spellnum];
        for (int i = 0; i < spellnum; i++)
            spellflags[i] = false;
        BossZoneCenterJudge();
        animator = GetComponent<Animator>();
        endbossflag = false;
        centerFlag = false;
        rightFlag = false;
        upflag = false;
        animator.SetInteger("ELR", -1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.theworld_time < -0.5f || !playerController.theworld_flag)
        {
            NextSpell();
            okuupos = new Vector2(this.transform.position.x, this.transform.position.y);
            if (spellflags[0])
                spell1();
            if (spellflags[1])
                spell2();
            if (spellflags[2])
                spell3();
            if (!spellflags[0] && !spellflags[1] && !spellflags[2])
            {
                megaFlareController.StopBom();
                bosskilltime += Time.deltaTime;
                if (bosskilltime > 1.0f)
                    endbossflag = true;
                else
                    shakeController.StartShake(1.0f, 0.1f, 5.0f);
            }
        }

    }

    // スペル1
    void spell1()
    {
        if (!centerFlag)
            Move_To_Center();
        if (centerFlag && !upflag)
        {
            float distance = Vector2.Distance(new Vector2(this.transform.position.x
            , this.transform.position.y),
             new Vector2(this.transform.position.x
            , up_r.transform.position.y));
            if (distance < 0.1f)
            {
                this.transform.position = new Vector2(this.transform.position.x
                , up_r.transform.position.y);
                upflag = true;
            }
            else
            {
                Vector2 direction = (new Vector2(this.transform.position.x
                , up_r.transform.position.y)
                - new Vector2(this.transform.position.x,
                this.transform.position.y)).normalized;
                this.transform.Translate(direction * move_upspeed * Time.deltaTime);
            }
        }
        if (centerFlag && upflag)
        {
            sp1_movetime += Time.deltaTime;
            if (sp1_movetime >= 4.0f)
            {
                if (infernoBurnerController.ismove)
                    infernoBurnerController.JudgeArea();
            }
            infernoBurnerController.MoveArea();
            if (!infernoBurnerController.setflag)
                infernoBurnerController.SetBom();
            if (infernoBurnerController.setflag)
                infernoBurnerController.MoveBom();
            if (infernoBurnerController.setflag && !infernoBurnerController.setbulletflag)
                infernoBurnerController.SetBomBullet();
            if (infernoBurnerController.setflag && infernoBurnerController.setbulletflag && !infernoBurnerController.firebulletflag)
                infernoBurnerController.FireBomBullet();
        }
    }

    // スペル2
    void spell2()
    {
        if (!diffuseHellFireController.moveflag)
            diffuseHellFireController.MoveJudge();
        if (diffuseHellFireController.moveflag && !diffuseHellFireController.freezeflag)
            diffuseHellFireController.MoveArea();
        if (diffuseHellFireController.freezeflag && !diffuseHellFireController.appearedflag)
            diffuseHellFireController.AppearBom();
        if (diffuseHellFireController.appearedflag && !diffuseHellFireController.setbeamflag)
            diffuseHellFireController.SetBeam();
        if (diffuseHellFireController.setbeamflag && !diffuseHellFireController.firedflag)
            diffuseHellFireController.FireBeam();
        if (diffuseHellFireController.firedflag)
            diffuseHellFireController.DestroySpell();
    }

    void spell3()
    {
        if (!rightFlag)
            Move_To_Right();
        else
        {
            if (megaFlareController.ismove)
                megaFlareController.JudgeArea();
            megaFlareController.MoveArea();
            Debug.Log(megaFlareController.isfire);
            if (!megaFlareController.ismove && megaFlareController.isfire)
                megaFlareController.FireBom();
        }
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
        if (distance < 0.1f)
        {
            animator.SetInteger("EMove", 0);
            this.transform.position = BossZoneCenter;
            centerFlag = true;
        }
        else
        {
            Vector2 direction = (BossZoneCenter - this.transform.position).normalized;
            if (direction.x < 0)
            {
                animator.SetInteger("ELR", -1);
                animator.SetInteger("EMove", -1);
            }
            else
            {
                animator.SetInteger("ELR", 1);
                animator.SetInteger("EMove", 1);
            }
            this.transform.Translate(direction * okuuspeed * Time.deltaTime);
        }
    }

    // 右端へ移動
    void Move_To_Right()
    {
        float distance = Vector2.Distance(this.transform.position,
                         (under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
        if (distance < 0.1f)
        {
            animator.SetInteger("EMove", 0);
            this.transform.position = under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f);
            rightFlag = true;
        }
        else
        {
            Vector2 direction = ((under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)) - this.transform.position).normalized;
            if (direction.x < 0)
            {
                animator.SetInteger("ELR", -1);
                animator.SetInteger("EMove", -1);
            }
            else
            {
                animator.SetInteger("ELR", 1);
                animator.SetInteger("EMove", 1);
            }
            transform.Translate(direction * okuuspeed * Time.deltaTime);
        }
    }

    void NextSpell()
    {
        if (bOSSHPController.HP_BOSS.value > (2.0f / 3.0f) && bOSSHPController.HP_BOSS.value <= 1.0f)
            spellflags[0] = true;
        if (bOSSHPController.HP_BOSS.value > (1.0f / 3.0f) && bOSSHPController.HP_BOSS.value <= (2.0f / 3.0f))
        {
            infernoBurnerController.DestroySpell();
            spellflags[0] = false;
            spellflags[1] = true;
        }
        if (bOSSHPController.HP_BOSS.value > 0.0f && bOSSHPController.HP_BOSS.value <= (1.0f / 3.0f))
        {
            diffuseHellFireController.DestroySpell();
            spellflags[1] = false;
            spellflags[2] = true;
        }
        if (bOSSHPController.HP_BOSS.value == 0.0f)
            spellflags[2] = false;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Star_Bullet")
        {
            bOSSHPController.TakeDamage(playerController.attackpower);
        }
    }
}
