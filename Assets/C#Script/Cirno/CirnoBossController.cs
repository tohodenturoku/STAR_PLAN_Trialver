using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボス戦チルノのAIクラス
/// 動きやmain部分はここで管理
/// </summary>
public class CirnoBossController : MonoBehaviour
{
    // 右端の上である印
    public GameObject up_r;
    // 右端の下である印
    public GameObject under_r;
    // 右端の上と下の中間である印
    public GameObject center_r;
    // spellのフラグ
    public bool[] spellflags;
    // ボス戦終了のフラグ
    [HideInInspector]
    public bool endbossflag;
    [HideInInspector]
    public Vector2 cirnopos;
    // ボス部屋の真ん中x座標
    [HideInInspector]
    public Vector3 BossZoneCenter;
    [SerializeField]
    private float move_upspeed = 20.0f;
    // ボス部屋の左端側x座標
    [SerializeField]
    private GameObject BorderLine1;
    // ボス部屋の右端側x座標
    [SerializeField]
    private GameObject BorderLine2;
    // チルノの速さ
    [SerializeField]
    private float cirnospeed = 5.0f;
    // spellの個数
    [SerializeField]
    private int spellnum = 3;
    [SerializeField]
    private BOSSHPController bOSSHPController;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private ShakeController shakeController;
    // チルノが真ん中にいるというフラグ
    private bool centerFlag;
    // チルノが右端にいるというフラグ
    private bool rightFlag;
    // 少し浮いたことを表すフラグ
    private bool upflag;
    private FrostKingController frostKingController;
    private PerfectFreezeController perfectFreezeController;
    private IcicleFallController icicleFallController;
    private float theworld_time;
    private Animator animator;
    private int center_count;
    private float bosskilltime;

    // Start is called before the first frame update
    void Start()
    {
        center_count = 0;
        animator = GetComponent<Animator>();
        theworld_time = 4.0f;
        endbossflag = false;
        spellflags = new bool[spellnum];
        spellflags[0] = true;
        centerFlag = false;
        rightFlag = false;
        upflag = false;
        frostKingController = GetComponent<FrostKingController>();
        perfectFreezeController = GetComponent<PerfectFreezeController>();
        icicleFallController = GetComponent<IcicleFallController>();
        BossZoneCenterJudge();
        animator.SetInteger("ELR", -1);
    }

    // Update is called once per frame
    void Update()
    {
        theworld_time -= Time.deltaTime;
        if (playerController.theworld_time < -0.5f || !playerController.theworld_flag)
        {
            NextSpell();
            cirnopos = new Vector2(this.transform.position.x, this.transform.position.y);
            if (spellflags[0])
                spell1();
            if (!spellflags[0] && spellflags[1] && !spellflags[2] && center_count == 0)
            {
                center_count++;
                centerFlag = false;
                frostKingController.DestroyFamiliar();
            }
            if (spellflags[1])
                spell2();
            if (!spellflags[0] && !spellflags[1] && spellflags[2] && center_count == 1)
            {
                center_count++;
                centerFlag = false;
                icicleFallController.DestroyIceShot();
            }
            if (spellflags[2])
                spell3();
            if (!spellflags[0] && !spellflags[1] && !spellflags[2])
            {
                perfectFreezeController.DestroyIceball();
                bosskilltime += Time.deltaTime;
                if (bosskilltime > 1.0f)
                    endbossflag = true;
                else
                    shakeController.StartShake(1.0f, 0.1f, 5.0f);
            }
        }
    }

    void spell1()
    {
        if (!centerFlag)
            Move_To_Center();
        if (centerFlag && !frostKingController.isfamiliar)
            frostKingController.SetFamiliar();
        if (frostKingController.isfamiliar)
        {
            frostKingController.FireIceball(frostKingController.familiars[0]);
            frostKingController.FireIceball(frostKingController.familiars[1]);
        }
    }

    void spell2()
    {
        if (!centerFlag)
            Move_To_Center();
        if (centerFlag && !icicleFallController.moveflag)
            icicleFallController.MoveJudge();
        if (icicleFallController.moveflag && !icicleFallController.freezeflag)
            icicleFallController.MoveArea();
        if (icicleFallController.freezeflag && !icicleFallController.setflag)
            icicleFallController.SetIceShot();
        if (icicleFallController.setflag && !icicleFallController.chargeflag)
            icicleFallController.ChargeIceShot();
        if (icicleFallController.chargeflag)
            icicleFallController.FireIceShot();
    }

    void spell3()
    {
        if (!centerFlag)
            Move_To_Center();
        if (centerFlag)
        {
            float distance = Vector2.Distance(new Vector2(this.transform.position.x
            , this.transform.position.y),
             new Vector2(this.transform.position.x
            , center_r.transform.position.y));
            if (distance < 0.5f)
            {
                this.transform.position = new Vector2(this.transform.position.x
                , center_r.transform.position.y);
                upflag = true;
            }
            else
            {
                Vector2 direction = (new Vector2(this.transform.position.x
                , center_r.transform.position.y)
                - new Vector2(this.transform.position.x,
                this.transform.position.y)).normalized;
                this.transform.Translate(direction * move_upspeed * Time.deltaTime);
            }
        }
        if (centerFlag && upflag && !perfectFreezeController.setflag)
            perfectFreezeController.SetIceball();
        if (perfectFreezeController.setflag && !perfectFreezeController.freezeflag && !perfectFreezeController.fallflag)
            perfectFreezeController.FireIceball();
        if (perfectFreezeController.freezeflag && !perfectFreezeController.fallflag)
            perfectFreezeController.FallDelayIceball();
        if (perfectFreezeController.fallflag && !perfectFreezeController.freezeflag)
            perfectFreezeController.FallIceball();
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
            this.transform.Translate(direction * cirnospeed * Time.deltaTime);
        }
    }

    // 右端へ移動
    void Move_To_Right()
    {
        float distance = Vector2.Distance(this.transform.position,
                         (under_r.transform.position - new Vector3(2.0f, 0.0f, 0.0f)));
        if (distance < 0.5f)
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
            transform.Translate(direction * cirnospeed * Time.deltaTime);
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
        }
        if (bOSSHPController.HP_BOSS.value > 0.0f && bOSSHPController.HP_BOSS.value <= (1.0f / 3.0f))
        {
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
