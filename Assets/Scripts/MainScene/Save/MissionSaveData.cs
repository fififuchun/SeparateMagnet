using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionSaveData
{
    public const int missionGroupCount = 6;
    public int[] ReceivedMissionCounts = new int[missionGroupCount];
    public int[] missionValues = new int[missionGroupCount];
}
