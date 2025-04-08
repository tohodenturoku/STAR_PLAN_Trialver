using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BOSSHPController : MonoBehaviour
{
    public Slider HP_BOSS;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private float MaxHP = 270.0f;
    private float currentHP;
    // Start is called before the first frame update
    void Start()
    {
        HP_BOSS.value = 1.0f;
        currentHP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        JudgeHP();
    }

    void JudgeHP()
    {
        HP_BOSS.value = currentHP / MaxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
    }
}
