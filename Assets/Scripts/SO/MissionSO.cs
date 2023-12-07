using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Create MissionSO")]
public class MissionSO : ScriptableObject
{
    public List<MissionData> missionData = new List<MissionData>();
}

[System.Serializable]
public class MissionData
{
    public MissionState missionState;
    public enum MissionState
    {

    }
}