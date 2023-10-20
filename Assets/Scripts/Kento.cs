using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kento", menuName = "Create Kento")]
public class Kento : ScriptableObject //一番でかい配列
{
    public FontData[] kentoData = new FontData[5];
}

[System.Serializable]
public class FontData //中身の配列、fontの種類のenumとkentoDataを持ってる
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
public class KentoData //一番小さいclass、行列でいうと成分
{
    public int score;
    public GameObject KentoPrefab;
}
