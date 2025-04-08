using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// プレイヤーに対する処理クラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    // プレイヤーのHP
    public float HealthPoint = 20f;
    public float CostPoint = 10.0f;
    // プレイヤーのスピードをダウンするトラップのレイヤー
    public LayerMask TrapLayer_Slow;
    // プレイヤーのスピードをアップするトラップのレイヤー
    public LayerMask Accelerate;
    //ダメージ係数
    public int attackpower = 3;
    // 処理を止める時間
    [HideInInspector]
    public float theworld_time;
    // 処理を止めるべきかのフラグ
    [HideInInspector]
    public bool theworld_flag;
    [HideInInspector]
    public bool startflag;
    //移動速度関係
    [HideInInspector]
    public float Gain = 1.0f;
    // プレイヤーの処理が止まっている（true）か否(flase)か
    [HideInInspector]
    public bool Freeze = false;
    [SerializeField]
    private AudioSource danmakuSE;
    // セリフの管理用クラス
    [SerializeField]
    private TalkWindowController talkWindowController;
    // 通常弾幕のプレハブ
    [SerializeField]
    private GameObject Bullet;
    // スペカ"夢想封印"のプレハブ
    [SerializeField]
    private GameObject LockOnBullet;
    [SerializeField]
    private GameObject lockedEnemy;
    [SerializeField]
    private GameObject TargetMarker;
    [SerializeField]
    private GameObject Bomb;
    [SerializeField]
    private GameObject ChaserBullet;
    [SerializeField]
    private GameObject GameOverPanel;
    [SerializeField]
    private LayerMask GroundLayer;
    // プレイヤーの速さ
    [SerializeField]
    private float Speed = 1.0f;
    // ジャンプ力
    [SerializeField]
    private float JumpPower = 500f;
    [SerializeField]
    private GameObject enemy;
    // プレイヤーHP表示用Slider
    [SerializeField]
    private GameObject HPSlider;
    [SerializeField]
    private GameObject CostSlider;
    // 小さい星を回す速さ
    [SerializeField]
    private float star_rotate_speed = 1.0f;
    // 大きい星の素
    [SerializeField]
    private GameObject BigStar;
    // 小さい星の素
    [SerializeField]
    private GameObject Star;
    // 大きい星が回るスピード
    [SerializeField]
    private float Star_speed = 2.3f;
    // 小さい星が流れ星として落ちるスピード
    [SerializeField]
    private float fallspeed = 3.0f;
    // サポートキャラ及びスペルカードの選択
    [SerializeField]
    private SelectWeaponLoader selectWeaponLoader;
    // カウントダウン用UI素材
    [SerializeField]
    private List<GameObject> CountDown = new List<GameObject>();
    // カウントダウン時のメッセージ
    [SerializeField]
    private GameObject Message;
    [SerializeField]
    private AudioSource bgm;
    [SerializeField]
    private GameObject menupanel;
    [SerializeField]
    private GameObject OperationPanel;
    // 大きい星の集合
    private List<GameObject> BigStars;
    // 小さい星の集合
    private List<GameObject> Stars;
    // イラストを動かす
    private Animator animator;
    // 無限ジャンプ対策用
    [HideInInspector] public bool isJumping = false;
    // プレイヤーの向き判定用
    private bool LR;
    //Raycastの変数
    private Vector2 Ray_R;
    private Vector2 Ray_L;
    private float Ray_Depth = 0.93f;
    private float Ray_X = 0.20f; //左右にどれだけずらすか
    // 通常弾幕の座標
    private Vector2 BulletPosition;
    private Rigidbody2D PlRigid;
    private float Move_x;
    private Vector2 Walk;
    private Vector2 Jump;
    // 地面にいるか否かのチェック用
    private bool isGrounded;
    // 最大HP
    private float MaxHP;
    private float MaxCost;
    // 大きな星を設置したかの確認
    private bool setbigstarjudge;
    // 小さな星を設置したかの確認
    private bool setstarjudge;
    // 星間の単位ベクトルs
    private Vector2 evec2_1;
    private Vector2 evec2_2;
    //　流れ星の寿命
    private float falltime;
    // 星を動かすタイミングを見るフラグ
    private bool moveflag;
    // 単位ベクトルを生成するタイミングを見るフラグ
    private bool e_vecmakeflag;
    // ボスのHP
    private BOSSHPController bOSSHPController;
    // 敵自動追尾用AIクラス
    private EnemyMoveController enemyMoveController;
    // 任意のタイミングにてスピードアップを図るためのフラグ
    private bool speedgainflag;
    // 任意のタイミングにてスピードアップを図った時の一定時間
    private float speed_gaintime;
    private Slider hpslider;
    private Slider costslider;
    // 受けるダメージを減らすためのフラグ
    private float damageGain;
    // 受けるダメージを減らす時間
    private float damage_gaintime;
    // 通常弾幕の初期射程距離
    private float initbulletdistance;
    // 通常弾幕の初期射程距離に倍率をかけたい時に使うやつ
    private float bulletdistance_gain;
    // 通常弾幕の初期射程距離に倍率をかける時の時間
    private float bulletdistance_gaintime;
    // 摩擦係数をいじる目的の物理マテリアル
    private PhysicsMaterial2D iceMaterial;
    private PhysicsMaterial2D groundMaterial;
    //弾幕の射出間隔を調整する
    private bool FireAble = true;
    private const float FireBulletTime = 0.1f; //ここの値をいじることで何秒おきに発射するか調整する
    private float FireBulletCoolTime = 0;
    // スペルを出し中(true)か否(false)か
    private bool isSpell;
    private float healtime;
    private int startcount;
    private int menubuttonsindex;
    private List<GameObject> buttons;
    private GameObject cursor;
    private List<TextMeshProUGUI> button_tmps;
    private Color32 init_buttontmpcol = Color.white;
    private bool SetOperationPanelflag;
    private int jumpcount;

    private void Awake()
    {
        Application.targetFrameRate = 120;
    }

    void Start()
    {
        jumpcount = 0;
        startflag = true;
        SetOperationPanelflag = false;
        startcount = 0;
        buttons = new List<GameObject>();
        button_tmps = new List<TextMeshProUGUI>();
        for (int i = 1; i < 5; i++)
        {
            string button_path = "button" + i.ToString();
            var button = menupanel.transform.Find(button_path).gameObject;
            var button_tmp = button.transform.Find("Text").gameObject;
            buttons.Add(button);
            button_tmps.Add(button_tmp.GetComponent<TextMeshProUGUI>());
        }
        cursor = menupanel.transform.Find("Cursor").gameObject;
        menubuttonsindex = 0;
        menupanel.SetActive(false);
        CostSlider.SetActive(true);
        healtime = 0.0f;
        isSpell = false;
        Message.SetActive(false);
        foreach (var c in CountDown)
            c.SetActive(false);
        theworld_flag = true;
        theworld_time = 4.0f;
        groundMaterial = new PhysicsMaterial2D();
        iceMaterial = new PhysicsMaterial2D();
        groundMaterial.friction = 1.0f;
        iceMaterial.friction = 0.1f;
        bulletdistance_gain = 1.0f;
        initbulletdistance = 0.7f;
        damageGain = 1.0f;
        hpslider = HPSlider.GetComponent<Slider>();
        costslider = CostSlider.GetComponent<Slider>();
        speed_gaintime = 0.0f;
        speedgainflag = false;
        if (SceneManager.GetActiveScene().name == "S1ROAD" ||
        SceneManager.GetActiveScene().name == "S2ROAD")
            enemyMoveController = enemy.GetComponent<EnemyMoveController>();
        BigStars = new List<GameObject>();
        Stars = new List<GameObject>();
        moveflag = false;
        e_vecmakeflag = false;
        LR = true;
        falltime = 0.0f;
        setstarjudge = false;
        setbigstarjudge = false;
        BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        PlRigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        MaxHP = HealthPoint;
        MaxCost = CostPoint;
        hpslider.maxValue = 1f;
        hpslider.value = 1f;
        costslider.maxValue = 1f;
        costslider.value = 1f;
        GameOverPanel.SetActive(false);
        for (int i = 0; i < 4; i++)
            BigStars.Add(Instantiate(BigStar, new Vector3(1000.0f, 1000.0f, 0.0f), Quaternion.identity));
        for (int i = 0; i < 12; i++)
            Stars.Add(Instantiate(Star, new Vector3(1000.0f, 1000.0f, 0.0f), Quaternion.identity));
        animator.SetInteger("LR", 1);
    }

    void Update()
    {
        // 回転止め
        StopPLRotate();
        if (startcount >= 1)
            SetMenuPanel();
        // カウントダウン時間計測
        if (theworld_time > -0.6f && startflag)
            theworld_time -= Time.unscaledDeltaTime;
        // プレイヤーメイン処理
        if (theworld_time < -0.5f || !theworld_flag)
        {
            bgm.mute = false;
            LRJudge();
            LRVecSet();
            Ray_Update();
            WalkUpdate();
            JumpUpdate();
            FireBullet();
            IcicleFall();
            MegaFlare();
            MusouFuin();
            TwinkleSapphire();
            SunnyMilk();
            LunaChild();
            MarisaKirisame();
            LimitDamageGain();
            LimitBulletDistanceGain();
            GainSpeed();
            HeelCost();
        }
        else if (startflag || startcount < 1)// カウントダウン
        {
            SetCount();
        }
    }

    void SetMenuPanel()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !theworld_flag)
        {
            menupanel.SetActive(true);
            theworld_flag = true;
            Time.timeScale = 0;
        }
        else if (((menubuttonsindex == 0 && Input.GetKeyUp(KeyCode.Z))
        || Input.GetKeyUp(KeyCode.Escape)) && theworld_flag && !SetOperationPanelflag && !startflag)
        {
            menupanel.SetActive(false);
            theworld_time = 4.0f;
            startflag = true;
        }
        else if (Input.GetKeyUp(KeyCode.Escape) && theworld_flag && startflag)
        {
            Message.SetActive(false);
            startflag = false;
            for (int i = 0; i < 4; i++)
                CountDown[i].SetActive(false);
            menupanel.SetActive(true);
        }
        else if ((menubuttonsindex == 1 && Input.GetKeyUp(KeyCode.Z))
        && theworld_flag)
        {
            SetOperationPanel();
        }
        else if ((menubuttonsindex == 2 && Input.GetKeyUp(KeyCode.Z))
        && theworld_flag)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if ((menubuttonsindex == 3 && Input.GetKeyUp(KeyCode.Z))
        && theworld_flag)
            SceneManager.LoadScene("TITLE");
        if (theworld_flag)
            MoveMenuCorsor();
        if (Input.GetKeyUp(KeyCode.Escape) && SetOperationPanelflag)
        {
            OperationPanel.SetActive(false);
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].SetActive(true);
            SetOperationPanelflag = false;
        }
    }

    void SetOperationPanel()
    {
        if (!SetOperationPanelflag)
        {
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].SetActive(false);
            OperationPanel.SetActive(true);
            SetOperationPanelflag = true;
        }
    }

    void MoveMenuCorsor()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            menubuttonsindex--;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            menubuttonsindex++;
        if (menubuttonsindex < 0)
            menubuttonsindex = 0;
        else if (menubuttonsindex >= buttons.Count)
            menubuttonsindex = buttons.Count - 1;
        RectTransform cursorRectTransform = cursor.GetComponent<RectTransform>();
        RectTransform targetRectTransform = buttons[menubuttonsindex].gameObject.GetComponent<RectTransform>();
        // 現在のpositionを取得
        Vector3 newPosition = cursorRectTransform.position;

        // y座標を目標のy座標に変更
        newPosition.y = targetRectTransform.position.y;

        // 変更後のpositionを再設定
        cursorRectTransform.position = newPosition;
        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != menubuttonsindex)
                button_tmps[i].color = init_buttontmpcol;
            else
                button_tmps[i].color = Color.red;
        }
    }

    void SetCount()
    {
        if (theworld_time <= 4.0f && theworld_time > 3.0f)
        {
            Message.SetActive(true);
            CountDown[3].SetActive(true);
        }
        if (theworld_time <= 3.0f && theworld_time > 2.0f)
        {
            CountDown[3].SetActive(false);
            CountDown[2].SetActive(true);
        }
        if (theworld_time <= 2.0f && theworld_time > 1.0f)
        {
            CountDown[2].SetActive(false);
            CountDown[1].SetActive(true);
        }
        if (theworld_time <= 1.0f && theworld_time > 0.0f)
        {
            CountDown[1].SetActive(false);
            CountDown[0].SetActive(true);
        }
        if (theworld_time <= 0.0f && theworld_time > -0.5f)
        {
            CountDown[0].SetActive(false);
            startcount++;
            Message.SetActive(false);
            theworld_flag = false;
            startflag = false;
            Time.timeScale = 1.0f;
        }
    }

    //左右の判定
    void LRJudge()
    {
        if (Move_x < 0)
        {
            animator.SetInteger("LR", -1);
            LR = false;
        }
        else if (Move_x > 0)
        {
            animator.SetInteger("LR", 1);
            LR = true;
        }
    }

    // 通常弾幕の場所決定
    void LRVecSet()
    {
        if (!LR)
        {
            BulletPosition = new Vector2(this.transform.position.x - 0.5f, this.transform.position.y);
        }
        else
        {
            BulletPosition = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        }
    }

    //Raycast用の変数の値を更新
    void Ray_Update()
    {
        Ray_R = new Vector2(this.transform.position.x + Ray_X, this.transform.position.y);
        Ray_L = new Vector2(this.transform.position.x - Ray_X, this.transform.position.y);
    }

    // 回転止めの処理
    void StopPLRotate()
    {
        if (Move_x == 0)
        {
            PlRigid.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            PlRigid.constraints &= RigidbodyConstraints2D.FreezeRotation | ~RigidbodyConstraints2D.FreezePositionX;
        }
    }

    //移動の処理
    void WalkUpdate()
    {
        Move_x = Input.GetAxis("Horizontal");
        //移動のバフ/デバフの処理
        bool Accel = Physics2D.Raycast(Ray_R, Vector2.down, Ray_Depth, Accelerate) || Physics2D.Raycast(Ray_L, Vector2.down, Ray_Depth, Accelerate);
        bool Slow = Physics2D.Raycast(Ray_R, Vector2.down, Ray_Depth, TrapLayer_Slow) || Physics2D.Raycast(Ray_L, Vector2.down, Ray_Depth, TrapLayer_Slow);
        if (Freeze) Gain = 0;
        else if (Accel || speedgainflag) Gain = 1.5f;
        else if (Slow) Gain = 0.5f;
        else if (!isGrounded) ;
        else if (!speedgainflag) Gain = 1.0f;
        else Gain = 1.0f;
        Walk = new Vector2(Move_x * Speed * Time.deltaTime * Gain, 0.0f);
        transform.Translate(Walk);
        StopPLRotate();
        // Animator用
        if (Move_x > 0f)
        {
            animator.SetInteger("Move", 1);
        }
        else if (Move_x < 0f)
        {
            animator.SetInteger("Move", -1);
        }
        else if (Move_x == 0f)
        {
            animator.SetInteger("Move", 0);
        }
    }

    //接地判定とジャンプ処理
    void JumpUpdate()
    {
        Jump = new Vector2(0.0f, JumpPower);
        isGrounded = Physics2D.Raycast(Ray_R, Vector2.down, Ray_Depth, GroundLayer) || Physics2D.Raycast(Ray_L, Vector2.down, Ray_Depth, GroundLayer);
        Debug.DrawRay(Ray_R, Vector2.down * Ray_Depth, Color.red);
        Debug.DrawRay(Ray_L, Vector2.down * Ray_Depth, Color.red);
        if (isGrounded && (jumpcount == 1 || jumpcount == 0))
        {
            jumpcount = 0;
            isJumping = false;
        }
        else if (!isJumping && jumpcount == 2)
        {
            jumpcount = 0;
            isJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping && !Freeze)
        {
            jumpcount++;
            PlRigid.velocity = new Vector2(PlRigid.velocity.x, 0);
            PlRigid.AddForce(Jump, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    //通常弾
    void FireBullet()
    {
        if (Input.GetKey(KeyCode.Z) && FireAble)
        {
            danmakuSE.Play();
            animator.SetTrigger("Attack");
            // 弾丸の複製
            GameObject BulletCopy = Instantiate(Bullet) as GameObject;
            if (!LR)
            {
                animator.SetInteger("Move", -1);
                animator.SetInteger("LR", -1);
                BulletCopy.transform.Rotate(0.0f, 0.0f, 180.0f);
                BulletCopy.GetComponent<BulletController>().direcition(LR);
            }
            else
            {
                animator.SetInteger("Move", 1);
                animator.SetInteger("LR", 1);
                BulletCopy.GetComponent<BulletController>().direcition(LR);
            }
            BulletCopy.GetComponent<BulletController>().limitTime = initbulletdistance;
            BulletCopy.GetComponent<BulletController>().limitTime *= bulletdistance_gain;
            // 弾丸の位置を調整
            BulletCopy.transform.position = BulletPosition;
            FireAble = false;
        }
        if (!FireAble)
        {
            FireBulletCoolTime += Time.deltaTime;
            if (FireBulletCoolTime >= FireBulletTime)
            {
                FireBulletCoolTime = 0;
                FireAble = true;
            }
        }
    }

    // サポートキャラ"ルナチャイルド
    void LunaChild()
    {
        if (selectWeaponLoader.data.selectChar == 0
        && (SceneManager.GetActiveScene().name == "S1ROAD" ||
        SceneManager.GetActiveScene().name == "S2ROAD"))
        {
            if (Input.GetKeyDown(KeyCode.C) && ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint) && !speedgainflag)
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                speedgainflag = true;
            }
        }
        if (selectWeaponLoader.data.selectChar == 0
        && (SceneManager.GetActiveScene().name == "S1BOSS" ||
        SceneManager.GetActiveScene().name == "S2BOSS"))
        {
            if (Input.GetKeyDown(KeyCode.C) && ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint))
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                HealthPoint += MaxHP / 2;
                if (HealthPoint > MaxHP)
                {
                    HealthPoint = MaxHP;
                }
                hpslider.value = HealthPoint / MaxHP;
            }
        }
    }

    // サポートキャラ"サニーミルク"
    void SunnyMilk()
    {
        if (selectWeaponLoader.data.selectChar == 1
        && (SceneManager.GetActiveScene().name == "S1ROAD" ||
        SceneManager.GetActiveScene().name == "S2ROAD"))
        {
            if (Input.GetKeyDown(KeyCode.C) && ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint) && !enemyMoveController.speeddownflag)
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                enemyMoveController.speeddownflag = true;
            }
        }
        if (selectWeaponLoader.data.selectChar == 1
        && (SceneManager.GetActiveScene().name == "S1BOSS" ||
        SceneManager.GetActiveScene().name == "S2BOSS"))
        {
            Debug.Log(damage_gaintime);
            if (Input.GetKeyDown(KeyCode.C) && ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint) && damageGain == 1.0f)
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                damageGain = 0.5f;
            }
        }
    }

    void LimitDamageGain()
    {
        if (damageGain != 1.0f)
        {
            damage_gaintime += Time.deltaTime;
            if (damageGain >= 6.0f)
            {
                damageGain = 1.0f;
                damage_gaintime = 0.0f;
            }
        }
    }

    // サポートキャラ"霧雨魔理沙"
    void MarisaKirisame()
    {
        if (selectWeaponLoader.data.selectChar == 2
        && (SceneManager.GetActiveScene().name == "S1ROAD" ||
        SceneManager.GetActiveScene().name == "S2ROAD"))
        {
            if (Input.GetKeyDown(KeyCode.C) &&
            ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint))
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                enemyMoveController.HitPoint -= enemyMoveController.initHitpoint * 0.25f;
            }
        }
        if (selectWeaponLoader.data.selectChar == 2
        && (SceneManager.GetActiveScene().name == "S1BOSS" ||
        SceneManager.GetActiveScene().name == "S2BOSS"))
        {
            if (Input.GetKeyDown(KeyCode.C)
            && ((selectWeaponLoader.data.charCost * 1.0f)
            <= CostPoint) && bulletdistance_gain == 1.0f)
            {
                TakeCost((selectWeaponLoader.data.charCost * 1.0f));
                bulletdistance_gain = 2.0f;
            }
        }
    }

    void LimitBulletDistanceGain()
    {
        if (bulletdistance_gain != 1.0f)
        {
            bulletdistance_gaintime += Time.deltaTime;
            if (bulletdistance_gaintime >= 4.0f)
            {
                bulletdistance_gain = 1.0f;
                bulletdistance_gaintime = 0.0f;
            }
        }
    }

    // 氷符「アイシクルフォール」
    void IcicleFall()
    {
        if (!isSpell && Input.GetKeyUp(KeyCode.X) && (selectWeaponLoader.data.selectSpell == 1)
        && ((selectWeaponLoader.data.spellCost * 1.0f) <= CostPoint))
        {
            TakeCost((selectWeaponLoader.data.spellCost * 1.0f));
            isSpell = true;
            animator.SetTrigger("Attack");
            FindNearestEnemy();
            if (lockedEnemy != null)
            {
                GameObject LCBulletCopyL = Instantiate(LockOnBullet) as GameObject;
                GameObject LCBulletCopyR = Instantiate(LockOnBullet) as GameObject;
                LCBulletCopyL.GetComponent<LockOnBullet>().DecideAngle(300f);
                LCBulletCopyR.GetComponent<LockOnBullet>().DecideAngle(240f);
                LCBulletCopyL.transform.position = new Vector2(lockedEnemy.transform.position.x - 1.5f, lockedEnemy.transform.position.y + 2.0f);
                LCBulletCopyR.transform.position = new Vector2(lockedEnemy.transform.position.x + 1.5f, lockedEnemy.transform.position.y + 2.0f);
                GameObject TargetMarkerCopy = Instantiate(TargetMarker) as GameObject;
                TargetMarkerCopy.transform.position = lockedEnemy.transform.position;
            }
            isSpell = false;
        }
    }

    // 爆符「メガフレア」
    void MegaFlare()
    {
        if (!isSpell && Input.GetKeyUp(KeyCode.X) && (selectWeaponLoader.data.selectSpell == 2)
        && ((selectWeaponLoader.data.spellCost * 1.0f) <= CostPoint))
        {
            TakeCost((selectWeaponLoader.data.spellCost * 1.0f));
            isSpell = true;
            animator.SetTrigger("Attack");
            GameObject BombCopy = Instantiate(Bomb) as GameObject;
            BombCopy.GetComponent<BombController>().direcition(LR);
            BombCopy.transform.position = BulletPosition;
            isSpell = false;
        }
    }

    // 霊符「夢想封印」
    void MusouFuin()
    {
        if (!isSpell && Input.GetKeyUp(KeyCode.X) && (selectWeaponLoader.data.selectSpell == 3)
        && ((selectWeaponLoader.data.spellCost * 1.0f) <= CostPoint))
        {
            TakeCost((selectWeaponLoader.data.spellCost * 1.0f));
            animator.SetTrigger("Attack");
            StartCoroutine(MusouFuinCoroutein());
            isSpell = false;
        }
    }

    IEnumerator MusouFuinCoroutein()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject BulletCopy = Instantiate(ChaserBullet) as GameObject;
            BulletCopy.GetComponent<ChaserBulletController>().angleFlag = i % 2 == 0 ? true : false;
            BulletCopy.transform.localScale += new Vector3(2.0f, 2.0f, 0.0f);
            BulletCopy.transform.position = BulletPosition;
            yield return new WaitForSeconds(0.63f);
        }
    }

    //ダメージ処理
    public void TakeDamage(float damage)
    {
        //小数点以下を切り捨てる
        damage = Mathf.Floor(damage * damageGain);
        //1未満になるようなら1にする
        if (damage < 1.0f)
            damage = 1.0f;
        HealthPoint -= damage;
        hpslider.value = HealthPoint / MaxHP;
        // もしHPが0以下になったら、プレイヤーを破棄
        if (HealthPoint <= 0)
        {
            this.gameObject.SetActive(false);
            GameOverPanel.SetActive(true);
        }
    }

    public void TakeCost(float cost)
    {
        CostPoint -= cost;
        Debug.Log(CostPoint);
        if (CostPoint < 0)
            CostPoint = 0;
        costslider.value = CostPoint / MaxCost;
    }

    //一番近い距離の敵を見つける
    void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  //Enemyタグがついたオブジェクトの配列を作成
        float nearestDistance = float.MaxValue;  //プレイヤーとの距離を取る変数(初期値はfloat型の最大値)
                                                 //lockedEnemyに最も距離が近いものを代入する
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                lockedEnemy = enemy;
            }
        }
    }

    // 星符「トゥインクルサファイア」
    void TwinkleSapphire()
    {
        if (!isSpell && Input.GetKeyUp(KeyCode.X) && (selectWeaponLoader.data.selectSpell == 0)
        && ((selectWeaponLoader.data.spellCost * 1.0f) <= CostPoint))
        {
            TakeCost((selectWeaponLoader.data.spellCost * 1.0f));
            isSpell = true;
            if (!setbigstarjudge)
                SetBigStar();
            if (!setstarjudge)
                SetStar();
            e_vecmakeflag = true;
        }
        if (e_vecmakeflag)
        {
            evec2_1 = GetStarEVec(0);
            evec2_2 = GetStarEVec(6);
            moveflag = true;
            e_vecmakeflag = false;
        }
        if (moveflag)
        {
            MoveFallStar(evec2_1, evec2_2);
            RotateBigStar();
        }
    }

    // 大きな星を設置する
    void SetBigStar()
    {
        for (int i = 0; i < 2; i++)
        {
            BigStars[i].SetActive(true);
            BigStars[i + 2].SetActive(true);
        }
        for (int i = 0; i < 2; i++)
        {
            if (!LR)
            {
                BigStars[i].transform.position = this.transform.position - new Vector3(1.5f + i, -0.5f, 0.0f);
                BigStars[i + 2].transform.position = this.transform.position - new Vector3(1.5f + i, 0.5f, 0.0f);
            }
            else if (LR)
            {
                BigStars[i].transform.position = this.transform.position + new Vector3(1.5f + i, -0.5f, 0.0f);
                BigStars[i + 2].transform.position = this.transform.position + new Vector3(1.5f + i, 0.5f, 0.0f);
            }
        }
        setbigstarjudge = true;
    }

    // 大きい星を回す
    void RotateBigStar()
    {
        if (falltime < 3.0f)
        {
            for (int i = 0; i < 2; i++)
            {
                BigStars[i].transform.Rotate(new Vector3(0.0f, 0.0f, 360.0f) * Time.deltaTime);
                BigStars[i + 2].transform.Rotate(new Vector3(0.0f, 0.0f, 360.0f) * Time.deltaTime);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                BigStars[i].SetActive(false);
                BigStars[i + 2].SetActive(false);
            }
            setbigstarjudge = false;
            moveflag = false;
            setstarjudge = false;
            isSpell = false;
        }
    }

    // 小さい星を設置する
    void SetStar()
    {
        for (int i = 0; i < 6; i++)
        {
            Stars[i].SetActive(true);
            Stars[i + 6].SetActive(true);
        }
        for (int i = 0; i < 6; i++)
        {
            if (!LR)
            {
                Stars[i].transform.position = this.transform.position - new Vector3(0.0f - (i * 0.1f), -3.0f - (i * 0.4f), 0.0f);
                Stars[i + 6].transform.position = this.transform.position - new Vector3(1.0f - (i * 0.1f), -3.0f - (i * 0.4f), 0.0f);
            }
            else if (LR)
            {
                Stars[i].transform.position = this.transform.position + new Vector3(0.0f + (i * 0.1f), 3.0f - (i * 0.4f), 0.0f);
                Stars[i + 6].transform.position = this.transform.position + new Vector3(1.0f + (i * 0.1f), 3.0f - (i * 0.4f), 0.0f);
            }
        }
        setstarjudge = true;
        falltime = 0.0f;
    }

    // 流れ星
    void MoveFallStar(Vector2 vec2_1, Vector2 vec2_2)
    {
        falltime += Time.deltaTime;
        if (falltime <= 2.0f)
        {
            for (int i = 0; i < 6; i++)
            {
                Stars[i].transform.Translate(new Vector3(vec2_1.x, vec2_1.y, 0.0f) * Time.deltaTime * fallspeed);
                Stars[i + 6].transform.Translate(new Vector3(vec2_2.x, vec2_2.y, 0.0f) * Time.deltaTime * fallspeed);
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                Stars[i].SetActive(false);
                Stars[i + 6].SetActive(false);
            }
        }
    }

    // 流れ星の向きを出す
    Vector2 GetStarEVec(int starcount)
    {
        Vector2 stare_vec = (new Vector2(BigStars[2].transform.position.x,
        BigStars[2].transform.position.y) -
        new Vector2(Stars[starcount].transform.position.x,
        Stars[starcount].transform.position.y)).normalized;
        return stare_vec;
    }

    // コスト自動回復
    void HeelCost()
    {
        healtime += Time.deltaTime;
        if (healtime > 15.0f)
        {
            CostPoint += 1.0f;
            if (CostPoint > MaxCost)
                CostPoint = MaxCost;
            costslider.value = CostPoint / MaxCost;
            healtime = 0.0f;
        }
    }

    // 一定時間速くする
    void GainSpeed()
    {
        if (speedgainflag)
        {
            speed_gaintime += Time.deltaTime;
            if (speed_gaintime >= 3.0f)
            {
                speedgainflag = false;
                speed_gaintime = 0.0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "IceFloor")
        {
            PlRigid.sharedMaterial = iceMaterial;
        }
        else
        {
            PlRigid.sharedMaterial = groundMaterial;
        }
        if (other.gameObject.tag == "Enemy" && (SceneManager.GetActiveScene().name == "S1ROAD" ||
        SceneManager.GetActiveScene().name == "S2ROAD"))
        {
            TakeDamage(1e5f);
        }
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Enemy" && (SceneManager.GetActiveScene().name == "S1BOSS" ||
        SceneManager.GetActiveScene().name == "S2BOSS"))
        {
            TakeDamage(1.0f);
        }
        if (c.gameObject.tag == "iceshot" && SceneManager.GetActiveScene().name == "S2BOSS")
        {
            TakeDamage(4.0f);
        }
    }
}