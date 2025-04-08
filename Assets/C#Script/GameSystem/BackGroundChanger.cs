using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackGroundChanger : MonoBehaviour
{
    [SerializeField]
    private List<BackGroundImageData> background = new List<BackGroundImageData>();
    private TalkWindowController TWC;
    private int talkNum;

    void Awake()
    {
        talkNum = 0;
        string sceneText = SceneManager.GetActiveScene().name;
        if (sceneText.Substring(sceneText.Length - 5) == "FIRST")
        {
            background[talkNum].BackGroundImage.SetActive(true);
            for (int i = talkNum + 1; i < background.Count; i++)
                background[i].BackGroundImage.SetActive(false);
        }
    }

    void Start()
    {
        TWC = GetComponent<TalkWindowController>();
    }
    public void ChangeBackGround()
    {
        string sceneText = SceneManager.GetActiveScene().name;
        if (sceneText.Substring(sceneText.Length - 5) == "FIRST" && background.Count != talkNum + 1)
        {
            if (TWC.talk_num == background[talkNum].EndTalkNum)
            {
                background[talkNum].BackGroundImage.SetActive(false);
                background[talkNum + 1].BackGroundImage.SetActive(true);
                talkNum++;
            }
        }
    }
}

[System.Serializable]
public class BackGroundImageData
{
    public GameObject BackGroundImage;
    public int EndTalkNum;
}