using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TalkWindowController : MonoBehaviour
{
    [HideInInspector]
    public bool endjudge;
    [HideInInspector]
    public int talk_num;
    [SerializeField]
    private TextMeshProUGUI name_TMP;
    [SerializeField]
    private TextMeshProUGUI talk_TMP;
    [SerializeField]
    private GameObject empty_chars;
    [SerializeField]
    private List<GameObject> Chars = new List<GameObject>();
    [SerializeField]
    private List<TalkData> talk = new List<TalkData>();
    [SerializeField]
    private List<GameObject> CountDown = new List<GameObject>();
    [SerializeField]
    private GameObject Message;
    [SerializeField]
    private AudioSource bgm;
    private BackGroundChanger BGC;

    void Start()
    {
        bgm.mute = true;
        Message.SetActive(false);
        foreach (var c in CountDown)
            c.SetActive(false);
        foreach (var c in Chars)
            c.SetActive(false);
        endjudge = false;
        BGC = GetComponent<BackGroundChanger>();
        talk_num = 0;
        name_TMP.text = talk[talk_num].Talk;
        if (talk[talk_num].Name == CharactorType.None)
            name_TMP.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        GoNextTalk();
        DrawName();
        DrawTalk();
        BGC.ChangeBackGround();
    }

    void DrawTalk()
    {
        talk_TMP.text = talk[talk_num].Talk;
    }

    void GoNextTalk()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (talk.Count - 1 <= talk_num)
                endjudge = true;
            else
                talk_num++;
        }
    }

    void DrawName()
    {
        if (talk[talk_num].Name == CharactorType.None)
        {
            foreach (var c in Chars)
                c.SetActive(false);
            name_TMP.text = "";
        }
        else
        {
            if (talk[talk_num].Name == CharactorType.Star)
            {
                name_TMP.text = "スターサファイア";
                foreach (var c in Chars)
                    c.SetActive(false);
                if (SceneManager.GetActiveScene().name == "S1FIRST" && talk_num < 57)
                    empty_chars.transform.Find("Star1").gameObject.SetActive(true);
                else
                    empty_chars.transform.Find("Star2").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Luna)
            {
                name_TMP.text = "ルナチャイルド";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Luna").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Sunny)
            {
                name_TMP.text = "サニーミルク";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Sunny").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Marisa)
            {
                name_TMP.text = "霧雨魔理沙";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Marisa").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Cirno)
            {
                name_TMP.text = "チルノ";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Cirno").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Okuu)
            {
                name_TMP.text = "霊烏路空";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Okuu").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.Reimu)
            {
                name_TMP.text = "博麗霊夢";
                foreach (var c in Chars)
                    c.SetActive(false);
                empty_chars.transform.Find("Reimu").gameObject.SetActive(true);
            }
            if (talk[talk_num].Name == CharactorType.TreeFairys)
            {
                name_TMP.text = "三人";
                foreach (var c in Chars)
                    c.SetActive(false);
            }
        }
    }
}