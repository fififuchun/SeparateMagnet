using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "MatrixText", menuName = "Create MatrixTextSO"), System.Serializable]
public class MatrixTextSO : ScriptableObject
{
    public MatrixTextData[] stringGroups;
}

//表示用のクラス
[System.Serializable]
public class MatrixTextData
{
    [TextArea(3, 10)] public string[] strings;
}

#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(MatrixTextSO), true)]
public class MatrixTextSOEditor : Editor
{
    public MatrixTextSO matrixTextSO;

    private void OnEnable()
    {
        matrixTextSO = target as MatrixTextSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
#endif
