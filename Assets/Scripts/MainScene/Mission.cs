using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;

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



public class Mission : MonoBehaviour
{
    public List<MissionGroupDatas> missionGroupDatas = new List<MissionGroupDatas>();

    //update mission
    public UnityEvent onValidate = new UnityEvent();

    public UnityEvent toYellow = new UnityEvent();
    public UnityEvent toGray = new UnityEvent();

    //Missionクラスの情報が少しでも変更されたら走る
    void OnValidate()
    {
        Start(); //debug??
        CheckMission();

        //GameObject側を変更
        onValidate?.Invoke();
        Debug.Log("Mission is modified.");
    }

    void Start()
    {
        // SetMission(new Vector2Int(0, 1), new Vector2Int(0, 0));

        SetID();
    }

    //IDをセット・currentValueを共通させる
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

    //ミッションクラスを新規作成する
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

    //クリアしているミッションをAchievedにする
    public void CheckMission()
    {
        for (int i = 0; i < missionGroupDatas.Count(); i++)
        {
            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
            {
                missionGroupDatas[i].missionDatas[j].JudgeAchieveMissionState();
            }

            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
            {
                if (missionGroupDatas[i].missionDatas[j].missionState == MissionState.Achieved)
                {
                    toYellow?.Invoke();
                    // Debug.Log($"{i},{j}: ミッションクリア");
                    break;
                }
                else if (j == missionGroupDatas[i].missionDatas.Count() - 1) toGray?.Invoke();
                else toGray?.Invoke();
            }
        }
    }

    public void RefreshAllMissions(int[] missionGroupCount)
    {
        for (int i = 0; i < missionGroupCount.Count(); i++)
        {
            for (int j = 0; j < missionGroupCount[i]; j++)
            {
                // if()
                missionGroupDatas[i].missionDatas[j].missionState = MissionState.Received;
            }
        }
    }

    //CurrentMissionNumをmissionGroupDatas.Count()個を持った配列にして返す
    public int[] CurrentMissionNums()
    {
        int[] currentMissionNums = new int[missionGroupDatas.Count()];
        for (int i = 0; i < missionGroupDatas.Count(); i++) currentMissionNums[i] = CurrentMissionNum(i);
        return currentMissionNums;
    }

    //i番目の現在出現中のMissionオブジェクト（未クリア・未報酬受け取り）がi番目のMissionGroupの中で何番目にいるか
    //評価対象外・ミッション全てをクリアしている場合は-1を返す
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
    //IDの上二桁
    [HideInInspector] public string headId;

    //ミッションのタイプ・貫通or分別
    public MissionType missionType;

    //missionTypeが貫通のとき、全missionDataを通して共通の値
    public int throughCurrentValue;

    //表示するゲームオブジェクト
    public GameObject missionObject;

    //一個下のクラス
    public List<MissionDatas> missionDatas = new List<MissionDatas>();
}



[System.Serializable]
public class MissionDatas
{
    //ID
    [HideInInspector] public string bottomId;
    [SerializeField] public string id;

    //ミッションのタイトル
    public string missionMessage;

    //ミッションの状態
    public MissionState missionState;

    //ミッションのクリア状況
    public int currentValue;
    public int goalValue;

    //クリア報酬
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
        if (goalValue <= currentValue && missionState == MissionState.NotAchieved) missionState = MissionState.Achieved;
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