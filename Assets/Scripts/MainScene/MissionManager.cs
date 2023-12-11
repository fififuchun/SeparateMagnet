using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[ExecuteAlways]
public class MissionManager : MonoBehaviour
{
    [SerializeField, ReadOnly] private MissionSO missionSO;

    void Start()
    {
        for (int i = 0; i < missionSO.missionGroupDatas.Count(); i++)
        {
            missionSO.missionGroupDatas[i].headId = Library.LastTwoDigits(1 + i);
            for (int j = 0; j < missionSO.missionGroupDatas[i].missionDatas.Count(); j++)
            {
                missionSO.missionGroupDatas[i].missionDatas[j].bottomId = Library.LastTwoDigits(1 + j);
                missionSO.missionGroupDatas[i].missionDatas[j].id = missionSO.missionGroupDatas[i].headId + missionSO.missionGroupDatas[i].missionDatas[j].bottomId;

                missionSO.missionGroupDatas[i].missionDatas[j].InitializeMissionState();

                missionSO.missionGroupDatas[i].missionDatas[j].missionDataPrefab = new MissionDataPrefab();
            }
        }
    }
}

public class MissionDataPrefab : MonoBehaviour
{

}
