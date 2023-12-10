using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private MissionSO missionSO;

    void Start()
    {
        for (int i = 0; i < missionSO.missionGroupDatas.Count(); i++)
        {
            missionSO.missionGroupDatas[i].headId= Library.LastTwoDigits(i);
        }
    }


}
