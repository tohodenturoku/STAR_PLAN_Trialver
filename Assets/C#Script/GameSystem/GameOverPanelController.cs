using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverPanelController : MonoBehaviour
{
    [SerializeField]
    private GameObject Cursor;
    [SerializeField]
    private List<GameObject> buttons = new List<GameObject>();
    private List<TextMeshProUGUI> buttons_tmp;
    private int buttonindex;
    private Color32 init_buttontmpcol = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        buttons_tmp = new List<TextMeshProUGUI>();
        buttonindex = 0;
        for (int i = 0; i < buttons.Count; i++)
        {
            string path = "Text";
            var button_tmp = buttons[i].transform.Find(path).gameObject;
            buttons_tmp.Add(button_tmp.GetComponent<TextMeshProUGUI>());
        }
    }

    void Update()
    {
        if (buttonindex == 0 && Input.GetKeyUp(KeyCode.Z))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else if (buttonindex == 1 && Input.GetKeyUp(KeyCode.Z))
            SceneManager.LoadScene("TITLE");

        if (Input.GetKeyUp(KeyCode.UpArrow))
            buttonindex--;
        else if (Input.GetKeyUp(KeyCode.DownArrow))
            buttonindex++;
        Debug.Log(buttons.Count);
        // 範囲チェックを先に行う
        if (buttonindex < 0)
            buttonindex = 0;
        else if (buttonindex >= buttons.Count)
            buttonindex = buttons.Count - 1;
        RectTransform cursorRectTransform = Cursor.GetComponent<RectTransform>();
        RectTransform targetRectTransform = buttons[buttonindex].gameObject.GetComponent<RectTransform>();

        Vector3 newPosition = cursorRectTransform.position;
        newPosition.y = targetRectTransform.position.y;
        cursorRectTransform.position = newPosition;

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != buttonindex)
                buttons_tmp[i].color = init_buttontmpcol;
            else
                buttons_tmp[i].color = Color.red;
        }
    }
}
