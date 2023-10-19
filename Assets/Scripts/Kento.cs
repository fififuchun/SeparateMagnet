using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kento", menuName = "Create Kento")]
public class Kento : ScriptableObject
{
    public FontData[] kento = new FontData[5];
}

[System.Serializable]
public class FontData
{
    public Font font;
    public enum Font
    {
        Gothic,
        Gyosho,
        Mincho,
        Pop,
        Rare,
    }

    public List<KentoData> kentoFont = new List<KentoData>();
}

[System.Serializable]
public class KentoData
{
    public int score;
    public GameObject KentoPrefab;
}
