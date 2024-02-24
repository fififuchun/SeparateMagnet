using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MatrixText", menuName = "Create MatrixTextSO"), System.Serializable]
public class MatrixTextSO : ScriptableObject
{
    public MatrixTextData[] stringGroups; //= new List<MatrixTextData>();
}

//表示用のクラス
[System.Serializable]
public class MatrixTextData
{
    [TextArea] public string[] strings;
}
