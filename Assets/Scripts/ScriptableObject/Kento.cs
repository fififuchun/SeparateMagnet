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
    // Gothic = 3,
    Gothic_Bold = 3,
    Gothic_Modify = 4,
    Gothic_Rounded = 5,
    Gyosho = 6,
    Handwritten = 7,
    Kaisho = 8,
    Maka = 9,
    Mincho = 10,
    Pop = 11,
    Scary = 12,
    Shake = 13,
    Sloopy = 14,
    Strong = 15,
    Textbook = 16,
    Textbook_Rounded = 17,
    Thin = 18,
    Unknown = 19,
    Weak = 20,
    Rare = 21,
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
