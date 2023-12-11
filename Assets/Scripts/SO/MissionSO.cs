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
    public List<MissionData> missionDatas = new List<MissionData>();
}

[System.Serializable]
public class MissionData
{
    [HideInInspector] public string bottomId;
    public string id;

    public GameObject missionPrefab;

    public MissionDataPrefab missionDataPrefab;

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

    //初期化
    public void InitializeMissionState()
    {
        if (missionState == MissionState.None) missionState = MissionState.NotAcieved;
    }

    //ミッションを達成したとき
    public void AchieveMissionState()
    {
        if (goalValue < currentValue) missionState = MissionState.Acieved;
    }

    //ミッション報酬を受け取ったとき
    public void ReceiveMissionState()
    {
        missionState = MissionState.Received;
    }
}


public class IdLibrary
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
}