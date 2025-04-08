using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S3BOSSEndController : MonoBehaviour
{
    [SerializeField]
    private UtuhoReiuziBossController URBC;
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
    private GameObject HP_BOSS;
    [SerializeField]
    private GameObject Cost_Slider;
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
    }
    void Update()
    {
        if (TWC.talk_num == 19 && BHC.HP_BOSS.value > 0 && player.GetComponent<PlayerController>().HealthPoint > 0)
        {
            Balloon.SetActive(false);
            Chars.SetActive(false);
            player.SetActive(true);
            enemy.SetActive(true);
            HP_Slider.SetActive(true);
            Cost_Slider.SetActive(true);
            HP_BOSS.SetActive(true);
        }
        if (URBC.endbossflag)
        {
            Balloon.SetActive(true);
            Chars.SetActive(true);
            player.SetActive(false);
            enemy.SetActive(false);
            HP_Slider.SetActive(false);
            Cost_Slider.SetActive(false);
            HP_BOSS.SetActive(false);
        }
    }
}
