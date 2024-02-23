using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatrixText", menuName = "Create MatrixTextSO")]
public class TutorialSO : ScriptableObject
{
    public List<TutorialData> stringGroups= new List<TutorialData>();
}

//表示用のクラス
[System.Serializable]
public class TutorialData
{
    [TextArea] public string[] strings;
}
