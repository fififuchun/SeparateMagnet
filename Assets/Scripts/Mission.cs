using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class Mission : MonoBehaviour
{
    public List<MissionGroupData> missionGroupDatas = new List<MissionGroupData>();

    void OnValidate()
    {
        Debug.Log("a");
    }

    void Start()
    {
        for (int i = 0; i < missionGroupDatas.Count(); i++)
        {
            missionGroupDatas[i].headId = Library.LastTwoDigits(i);
            for (int j = 0; j < missionGroupDatas[i].missionDatas.Count(); j++)
            {
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
            missionGroupDatas.Add(new MissionGroupData());
            for (int i = 0; i < place.y + 1; i++) missionGroupDatas[place.x].missionDatas.Add(new MissionData());
            Debug.Log("指定の位置にミッショングループデータを新規作成しました");
        }
        else if (missionGroupDatas[place.x].missionDatas.Count() <= place.y)
        {
            for (int i = 0; i < place.y - missionGroupDatas[place.x].missionDatas.Count() + 1; i++)
                missionGroupDatas[place.x].missionDatas.Add(new MissionData());
        }

        missionGroupDatas[place.x].missionDatas[place.y].currentValue = value.x;
        missionGroupDatas[place.x].missionDatas[place.y].goalValue = value.y;
    }
}



[System.Serializable]
public class MissionGroupDatas
{
    [HideInInspector] public string headId;

    public int throughCurrentValue;


    public MissionType missionType;
    public enum MissionType
    {
        Through,
        Separate,
    }


    public List<MissionData> missionDatas = new List<MissionData>();
}



[System.Serializable]
public class MissionDatas
{
    [HideInInspector] public string bottomId;
    [ReadOnly, SerializeField] private string id;

    public GameObject missionPrefab;

    // public MissionDataPrefab missionDataPrefab;

    public int currentValue;
    public int goalValue;

    public MissionState missionState;
    public enum MissionState
    {
        None,
        Acieved,
        NotAcieved,
        Received,
    }


    //便利な関数
    //初期化
    public void InitializeMissionState()
    {
        if (missionState == MissionState.None) missionState = MissionState.NotAcieved;
    }

    //ミッションを達成したとき
    public void JudgeAchieveMissionState()
    {
        if (goalValue < currentValue) missionState = MissionState.Acieved;
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
[CustomEditor(typeof(Mission))]
public class MissionEditor : Editor
{
    private Mission mission;
    private SerializedProperty[] _throughCurrentValueProperties;

    private void OnEnable()
    {
        mission = target as Mission;

        _throughCurrentValueProperties = new SerializedProperty[mission.missionGroupDatas.Count()];
        for (int i = 0; i < _throughCurrentValueProperties.Length;i++){
            // _throughCurrentValueProperties[i]= mission.missionGroupDatas[i].throughCurrentValue;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            switch (mission.missionGroupDatas[i].missionType)
            {
                case MissionGroupData.MissionType.Through:
                    // EditorGUILayout.IntField("currentValue", mission.missionGroupDatas[i].throughCurrentValue);
                    EditorGUILayout.HelpBox("表示するテキスト", MessageType.Info);
                    break;
            }
        }
    }
}
#endif