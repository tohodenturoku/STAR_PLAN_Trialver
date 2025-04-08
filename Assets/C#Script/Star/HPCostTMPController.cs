using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPCostTMPController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI HPTMP;
    [SerializeField]
    private TextMeshProUGUI CostTMP;
    private float initHP;
    private float initCost;
    private float currentHP;
    private float currentCost;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        initHP = player.GetComponent<PlayerController>().HealthPoint;
        initCost = player.GetComponent<PlayerController>().CostPoint;
        currentHP = initHP;
        currentCost = initCost;
        HPTMP.text = currentHP.ToString() + "/" + initHP.ToString();
        CostTMP.text = currentCost.ToString() + "/" + initCost.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().HealthPoint < 0)
            currentHP = 0;
        else
            currentHP = player.GetComponent<PlayerController>().HealthPoint;
        currentCost = player.GetComponent<PlayerController>().CostPoint;
        HPTMP.text = currentHP.ToString() + "/" + initHP.ToString();
        CostTMP.text = currentCost.ToString() + "/" + initCost.ToString();
    }
}
