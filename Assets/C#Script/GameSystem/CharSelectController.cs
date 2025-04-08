using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// サポートキャラクタ選択UIを制御するクラス
/// </summary>
public class CharSelectController : MonoBehaviour
{
    // SupportChara管理用List長さは必ず3以下とすること
    public List<SupportChara> chars = new List<SupportChara>();
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject MapNavi;
    [SerializeField]
    private GameObject MapNaviController;
    [SerializeField]
    private GameObject HPSlider;
    [SerializeField]
    private GameObject mainCanvas;
    [SerializeField]
    private GameObject talkChars;
    [SerializeField]
    private GameObject Balloon;
    [SerializeField]
    private GameObject spellSelect;
    [SerializeField]
    private List<GameObject> CharNameColumn;
    [SerializeField]
    private GameObject Finger;
    [SerializeField]
    private SelectWeaponLoader selectWeaponLoader;
    [SerializeField]
    private TextMeshProUGUI explanation;
    [SerializeField]
    private TextMeshProUGUI cost;
    private Color initcharcolor;
    private int charsindex;
    // Start is called before the first frame update

    void Awake()
    {
        player.gameObject.SetActive(false);
        enemy.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(false);
    }
    void Start()
    {
        charsindex = 0;
        selectWeaponLoader.data.selectChar = charsindex;
        initcharcolor = new Color(1.0f, 0.9508464f, 0.0f, 1.0f);
        NameDraw();
        RecordDelete();
    }

    // Update is called once per frame
    void Update()
    {
        CharaChange();
        FingerMove();
        CharColorChange();
        CharSelect();
        CharaExplanation();
        CostDraw();
    }

    // 名前を描く
    void NameDraw()
    {
        for (int i = 0; i < chars.Count; i++)
            CharNameColumn[i].gameObject.
            GetComponent<TextMeshProUGUI>().text
            = chars[i].charname;
    }

    void CostDraw()
    {
        cost.text = "必要コスト：" + chars[charsindex].charcost.ToString();
    }

    // Listに入れる
    void CharaPush(string charname, int charcost, string charexplanation)
    {
        chars.Add(new SupportChara(charname, charcost, charexplanation));
        // 重複したら削除
        for (int i = 0; i < chars.Count - 1; i++)
        {
            if (new SupportChara(charname, charcost, charexplanation) == chars[i])
                chars.RemoveAt(chars.Count - 1);
        }
    }

    // charが入っていない場合UIの見た目ごと消す
    void RecordDelete()
    {
        for (int i = chars.Count; i < CharNameColumn.Count; i++)
            CharNameColumn[i].SetActive(false);
    }

    // 矢印キーでメニューを選択
    void CharaChange()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            if (charsindex - 1 >= 0)
            {
                charsindex--;
                selectWeaponLoader.data.selectChar = charsindex;
                selectWeaponLoader.data.charCost = chars[charsindex].charcost;
            }
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (charsindex + 1 < chars.Count)
            {
                charsindex++;
                selectWeaponLoader.data.selectChar = charsindex;
                selectWeaponLoader.data.charCost = chars[charsindex].charcost;
            }
        }
        if (charsindex < 0)
        {
            charsindex = 0;
            selectWeaponLoader.data.selectChar = charsindex;
            selectWeaponLoader.data.charCost = chars[charsindex].charcost;
        }
        if (charsindex >= chars.Count)
        {
            charsindex = chars.Count - 1;
            selectWeaponLoader.data.selectChar = charsindex;
            selectWeaponLoader.data.charCost = chars[charsindex].charcost;
        }
        selectWeaponLoader.data.selectChar = charsindex;
        selectWeaponLoader.data.charCost = chars[charsindex].charcost;
    }

    // 指のマークを動かす
    void FingerMove()
    {
        RectTransform fingerRectTransform = Finger.GetComponent<RectTransform>();
        RectTransform targetRectTransform = CharNameColumn[charsindex].gameObject.GetComponent<RectTransform>();
        // 現在のpositionを取得
        Vector3 newPosition = fingerRectTransform.position;

        // y座標を目標のy座標に変更
        newPosition.x = targetRectTransform.position.x;

        // 変更後のpositionを再設定
        fingerRectTransform.position = newPosition;
    }

    // 選択している部分の色黄色から赤に変える
    void CharColorChange()
    {
        CharNameColumn[charsindex].gameObject.
            GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f, 0.8f);
        for (int i = 0; i < chars.Count; i++)
        {
            if (i == charsindex)
                continue;
            CharNameColumn[i].gameObject.
            GetComponent<TextMeshProUGUI>().color = initcharcolor;
        }
    }

    // Zキーで選択を行う
    void CharSelect()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            this.gameObject.SetActive(false);
            mainCanvas.gameObject.SetActive(true);
            HPSlider.gameObject.SetActive(false);
            MapNavi.gameObject.SetActive(false);
            MapNaviController.gameObject.SetActive(false);
            talkChars.gameObject.SetActive(true);
            Balloon.gameObject.SetActive(true);
        }
    }

    void CharaExplanation()
    {
        explanation.text = chars[charsindex].charexplanation;
    }
}

/// </summary>
[System.Serializable]
public class SupportChara
{
    public string charname;
    public int charcost;
    [Multiline(5)]
    public string charexplanation;
    public SupportChara(string _charname, int _charcost, string _charexplanation)
    {
        this.charname = _charname;
        this.charcost = _charcost;
        this.charexplanation = _charexplanation;
    }
}
