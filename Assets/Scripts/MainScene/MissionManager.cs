using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

//Missionの中でもゲーム特有のものをここに書きたい(詳細な値設定とかオブジェクトの管理とか)
[ExecuteAlways]
public class MissionManager : MonoBehaviour
{
    [SerializeField] private Mission mission;
    [SerializeField] private MissionDataManager missionDataManager;
    [SerializeField] private DiamondCount diamondCount;

    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private GameObject missionContent;

    void OnValidate()
    {
        SetMissionInformation();
    }

    void Start()
    {
        SetMissionInformation();
        InstantiateMissions();
        mission.RefreshAllMissions(missionDataManager.data.AchivedMissionCounts);
        UpdateMissions();
    }

    public void SetMissionInformation()
    {
        for (int j = 0; j < mission.missionGroupDatas[0].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[0].missionDatas[j].goalValue = j + 2;
            mission.missionGroupDatas[0].missionDatas[j].reward = (j + 2) * 10;
            mission.missionGroupDatas[0].missionDatas[j].missionMessage = $"ランク{j + 2}にする";
        }

        for (int j = 0; j < mission.missionGroupDatas[1].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[1].missionDatas[j].goalValue = (j + 1) * 5;
            mission.missionGroupDatas[1].missionDatas[j].reward = (j + 1) * 10;
            mission.missionGroupDatas[1].missionDatas[j].missionMessage = $"国民の怒りづらさ\nレベルを{(j + 1) * 5}にする";
        }

        for (int j = 0; j < mission.missionGroupDatas[2].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[2].missionDatas[j].missionMessage = $"検討を重ねた時に\n怒る確率を{mission.missionGroupDatas[2].missionDatas[j].goalValue}回下げる";
        }

        for (int j = 0; j < mission.missionGroupDatas[3].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[3].missionDatas[j].missionMessage = $"国民の怒りゲージ\n上限を{mission.missionGroupDatas[3].missionDatas[j].goalValue + 3}にする";
        }

        for (int j = 0; j < mission.missionGroupDatas[4].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[4].missionDatas[j].missionMessage = $"{mission.missionGroupDatas[4].missionDatas[j].goalValue}円稼げ";
        }
    }

    public void InstantiateMissions()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++) InstantiateMission(i);
    }

    public void UpdateMissions()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            mission.missionGroupDatas[i].throughCurrentValue = missionDataManager.data.missionValues[i];
            UpdateMission(i);
        }
        mission.CheckMission();
    }

    public void InstantiateMission(int i)
    {
        if (mission.missionGroupDatas[i].missionObject != null) return;

        GameObject missionObject = Instantiate(missionPrefab, missionContent.transform);
        mission.missionGroupDatas[i].missionObject = missionObject;

        UpdateMission(i);
    }

    public void UpdateMission(int i)
    {
        //変更しました
        mission.missionGroupDatas[i].missionObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].missionMessage;
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].currentValue.ToString();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].goalValue.ToString();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).gameObject.GetComponent<Slider>().value = (float)mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].currentValue / (float)mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].goalValue;
        mission.missionGroupDatas[i].missionObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].reward}";

        mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => PushRecieveRewardButton(i));
    }

    public void PushRecieveRewardButton(int i)
    {
        if (mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].missionState == MissionState.Achieved)
        {
            mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].ReceiveMissionState();
            diamondCount.GetDiamond(mission.missionGroupDatas[i].missionDatas[missionDataManager.data.AchivedMissionCounts[i]].reward);

            missionDataManager.data.AchivedMissionCounts[i]++;
            Debug.Log("報酬を受け取りました");
        }
        else
        {
            Debug.Log("クリアしてね");
        }

        UpdateMission(i);
    }
}