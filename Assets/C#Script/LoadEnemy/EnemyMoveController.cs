using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 道中にて追いかけてくる敵のAIクラス
/// </summary>
public class EnemyMoveController : MonoBehaviour
{
    public float HitPoint = 4f; //スタンするまでのHP
    [HideInInspector]
    public float initHitpoint;
    [HideInInspector]
    public bool speeddownflag;
    // 大ジャンプの通常と比べた時の倍率
    [SerializeField]
    private float BigJMPPower = 1.5f;
    // 超大ジャンプの通常と比べた時の倍率
    [SerializeField]
    private float SuperBigJMPPower = 3.0f;
    // 小ジャンプの通常と比べた時の倍率（割り算）
    [SerializeField]
    private float SmallJMPPower = 4.0f;
    // 超小ジャンプの通常と比べた時の倍率（割り算）
    [SerializeField]
    private float SuperSmallJMPPower = 4.0f;
    // プレイヤーを追いかける基準値
    [SerializeField]
    private float changerange = 1.0f;
    [SerializeField]
    private GameObject player;
    // 敵の速さ
    [SerializeField]
    private float enemy_speed = 10.0f;
    // 敵のジャンプ力
    [SerializeField]
    private float enemyjmp_power = 10.0f;
    // 地面の位置
    [SerializeField]
    private Transform GroundCheck;
    // 地面のレイヤー
    [SerializeField]
    private LayerMask GroundLayer;
    // 地面のレイヤー
    [SerializeField]
    private Slider enemystangauge;
    private Rigidbody2D en_rb;
    // 通常ジャンプのジャンプ力
    private float NormalJMPPower;
    // 地面判定用
    private bool isGrounded;
    // スタン関係
    private bool stun = false; //スタン状態の判定
    const float stuntime = 1f; //スタン時間の調整用
    private float stunDtime = 0.0f;//スタン時間測定用
    [SerializeField] private float HPgain = 4f;
    private float StunCount = 1f;
    //強制移動
    private bool Tracking = true;
    private bool MoveLeft = false;

    private Animator animator;
    private float initenemy_speed;
    private float enemyspeed_downtime;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        enemystangauge.value = 1.0f;
        enemyspeed_downtime = 0.0f;
        en_rb = this.GetComponent<Rigidbody2D>();
        NormalJMPPower = enemyjmp_power;
        animator = GetComponent<Animator>();
        initenemy_speed = enemy_speed;
        speeddownflag = false;
        initHitpoint = HitPoint;
        animator.SetInteger("ELR", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if ((playerController.theworld_time < -0.5f || !playerController.theworld_flag) && player.activeSelf)
        {
            SpeedDownEnemy();
            UpdateStanGauge();
            CheckGround();
            if (Vector2.Distance(player.transform.position,
                this.transform.position) >= changerange && !stun && Tracking)
                Player_Tracking();
            else if (!Tracking)
                ForcedMove();
            if (stun)
                StunControll();
        }
        else
            en_rb.velocity = new Vector2(0.0f, en_rb.velocity.y);
    }

    // プレイヤーを追いかける
    void Player_Tracking()
    {
        float vecdici_x = player.transform.position.x > this.transform.position.x ? 1.0f : -1.0f;
        //アニメーター処理
        if (vecdici_x > 0)
        {
            animator.SetInteger("EMove", 1);
        }
        else if (vecdici_x < 0)
        {
            animator.SetInteger("EMove", -1);
        }
        else if (vecdici_x == 0)
        {
            animator.SetInteger("EMove", 0);
        }
        en_rb.velocity = new Vector2(enemy_speed * vecdici_x, en_rb.velocity.y);
    }

    // 地面と接しているか
    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(this.transform.position, 2.0f, GroundLayer);
    }

    // 敵のジャンプ
    void Enemy_Jump()
    {
        float vecdici_x = MoveLeft ? -1.0f : 1.0f;
        if (isGrounded)
        {
            if (vecdici_x < 0)
                animator.SetTrigger("EJump");
            // else
            //     animator.SetInteger("EJump", 1);
            en_rb.velocity = new Vector2(en_rb.velocity.x, 0);
            en_rb.AddForce(new Vector2(0.0f,
                           enemyjmp_power * Time.deltaTime),
                           ForceMode2D.Impulse);
        }
    }

    //　敵のスタン
    void StunControll()
    {
        stunDtime -= Time.deltaTime;
        en_rb.velocity = new Vector2(0, en_rb.velocity.y);
        if (stunDtime < 0)
        {
            RecoverHitPoint();
            stun = false;
        }
    }

    void ForcedMove()
    {
        float vecdici_x = MoveLeft ? -1.0f : 1.0f;
        en_rb.velocity = new Vector2(enemy_speed * vecdici_x, en_rb.velocity.y);
        if (vecdici_x > 0)
        {
            animator.SetInteger("EMove", 1);
        }
        else if (vecdici_x < 0)
        {
            animator.SetInteger("EMove", -1);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 通常のジャンプ
        if (other.gameObject.CompareTag("JumpZone"))
            Enemy_Jump();
        // 大ジャンプ
        if (other.gameObject.CompareTag("BigJMP"))
        {
            enemyjmp_power *= BigJMPPower;
            Enemy_Jump();
            enemyjmp_power = NormalJMPPower;
        }
        // 小ジャンプ
        if (other.gameObject.CompareTag("SmallJMP"))
        {
            enemyjmp_power /= SmallJMPPower;
            Enemy_Jump();
            enemyjmp_power = NormalJMPPower;
        }
        if (other.gameObject.CompareTag("SuperSmallJMP"))
        {
            enemyjmp_power /= SuperSmallJMPPower;
            Enemy_Jump();
            enemyjmp_power = NormalJMPPower;
        }
        // 超大ジャンプ
        if (other.gameObject.CompareTag("SuperBigJMP"))
        {
            enemyjmp_power *= SuperBigJMPPower;
            Enemy_Jump();
            enemyjmp_power = NormalJMPPower;
        }
        if (other.gameObject.CompareTag("VariableJMP"))
        {
            enemyjmp_power *= other.GetComponent<VariableJumpController>().JMP_Power;
            Enemy_Jump();
            enemyjmp_power = NormalJMPPower;
        }
        //スタン判定
        if (other.gameObject.CompareTag("Star_Bullet") && !stun)
        {
            HitPoint--;
            if (HitPoint <= 0)
            {
                stunDtime = stuntime;
                StunCount++;
                stun = true;
            }
        }
        //強制移動解除
        if (other.gameObject.CompareTag("EnableTrack"))
            EnableTrack();
        if (other.gameObject.CompareTag("GoLeft"))
            GoLeft();
        if (other.gameObject.CompareTag("GoRight"))
            GoRight();
    }
    void OnTriggerStay2D(Collider2D other)
    {
        //強制移動解除
        if (other.gameObject.CompareTag("EnableTrack"))
            EnableTrack();
        if (other.gameObject.CompareTag("GoLeft"))
            GoLeft();
        if (other.gameObject.CompareTag("GoRight"))
            GoRight();
    }
    public void EnableTrack()
    {
        Tracking = true;
    }
    public void GoLeft()
    {
        Tracking = false;
        MoveLeft = true;
    }
    public void GoRight()
    {
        Tracking = false;
        MoveLeft = false;
    }

    void RecoverHitPoint()
    {
        HitPoint = StunCount * HPgain;
        initHitpoint = HitPoint;
    }

    void UpdateStanGauge()
    {
        enemystangauge.value = HitPoint / initHitpoint;
    }

    void SpeedDownEnemy()
    {
        if (speeddownflag)
        {
            enemyspeed_downtime += Time.deltaTime;
            if (enemyspeed_downtime < 3.0f)
                enemy_speed = initenemy_speed / 1.5f;
            else
            {
                enemy_speed = initenemy_speed;
                enemyspeed_downtime = 0.0f;
                speeddownflag = false;
            }
        }
    }
}
