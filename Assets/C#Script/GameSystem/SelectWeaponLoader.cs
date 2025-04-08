using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectWeaponLoader : MonoBehaviour
{
    [HideInInspector]
    public SaveSelectWeapon data;
    private string filepath;
    private string filename = "SavaSelectWeapon.json";
    private SaveSelectWeapon initdata;

    void Awake()
    {
        filepath = Application.dataPath + "/" + filename;

        if (!File.Exists(filepath))
            Save(filepath);

        data = Load(filepath);
        initdata = JsonUtility.FromJson<SaveSelectWeapon>(JsonUtility.ToJson(data));
    }

    void Update()
    {
        ChangeJudge();
    }

    void ChangeJudge()
    {
        if (data.selectChar != initdata.selectChar || data.selectSpell != initdata.selectSpell)
            Save(filepath);
    }

    void Save(string path)
    {
        string json = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(path, false);
        wr.WriteLine(json);
        wr.Close();
    }

    SaveSelectWeapon Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<SaveSelectWeapon>(json);
    }
}
