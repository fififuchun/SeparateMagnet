// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [CreateAssetMenu(fileName = "KentoDataSO", menuName = "Create KentoDataSo")]
// public class KentoDataSO : ScriptableObject //一番でかいclass
// {
//     public Font font;
//     public enum Font
//     {
//         Gothic,
//         Gyosho,
//         Mincho,
//         Pop,
//         Rare,
//     }
//     public List<FontData> fontData = new List<FontData>();

//     public List<FontData> GetFontData(FontData.Font font)
//     {
//         return fontData.FindAll(data => data.font == font);
//     }

// }

// [System.Serializable]
// public class FontDataSO //中身の配列、fontの種類のenumとkentoDataを持ってる
// {
//     public List<KentoData> sizeData = new List<KentoData>();
// }

// [System.Serializable]
// public class sizeDataSO //一番小さいclass、行列でいうと成分
// {
//     public int score;
//     public GameObject KentoPrefab;

//     public int FindObject(KentoManager kento)
//     {
//         if (kento.gameObject.name.Split("(")[0] == KentoPrefab.name) return score;
//         else return 0;
//     }
// }