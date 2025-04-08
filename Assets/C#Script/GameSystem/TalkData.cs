using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharactorType
{
    None = 0,
    Star,
    Luna,
    Sunny,
    Marisa,
    Cirno,
    Okuu,
    Reimu,
    TreeFairys
};

[System.Serializable]
public class TalkData
{
    public CharactorType Name;
    [Multiline(3)]
    public string Talk = "";
}