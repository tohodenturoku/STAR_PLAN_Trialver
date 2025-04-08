using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstEndController : MonoBehaviour
{
    [SerializeField]
    private TalkWindowController TWC;
    // Update is called once per frame
    void Update()
    {
        string sceneText = SceneManager.GetActiveScene().name;
        if (TWC.endjudge)
            SceneManager.LoadScene(sceneText.Substring(0, 2) + "ROAD");
    }
}
