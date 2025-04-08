using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S1BOSSEndController : MonoBehaviour
{
    [SerializeField]
    private MarisaKirisameBossController MKBC;
    [SerializeField]
    private BOSSHPController BHC;
    [SerializeField]
    private TalkWindowController TWC;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject HP_Slider;
    [SerializeField]
    private GameObject Cost_Slider;
    [SerializeField]
    private GameObject HPCostTMPController;
    [SerializeField]
    private GameObject HP_BOSS;
    [SerializeField]
    private GameObject Chars;
    [SerializeField]
    private GameObject Balloon;
    // Update is called once per frame
    void Start()
    {
        Cost_Slider.SetActive(false);
        player.SetActive(false);
        enemy.SetActive(false);
        HP_Slider.SetActive(false);
        HP_BOSS.SetActive(false);
        HPCostTMPController.SetActive(false);
    }
    void Update()
    {
        if (TWC.talk_num == 10 && BHC.HP_BOSS.value > 0 && player.GetComponent<PlayerController>().HealthPoint > 0)
        {
            Balloon.SetActive(false);
            Chars.SetActive(false);
            player.SetActive(true);
            enemy.SetActive(true);
            HP_Slider.SetActive(true);
            Cost_Slider.SetActive(true);
            HP_BOSS.SetActive(true);
            HPCostTMPController.SetActive(true);
        }
        if (MKBC.endbossflag)
        {
            Balloon.SetActive(true);
            Chars.SetActive(true);
            HPCostTMPController.SetActive(false);
            player.SetActive(false);
            enemy.SetActive(false);
            HP_Slider.SetActive(false);
            Cost_Slider.SetActive(false);
            HP_BOSS.SetActive(false);
        }
        if (TWC.endjudge)
            SceneManager.LoadScene("S2ROAD");
    }
}
