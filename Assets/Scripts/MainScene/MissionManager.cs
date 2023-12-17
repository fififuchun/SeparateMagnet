using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

[ExecuteAlways]
public class MissionManager : MonoBehaviour
{
    //ミッション用
    [SerializeField, ReadOnly] private MissionSO missionSO;

    //各クラスのインスタンス化、必要な値があれば必要なクラスを適宜追加
    [SerializeField] private DataManager dataManager;
    [SerializeField] private CoinCount coinCount;
    [SerializeField] private DiamondCount diamondCount;
    [SerializeField] private RankManager rankManager;

    void Start()
    {
        for (int i = 0; i < missionSO.missionGroupDatas.Count(); i++)
        {
            missionSO.missionGroupDatas[i].headId = Library.LastTwoDigits(i);
            for (int j = 0; j < missionSO.missionGroupDatas[i].missionDatas.Count(); j++)
            {
                missionSO.missionGroupDatas[i].missionDatas[j].bottomId = Library.LastTwoDigits(j);
                missionSO.missionGroupDatas[i].missionDatas[j].id = missionSO.missionGroupDatas[i].headId + missionSO.missionGroupDatas[i].missionDatas[j].bottomId;

                missionSO.missionGroupDatas[i].missionDatas[j].InitializeMissionState();

                // missionSO.missionGroupDatas[i].missionDatas[j].missionDataPrefab = new MissionDataPrefab();
            }
        }

        // missionSO.missionGroupDatas[0].missionDatas.Add(new MissionData());
        // SetSO(new Vector2Int(0, 3), new Vector2Int(0, 500));
        // Debug.Log("");
    }

    void OnValidate()
    {
        Debug.Log("a");
    }

    public void SetSO(Vector2Int place, Vector2Int value)
    {
        if (missionSO.missionGroupDatas.Count() <= place.x)
        {
            missionSO.missionGroupDatas.Add(new MissionGroupData());
            for (int i = 0; i < place.y + 1; i++) missionSO.missionGroupDatas[place.x].missionDatas.Add(new MissionData());
            Debug.Log("指定の位置にミッショングループデータを新規作成しました");
        }
        else if (missionSO.missionGroupDatas[place.x].missionDatas.Count() <= place.y)
        {
            for (int i = 0; i < place.y - missionSO.missionGroupDatas[place.x].missionDatas.Count() + 1; i++)
                missionSO.missionGroupDatas[place.x].missionDatas.Add(new MissionData());
        }

        missionSO.missionGroupDatas[place.x].missionDatas[place.y].currentValue = value.x;
        missionSO.missionGroupDatas[place.x].missionDatas[place.y].goalValue = value.y;
    }
}

// public class MissionDataPrefab : MonoBehaviour
// {

// }
