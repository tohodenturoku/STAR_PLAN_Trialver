using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bind : MonoBehaviour
{
    // バインド解除用
    [SerializeField]
    private Slider slider;
    // バインド解除用UI全体
    [SerializeField]
    private GameObject Bind_UI;
    private PlayerController PC;
    // 初期値
    private float Durability = 0.0f;
    // 最大値
    private const float MaxDurability = 20.0f;

    private bool BindingPlayer = false;
    void Start()
    {
        slider.value = 0.0f;
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Bind_UI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        BindPlayer();
    }

    void BindPlayer()
    {
        if (PC.Freeze && BindingPlayer && (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            Durability++;
            slider.value = Durability / MaxDurability;
        }
        if (Durability >= MaxDurability)
        {
            Bind_UI.SetActive(false);
            slider.value = 0.0f;
            PC.Freeze = false;
            Durability = 0;
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !PC.Freeze)
        {
            slider.value = 0.0f;
            Bind_UI.SetActive(true);
            PC.Freeze = true;
            BindingPlayer = true;
        }
    }
}
