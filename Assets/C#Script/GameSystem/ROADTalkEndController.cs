using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ROADTalkEndController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject Vines;
    [SerializeField]
    private GameObject HPCostTMPController;
    [SerializeField]
    private GameObject MapNavi;
    [SerializeField]
    private GameObject MapNaviController;
    [SerializeField]
    private GameObject HPSlider;
    [SerializeField]
    private GameObject talkChars;
    [SerializeField]
    private GameObject Balloon;
    [SerializeField]
    private GameObject enemyPosNavi;
    [SerializeField]
    private TalkWindowController TWC;
    // Update is called once per frame
    void Update()
    {
        if (TWC.endjudge && player.GetComponent<PlayerController>().HealthPoint > 0)
        {
            talkChars.SetActive(false);
            Balloon.SetActive(false);
            player.SetActive(true);
            Vines.SetActive(true);
            enemy.SetActive(true);
            MapNavi.SetActive(true);
            MapNaviController.SetActive(true);
            HPSlider.SetActive(true);
            enemyPosNavi.SetActive(true);
            HPCostTMPController.SetActive(true);
        }
        else
        {
            MapNavi.SetActive(false);
            enemyPosNavi.SetActive(false);
        }
    }
}
