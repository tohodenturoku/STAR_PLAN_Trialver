using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckManaValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ManaTxt;
    [SerializeField] private TextMeshProUGUI ManaCost;
    [SerializeField]
    private SelectWeaponLoader selectWeaponLoader;
    [SerializeField] private PlayerController PC;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManaTxt.text = PC.CostPoint.ToString();
        ManaCost.text = "ManaCost = " + selectWeaponLoader.data.charCost.ToString();

    }
}
