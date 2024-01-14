using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Create MissionSO")]
public class MissionSO : ScriptableObject
{
    public List<MissionGroupData> missionGroupDatas = new List<MissionGroupData>();
}

[System.Serializable]
public class MissionGroupData
{
    [HideInInspector] public string headId;


    public MissionType missionType;
    public enum MissionType
    {
        Through,
        Separate,
    }


    public List<MissionData> missionDatas = new List<MissionData>();
}

[System.Serializable]
public class MissionData
{
    [HideInInspector] public string bottomId;
    public string id;

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



    // void OnValidate()
    // {
    //     Debug.Log("a");
    // }


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


public class IdLibrary : MonoBehaviour
{
    public string LastTwoDigits(int a)
    {
        int reminder = a % 100;

        if (a > 9) return reminder.ToString();
        else if (a > 0) return "0" + reminder.ToString();
        else return "";
    }

    public void ChangeId(string previousId, string nextId)
    {
        previousId = nextId;
    }

    void OnValidate()
    {
        Debug.Log("a");
    }
}