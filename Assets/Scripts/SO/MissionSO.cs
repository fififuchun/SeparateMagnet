using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Create MissionSO")]
public class MissionSO : ScriptableObject
{
    public List<MissionGroupData> missionGroupDatas = new List<MissionGroupData>();

}

[System.Serializable]
public class MissionGroupData
{
    // public static MissionSO missionSO;
    public string headId;//= missionSO.missionGroupDatas.IndexOf(this);
    public List<MissionData> missionDatas = new List<MissionData>();
}

[System.Serializable]
public class MissionData
{
    public string bottomId;
    // public string id= MissionGroupData.HeadId(this);

    public MissionState missionState;
    public enum MissionState
    {
        Acieved,
        NotAcieved,
        received,
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
}