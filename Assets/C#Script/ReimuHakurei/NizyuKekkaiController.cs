using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 霊夢のスペカ1 夢符「二重結界」の軌道周りの処理を記述したクラス
/// 動きやmain部分はここで管理
/// </summary>
public class NizyuKekkaiController : MonoBehaviour
{
    [HideInInspector]
    public bool setkekkaiflag;
    [HideInInspector]
    public bool setbulletflag;
    [HideInInspector]
    public bool recastflag;
    [SerializeField]
    private GameObject kekkai;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private float fireSpeed = 20.0f;
    private List<GameObject> kekkai_clones;
    private List<GameObject> bullet_clones;
    private List<Vector2> fire_direction;
    private List<List<float>> angles;
    // 縦横か斜めかの決定用
    private int angle_num;
    private float fireTimer;
    private float recastTimer;
    private List<Vector2> dist;
    void Start()
    {
        dist = new List<Vector2>();
        dist.Add(new Vector2(2.0f, 0.0f));
        dist.Add(new Vector2(0.0f, 2.0f));
        dist.Add(new Vector2(-2.0f, 0.0f));
        dist.Add(new Vector2(0.0f, -2.0f));
        fire_direction = new List<Vector2>();
        angles = new List<List<float>>();
        angle_num = 0;
        fireTimer = 0.0f;
        recastTimer = 0.0f;
        setkekkaiflag = false;
        setbulletflag = false;
        recastflag = false;
        kekkai_clones = new List<GameObject>();
        bullet_clones = new List<GameObject>();
        for (int i = 2; i <= 8; i += 2)
        {
            List<float> Items = new List<float>();
            Items.Add((i - 1) * 45.0f);
            Items.Add(i * 45.0f);
            angles.Add(Items);
        }
    }

    public void SetKekkai()
    {
        for (int i = 0; i < 2; i++)
        {
            kekkai_clones.Add(Instantiate(kekkai,
            this.transform.position,
            Quaternion.identity));
        }
        ResizeKekkai();
        setkekkaiflag = true;
    }

    void ResizeKekkai()
    {
        kekkai_clones[0].transform.localScale *= 2.6f;
        kekkai_clones[1].transform.localScale *= 4.0f;
    }

    public void DestroySpell()
    {
        foreach (var k in kekkai_clones)
            Destroy(k);
    }

    public void SetBullet()
    {
        if (angle_num == 0)
        {
            for (float i = 1.0f; i >= -1.0f; i -= 2.0f)
            {
                for (float j = 1.0f; j >= -1.0f; j -= 2.0f)
                {
                    // 第1象限→第2象限→第3象限→第4象限の順でクローンを作る様にしている
                    if (i > 0)
                    {
                        bullet_clones.Add(Instantiate(bullet,
                            this.transform.position - new Vector3(j, i, 0.0f),
                            Quaternion.identity));
                        fire_direction.Add(new Vector2(j, i));
                    }
                    else
                    {
                        bullet_clones.Add(Instantiate(bullet,
                            this.transform.position - new Vector3(-j, i, 0.0f),
                            Quaternion.identity));
                        fire_direction.Add(new Vector2(-j, i));
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {

                bullet_clones.Add(Instantiate(bullet,
                    this.transform.position - new Vector3(dist[i].x, dist[i].y, 0.0f),
                    Quaternion.identity));
                fire_direction.Add(new Vector2(dist[i].y, dist[i].x));
            }
        }
        for (int i = 0; i < 4; i++)
        {
            float distance = Vector2.Distance(this.transform.position, bullet_clones[i].transform.position);
            float radian = angles[i][angle_num] * Mathf.Deg2Rad;
            Vector2 offset = new Vector2(Mathf.Cos(radian), Mathf.Sin(radian) * distance);
            Vector2 newPosition = (Vector2)this.transform.position + offset;
            bullet_clones[i].transform.position = newPosition;
            bullet_clones[i].transform.rotation = Quaternion.Euler(0, 0, angles[i][angle_num] + 90.0f);
        }
        setbulletflag = true;
    }

    public void FireBullet()
    {
        for (int i = 0; i < 4; i++)
        {
            bullet_clones[i].GetComponent<Rigidbody2D>().velocity
                = fire_direction[i] * fireSpeed;
        }
        bullet_clones.Clear();
        fire_direction.Clear();
        setbulletflag = false;
    }
    public void FireTimeCount()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= 2.0f)
        {
            recastflag = true;
            fireTimer = 0.0f;
        }
    }

    public void RecastTimeCount()
    {
        recastTimer += Time.deltaTime;
        if (recastTimer >= 2.5f)
        {
            recastflag = false;
            recastTimer = 0.0f;
            angle_num = angle_num == 0 ? 1 : 0;
        }
    }
}
