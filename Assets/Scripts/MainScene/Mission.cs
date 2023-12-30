using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Events;

public enum MissionType
{
    Through,
    Separate,
}

public enum MissionState
{
    None,
    Achieved,
    NotAchieved,
    Received,
}



// [ExecuteAlways]
public class Mission : MonoBehaviour
{
    public List<MissionGroupDatas> missionGroupDatas = new List<MissionGroupDatas>();

    void OnValidate()
    {
        Start(); //debug??
        CheckMission();
    }

    void Start()
    {
        // SetMission(new Vector2Int(0, 1), new Vector2Int(0, 0));

        SetID();

        Debug.Log(String.Join(",", CurrentMissionNums()));
    }

    public void SetID()
    {
        for (int i = 0; i < missionGroupDatas.Count(); i++)
        {
            missionGroupDatas[i].headId = Library.LastTwoDigits(i);

            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
            {
                if (missionGroupDatas[i].missionType == MissionType.Through) missionGroupDatas[i].missionDatas[j].currentValue = missionGroupDatas[i].throughCurrentValue;
                missionGroupDatas[i].missionDatas[j].bottomId = Library.LastTwoDigits(j);
                missionGroupDatas[i].missionDatas[j].id = missionGroupDatas[i].headId + missionGroupDatas[i].missionDatas[j].bottomId;

                missionGroupDatas[i].missionDatas[j].InitializeMissionState();
            }
        }
    }

    public void SetMission(Vector2Int place, Vector2Int value)
    {
        if (missionGroupDatas.Count() <= place.x)
        {
            missionGroupDatas.Add(new MissionGroupDatas());
            for (int i = 0; i < place.y + 1; i++) missionGroupDatas[place.x].missionDatas.Add(new MissionDatas());
            Debug.Log("指定の位置にミッショングループデータを新規作成しました");
        }
        else if (missionGroupDatas[place.x].missionDatas.Count() <= place.y)
        {
            for (int i = 0; i < place.y - missionGroupDatas[place.x].missionDatas.Count() + 1; i++)
                missionGroupDatas[place.x].missionDatas.Add(new MissionDatas());
        }

        missionGroupDatas[place.x].missionDatas[place.y].currentValue = value.x;
        missionGroupDatas[place.x].missionDatas[place.y].goalValue = value.y;
        Debug.Log("初期化しました");
    }

    public void CheckMission()
    {
        for (int i = 0; i < missionGroupDatas.Count(); i++)
        {
            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
            {
                missionGroupDatas[i].missionDatas[j].JudgeAchieveMissionState();
            }
        }
    }

    public void RefreshAllMissions(int[] missionGroupCount)
    {
        for (int i = 0; i < missionGroupCount.Count(); i++)
        {
            for (int j = 0; j < missionGroupCount[i]; j++)
            {
                missionGroupDatas[i].missionDatas[j].missionState = MissionState.Received;
            }
        }
    }

    public int[] CurrentMissionNums()
    {
        int[] currentMissionNums = new int[missionGroupDatas.Count()];
        for (int i = 0; i < missionGroupDatas.Count(); i++) currentMissionNums[i] = CurrentMissionNum(i);
        return currentMissionNums;
    }

    public int CurrentMissionNum(int i)
    {
        if (i < 0 || i > missionGroupDatas.Count() - 1) return -1;
        if (missionGroupDatas[i].missionDatas.Count() == 0) return -1;

        for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
        {
            if (missionGroupDatas[i].missionDatas[j].missionState == MissionState.Achieved || missionGroupDatas[i].missionDatas[j].missionState == MissionState.NotAchieved)
            {
                return j;
            }
        }
        return -1;
    }
}



[System.Serializable]
public class MissionGroupDatas
{
    [HideInInspector] public string headId;


    public MissionType missionType;

    public int throughCurrentValue;

    public GameObject missionObject;

    // [HideInInspector] public bool isEvent;


    public List<MissionDatas> missionDatas = new List<MissionDatas>();
}



[System.Serializable]
public class MissionDatas
{
    [HideInInspector] public string bottomId;
    [SerializeField] public string id;

    public string missionMessage;

    public MissionState missionState;

    public int currentValue;
    public int goalValue;

    public int reward;


    //便利な関数
    //初期化
    public void InitializeMissionState()
    {
        if (missionState == MissionState.None) missionState = MissionState.NotAchieved;
    }

    //ミッションを達成したとき
    public void JudgeAchieveMissionState()
    {
        if (goalValue <= currentValue) missionState = MissionState.Achieved;
        // else missionState = MissionState.NotAchieved;
    }

    //ミッション報酬を受け取ったとき
    public void ReceiveMissionState()
    {
        missionState = MissionState.Received;
    }

    //IDからクラスの位置を特定
    public Vector2Int MissionDataIndex(MissionData missionData)
    {
        if (missionData.id.ToCharArray().Length == 4) return new Vector2Int(Mathf.FloorToInt(int.Parse(missionData.id) / 100), int.Parse(missionData.id) % 100);
        else return new Vector2Int(0, 0);
    }
}


#if UNITY_EDITOR
[CanEditMultipleObjects]
[CustomEditor(typeof(Mission), true)]
public class MissionEditor : Editor
{
    public Mission mission;

    private SerializedProperty _missionGroupDatas;


    // private SerializedProperty[] _throughCurrentValueProperties;

    // private SerializedProperty _id;
    // private SerializedProperty _currentValue;
    // private SerializedProperty _goalValue;
    // private SerializedProperty _missionState;

    private void OnEnable()
    {
        mission = target as Mission;

        _missionGroupDatas = serializedObject.FindProperty("missionGroupDatas");

        // _missionGroupDatas= serializedObject.FindProperty("missionGroupDatas");

        // _throughCurrentValueProperties = new SerializedProperty[mission.missionGroupDatas.Count()];
        // for (int i = 0; i < _throughCurrentValueProperties.Length; i++)
        // {
        //     // _throughCurrentValueProperties[i]= mission.missionGroupDatas[i].throughCurrentValue;
        // }

        // _id = serializedObject.FindProperty("id");
        // _currentValue = serializedObject.FindProperty("currentValue");
        // _goalValue = serializedObject.FindProperty("goalValue");
        // _missionState = serializedObject.FindProperty("goalValue");
    }

    public override void OnInspectorGUI()
    {
        // EditorGUILayout.PropertyField(_missionGroupDatas);

        base.OnInspectorGUI();


        // serializedObject.Update();

        // for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        // {
        //     switch (mission.missionGroupDatas[i].missionType)
        //     {
        //         case MissionType.Through:
        //             // EditorGUILayout.IntField("currentValue", mission.missionGroupDatas[i].throughCurrentValue);
        //             EditorGUILayout.HelpBox("表示するテキスト", MessageType.Info);
        //             break;
        //     }
        // }
        // EditorGUILayout.HelpBox("表示するテキスト", MessageType.Info);

        // serializedObject.ApplyModifiedProperties();
    }
}
#endif