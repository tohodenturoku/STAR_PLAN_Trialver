using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveEnemyMoveController : MonoBehaviour
{
    [SerializeField] private GameObject Checker1;
    [SerializeField] private GameObject Checker2;
    [SerializeField] private GameObject Checker3;
    [SerializeField] private GameObject player;
    EnemyMoveController MOV;
    private TreeEntityChecker PC1;
    private TreeEntityChecker PC2;
    private TreeEntityChecker PC3;
    private bool[] B = new bool[4];
    private int Ppos = 0;
    private int Epos = 0;
    void Start()
    {
        PC1 = Checker1.GetComponent<TreeEntityChecker>();
        PC2 = Checker2.GetComponent<TreeEntityChecker>();
        PC3 = Checker3.GetComponent<TreeEntityChecker>();
        MOV = this.GetComponent<EnemyMoveController>();
        for(int i = 0;i < 4;i++) B[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        getpos();
        MoveControll();
    }
    void getpos(){
        Ppos = (int)(Mathf.Max(PC1.f1(), Mathf.Max(PC2.f1(), PC3.f1())));
        Epos = (int)(Mathf.Max(PC1.f2(), Mathf.Max(PC2.f2(), PC3.f2())));
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("EntityCheck"))
            other.GetComponent<TreeEntityChecker>().E = true;
    }
    void OnTriggerExit2D(Collider2D other){
        if(other.CompareTag("EntityCheck"))
            other.GetComponent<TreeEntityChecker>().E = false;
    }
    void MoveControll(){//MEMO:プレイヤーだけ木の中は考えなくても良いようにenabletrakingを置く
        if(Ppos != 0 && Epos  != 0){//両者木の中
            if(Epos == Ppos)
                MOV.EnableTrack();
            else if(Epos < Ppos){
                if(Epos%2 == 1) MOV.GoRight();
                else MOV.GoLeft();
            }
            else if(Epos > Ppos){
                if(Epos%2 == 1) MOV.GoLeft();
                else MOV.GoRight();
            }
        }
        else if(Epos != 0){//敵だけ木の中
            //一旦一番下の判定の場所を基準に//
            if(player.transform.position.x < PC1.transform.position.x){//プレイヤー左側
                if(Epos%2 == 1) MOV.GoLeft();
                else MOV.GoRight();
                // Debug.Log("GOGOLEFT");
            }
            else{//プレイヤー右側
                if(Epos%2 == 1) MOV.GoRight();
                else MOV.GoLeft();
                // Debug.Log("GOGORIGHT");
            }
        }
    }
}
