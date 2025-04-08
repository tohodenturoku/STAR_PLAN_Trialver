using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemyMoveController : MonoBehaviour
{
    [SerializeField] private GameObject Checker1;
    [SerializeField] private GameObject Checker2;
    [SerializeField] private GameObject Checker3;
    [SerializeField] private GameObject Checker4;
    [SerializeField] private GameObject Checker5;
    [SerializeField] private GameObject player;
    EnemyMoveController MOV;
    private TreeEntityChecker PC1;
    private TreeEntityChecker PC2;
    private TreeEntityChecker PC3;
    private TreeEntityChecker PC4;
    private TreeEntityChecker PC5;
    private bool[] B = new bool[6];
    private int Ppos = 0;
    private int Epos = 0;
    void Start()
    {
        PC1 = Checker1.GetComponent<TreeEntityChecker>();
        PC2 = Checker2.GetComponent<TreeEntityChecker>();
        PC3 = Checker3.GetComponent<TreeEntityChecker>();
        PC4 = Checker4.GetComponent<TreeEntityChecker>();
        PC5 = Checker5.GetComponent<TreeEntityChecker>();
        MOV = this.GetComponent<EnemyMoveController>();
        for(int i = 0;i < 6;i++) B[i] = false;
    }

    // Update is called once per frame
    void Update()
    {
        f1();
        getpos();
        MoveControll();
    }
    void getpos(){
        Ppos = (int)(Mathf.Max(PC1.f1(), Mathf.Max(PC2.f1(), Mathf.Max(PC3.f1(), Mathf.Max(PC4.f1(), PC5.f1())))));
        Epos = (int)(Mathf.Max(PC1.f2(), Mathf.Max(PC2.f2(), Mathf.Max(PC3.f2(), Mathf.Max(PC4.f2(), PC5.f2())))));
    }
    void f1(){
        // Debug.Log("Ppos.x = " + player.transform.position.x);
        // Debug.Log("PC1.x = " + PC1.transform.position.x);
        // Debug.Log(PC1.f2());
        
        // Debug.Log("Ppos = "+Ppos + "\tEpos = "+Epos);

        // // Debug.Log("PC1.f1 = " + PC1.P);
        // // Debug.Log("PC2.f1 = " + PC2.P);
        // // Debug.Log("PC3.f1 = " + PC3.P);
        // // Debug.Log("PC4.f1 = " + PC4.P);
        // // Debug.Log("PC5.f1 = " + PC5.P);
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
