using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Spell選択UIを制御するためのクラス
/// </summary>
public class SpellSelectController : MonoBehaviour
{
    // Spellの順番は
    // 0:星符「トゥインクルサファイア」、1:氷符「アイシクルフォール」、2:爆符「メガフレア」、3:霊符「夢想封印」とすること
    // コストについては
    // 最大を12とし時間経過で回復する物とする
    // Spell管理用List長さは必ず4以下とすること
    public List<Spell> spells = new List<Spell>();
    // spellsのインデックスを管理するpublic変数
    [HideInInspector]
    public int spellsindex;
    // 各Spellの名前を格納するList
    [SerializeField]
    private List<GameObject> SpellNameColumn;
    // 各Spellのコストを格納するList
    [SerializeField]
    private List<GameObject> SpellCostColumn;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject charSelect;
    // メインキャンバスを消すことで面倒になるのでこいつも消す
    [SerializeField]
    private GameObject Vines;
    // playerを消すことで面倒になるのでこいつも消す
    [SerializeField]
    private GameObject HPCostTMPController;
    // 選択UI用指の画像
    [SerializeField]
    private GameObject Finger;
    [SerializeField]
    private GameObject CostSlider;
    [SerializeField]
    private SelectWeaponLoader selectWeaponLoader;
    [SerializeField]
    private AudioSource bgm;
    // 初期の文字色
    private Color initcharcolor;
    void Awake()
    {
        CostSlider.SetActive(false);
        bgm.mute = true;
        player.gameObject.SetActive(false);
        enemy.SetActive(false);
        mainCanvas.SetActive(false);
        charSelect.SetActive(false);
        Vines.SetActive(false);
        HPCostTMPController.SetActive(false);
    }

    void Start()
    {
        DontDestroyOnLoad(this);
        spellsindex = 0;
        selectWeaponLoader.data.selectSpell = spellsindex;
        initcharcolor = new Color(1.0f, 0.9508464f, 0.0f, 1.0f);
    }

    void Update()
    {
        NameDraw();
        CostDraw();
        RecordDelete();
        SpellChange();
        SpellColorChange();
        FingerMove();
        SpellSelect();
    }

    // Spellsに後ろから追加
    void SpellPush(string spellname, int spellcost)
    {
        spells.Add(new Spell(spellname, spellcost));
        // 重複したら削除
        for (int i = 0; i < spells.Count - 1; i++)
        {
            if (new Spell(spellname, spellcost) == spells[i])
                spells.RemoveAt(spells.Count - 1);
        }
    }

    // Spellの名前をTMPに代入するクラス
    void NameDraw()
    {
        for (int i = 0; i < spells.Count; i++)
            SpellNameColumn[i].gameObject.
            GetComponent<TextMeshProUGUI>().text
            = spells[i].spellname;
    }

    // SpellのコストをTMPに代入するクラス
    void CostDraw()
    {
        for (int i = 0; i < spells.Count; i++)
            SpellCostColumn[i].gameObject.
            GetComponent<TextMeshProUGUI>().text
            = spells[i].spellcost.ToString();
    }

    // spellが入っていない場合UIの見た目ごと消す
    void RecordDelete()
    {
        for (int i = spells.Count; i < SpellCostColumn.Count; i++)
        {
            SpellCostColumn[i].SetActive(false);
            SpellNameColumn[i].SetActive(false);
        }
    }

    // 矢印キーでメニューを選択
    void SpellChange()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (spellsindex - 1 >= 0)
            {
                spellsindex--;
                selectWeaponLoader.data.selectSpell = spellsindex;
                selectWeaponLoader.data.spellCost = spells[spellsindex].spellcost;
            }
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (spellsindex + 1 < spells.Count)
            {
                spellsindex++;
                selectWeaponLoader.data.selectSpell = spellsindex;
                selectWeaponLoader.data.spellCost = spells[spellsindex].spellcost;
            }
        }
        // 念の為用
        if (spellsindex < 0)
        {
            spellsindex = 0;
            selectWeaponLoader.data.selectSpell = spellsindex;
            selectWeaponLoader.data.spellCost = spells[spellsindex].spellcost;
        }
        if (spellsindex >= spells.Count)
        {
            spellsindex = spells.Count - 1;
            selectWeaponLoader.data.selectSpell = spellsindex;
            selectWeaponLoader.data.spellCost = spells[spellsindex].spellcost;
        }
        selectWeaponLoader.data.selectSpell = spellsindex;
        selectWeaponLoader.data.spellCost = spells[spellsindex].spellcost;
    }

    // 指のマークを動かす
    void FingerMove()
    {
        RectTransform fingerRectTransform = Finger.GetComponent<RectTransform>();
        RectTransform targetRectTransform = SpellNameColumn[spellsindex].gameObject.GetComponent<RectTransform>();
        // 現在のpositionを取得
        Vector3 newPosition = fingerRectTransform.position;

        // y座標を目標のy座標に変更
        newPosition.y = targetRectTransform.position.y;

        // 変更後のpositionを再設定
        fingerRectTransform.position = newPosition;
    }

    // 選択している部分の色黄色から赤に変える
    void SpellColorChange()
    {
        SpellNameColumn[spellsindex].gameObject.
                    GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, 0.8f);
        SpellCostColumn[spellsindex].gameObject.
                    GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, 0.8f);
        for (int i = 0; i < spells.Count; i++)
        {
            if (i == spellsindex)
                continue;
            SpellNameColumn[i].gameObject.
                    GetComponent<TextMeshProUGUI>().color = initcharcolor;
            SpellCostColumn[i].gameObject.
                    GetComponent<TextMeshProUGUI>().color = initcharcolor;
        }
    }

    // Zキーで選択を行う
    void SpellSelect()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            charSelect.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}

/// <summary>
/// Spellのプロパティを実装したタプルっぽい機能を持ったクラス
/// </summary>
[System.Serializable]
public class Spell
{
    public string spellname;
    public int spellcost;
    public Spell(string _spellname, int _spellcost)
    {
        this.spellname = _spellname;
        this.spellcost = _spellcost;
    }
}
