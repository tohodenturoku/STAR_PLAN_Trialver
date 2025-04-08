using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewGameController : MonoBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> Button_tmps = new List<TextMeshProUGUI>();
    [SerializeField]
    private List<GameObject> Buttons = new List<GameObject>();
    [SerializeField]
    private GameObject START_tmp;
    [SerializeField]
    private GameObject NewGamePanel;
    [SerializeField]
    private GameObject Cursor;
    [SerializeField]
    private GameObject OperationPanel;
    private bool setflag;
    private bool SetOperationPanelflag;
    private int buttonsindex;
    private Color32 init_tmpcol = Color.white;
    private bool chattering_defence;
    private float delay_timer;

    void Start()
    {
        delay_timer = 0.0f;
        chattering_defence = false;
        SetOperationPanelflag = false;
        buttonsindex = 0;
        setflag = false;
        NewGamePanel.SetActive(setflag);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        SetNewGameWindow();
        NewGamePanel.SetActive(setflag);
        MoveButtonCursor();
        if (chattering_defence)
        {
            delay_timer += Time.deltaTime;
            if (delay_timer > 0.2f)
            {
                NewGame();
                SetOperationPanel();
                EndGame();
            }
        }
    }

    void SetNewGameWindow()
    {
        if (!setflag && Input.GetKeyUp(KeyCode.Z))
        {
            START_tmp.SetActive(false);
            chattering_defence = true;
            setflag = true;
        }
    }

    void MoveButtonCursor()
    {
        if (setflag && !SetOperationPanelflag && Input.GetKeyUp(KeyCode.DownArrow))
        {
            if (buttonsindex + 1 < Buttons.Count)
                buttonsindex++;
            if (buttonsindex >= Buttons.Count)
                buttonsindex = 1;
        }
        if (setflag && !SetOperationPanelflag && Input.GetKeyUp(KeyCode.UpArrow))
        {
            if (buttonsindex - 1 >= 0)
                buttonsindex--;
            if (buttonsindex < 0)
                buttonsindex = 0;
        }
        RectTransform cursorRectTransform = Cursor.GetComponent<RectTransform>();
        RectTransform targetRectTransform = Buttons[buttonsindex].gameObject.GetComponent<RectTransform>();
        // 現在のpositionを取得
        Vector3 newPosition = cursorRectTransform.position;

        // y座標を目標のy座標に変更
        newPosition.y = targetRectTransform.position.y;

        // 変更後のpositionを再設定
        cursorRectTransform.position = newPosition;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i != buttonsindex)
                Button_tmps[i].color = init_tmpcol;
            else
                Button_tmps[i].color = Color.red;
        }
    }

    void SetOperationPanel()
    {
        if (setflag && buttonsindex == 1)
        {
            if (Input.GetKeyUp(KeyCode.Z) && !SetOperationPanelflag)
            {
                for (int i = 0; i < Buttons.Count; i++)
                    Buttons[i].SetActive(false);
                OperationPanel.SetActive(true);
                Cursor.SetActive(false);
                SetOperationPanelflag = true;
            }
            if (Input.GetKeyUp(KeyCode.Escape) && SetOperationPanelflag)
            {
                OperationPanel.SetActive(false);
                for (int i = 0; i < Buttons.Count; i++)
                    Buttons[i].SetActive(true);
                SetOperationPanelflag = false;
                Cursor.SetActive(true);
            }
        }
    }

    void NewGame()
    {
        if (setflag && buttonsindex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Z))
                SceneManager.LoadScene("S1FIRST");
        }
    }

    void EndGame()
    {
        if (setflag && buttonsindex == 2)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
                Application.Quit();//ゲームプレイ終了
#endif
            }
        }
    }

}
