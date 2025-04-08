using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSelectWeapon
{
    public int selectChar;
    public int charCost;
    public int selectSpell;
    public int spellCost;
    public int accessRight;
    public SaveSelectWeapon(int _selectChar, int _charCost, int _selectSpell, int _spellCost, int _accessRight)
    {
        selectChar = _selectChar;
        selectSpell = _selectSpell;
        charCost = _charCost;
        spellCost = _spellCost;
        accessRight = _accessRight;
    }
}
