using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum KentoFont
{
    Chark = 1,
    Dot = 2,
    Gothic = 3,
    Gothic_Bold = 4,
    Gothic_Modify = 5,
    Gothic_Rounded = 6,
    Gyosho = 7,
    Handwritten = 8,
    Kaisho = 9,
    Maka = 10,
    Mincho = 11,
    Pop = 12,
    Scary = 13,
    Shake = 14,
    Sloopy = 15,
    Strong = 16,
    Textbook = 17,
    Textbook_Rounded = 18,
    Thin = 19,
    Unknown = 20,
    Weak = 21,
    Rare = 22,
}

[CreateAssetMenu(fileName = "Kento", menuName = "Create Kento")]
public class Kento : ScriptableObject //一番でかいclass
{
    public List<FontData> sizeData = new List<FontData>();
}

[System.Serializable]
public class FontData //中身の配列、fontの種類のenumとkentoDataを持ってる
{
    public int score;

    public GameObject[] kentoPrefabs = new GameObject[21];
}

#if UNITY_EDITOR
// [CanEditMultipleObjects]
[CustomEditor(typeof(Kento), true)]
public class KentoEditor : Editor
{
    public Kento kento;

    private void OnEnable()
    {
        kento = target as Kento;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif
