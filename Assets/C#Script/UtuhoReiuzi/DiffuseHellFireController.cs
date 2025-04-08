using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// お空のスペカ2 獄光「ディヒューズヘルファイア」の軌道周りの処理を記述したクラス
/// </summary>
public class DiffuseHellFireController : MonoBehaviour
{
    // 動く準備に入ったかどうか判定するフラグ
    [HideInInspector]
    public bool moveflag;
    // 止まっているかどうか判定するフラグ
    [HideInInspector]
    public bool freezeflag;
    // ボムを発射したかの判定フラグ
    [HideInInspector]
    public bool appearedflag;
    [HideInInspector]
    public bool setbeamflag;
    [HideInInspector]
    public bool firedflag;
    [SerializeField]
    private List<GameObject> freeze = new List<GameObject>();
    [SerializeField]
    private float movespeed = 20.0f;
    [SerializeField]
    private GameObject Bom;
    [SerializeField]
    private GameObject Beam;
    [SerializeField]
    private GameObject BeamStart;
    private int MoveVecJudge;
    private Animator animator;
    private List<int> ableMove;
    private List<GameObject> Boms;
    private List<GameObject> Beams;
    private List<GameObject> beamstart_clones;
    private float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        beamstart_clones = new List<GameObject>();
        destroyTime = 0.0f;
        ableMove = new List<int>();
        Boms = new List<GameObject>();
        animator = GetComponent<Animator>();
        firedflag = false;
        moveflag = false;
        freezeflag = false;
        setbeamflag = false;
        appearedflag = false;
        Beams = new List<GameObject>();
    }

    public void MoveJudge()
    {
        MoveVecJudge = UnityEngine.Random.Range(0, 11);
        for (int i = 0; i < 12; i++)
        {
            if (i != MoveVecJudge)
                ableMove.Add(i);
        }
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

    public void AppearBom()
    {
        for (int i = 0; i < 4; i++)
        {
            System.Random r = new System.Random();
            int randIndex = r.Next(ableMove.Count);
            int random = ableMove[randIndex];
            Boms.Add(Instantiate(Bom,
            new Vector3(1000.0f, 1000.0f, 0.0f),
            Quaternion.identity));
            Boms[i].transform.position = freeze[random].transform.position;
            ableMove.Remove(ableMove[randIndex]);
        }
        appearedflag = true;
    }

    public void SetBeam()
    {
        for (int i = 0; i < 3; i++)
        {
            beamstart_clones.Add(Instantiate(BeamStart,
            new Vector3(this.transform.position.x,
            this.transform.position.y - 1.0f, 0.0f),
            Quaternion.identity));
            beamstart_clones[i].transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            Beams.Add(Instantiate(Beam,
            new Vector3(this.transform.position.x,
            this.transform.position.y - 1.0f, 0.0f),
            Quaternion.identity));
            Beams[i].transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
        }
        setbeamflag = true;
    }

    public void FireBeam()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject player = GameObject.Find("Player");
            int direction_to_player = this.gameObject.transform.position.x
            - player.transform.position.x > 0 ? -1 : 1;
            animator.SetTrigger("EAttack");
            animator.SetInteger("EMove", direction_to_player);
            Beams[i + 1].GetComponent<Rigidbody2D>().velocity = new Vector3(i * 5.0f, -5.0f, 0.0f);
        }
        firedflag = true;
    }

    public void DestroySpell()
    {
        destroyTime += Time.deltaTime;
        if (destroyTime >= 6.0f)
        {
            foreach (var b in Beams)
                Destroy(b);
            Beams.Clear();
            foreach (var b in Boms)
                Destroy(b);
            Boms.Clear();
            foreach (var bc in beamstart_clones)
                Destroy(bc);
            beamstart_clones.Clear();
            firedflag = false;
            moveflag = false;
            freezeflag = false;
            setbeamflag = false;
            appearedflag = false;
            destroyTime = 0.0f;
        }
    }

    Vector2 MoveEVec(Vector3 current, Vector3 target)
    {
        Vector2 vec = (new Vector2(target.x, target.y)
            - new Vector2(current.x, current.y)).normalized;
        return vec;
    }
}
