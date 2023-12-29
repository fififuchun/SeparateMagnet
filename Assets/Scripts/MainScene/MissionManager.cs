using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Missionの中でもゲーム特有のものをここに書きたい(詳細な値設定とかオブジェクトの管理とか)
[ExecuteAlways]
public class MissionManager : MonoBehaviour
{
    [SerializeField] private Mission mission;

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
            mission.missionGroupDatas[1].missionDatas[j].missionMessage = $"国民の怒りづらさレベルを{(j + 1) * 5}にする";
        }

        for (int j = 0; j < mission.missionGroupDatas[2].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[2].missionDatas[j].missionMessage = $"検討を重ねた時に怒る確率を{mission.missionGroupDatas[2].missionDatas[j].goalValue}回下げる";
        }

        for (int j = 0; j < mission.missionGroupDatas[3].missionDatas.Count(); j++)
        {
            mission.missionGroupDatas[3].missionDatas[j].missionMessage = $"国民の怒りゲージ上限を{mission.missionGroupDatas[3].missionDatas[j].goalValue + 3}にする";
        }
    }

    public void UpdateMission()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            // mission.missionGroupDatas[i].missionObject.
        }
    }

    public void InstantiateMissions()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            InstantiateMission(i);
        }
    }

    // public void InstantiateMission(string missionMessage, int current, int goal, int reward)
    // {
    //     GameObject missionObject = Instantiate(missionPrefab, missionContent.transform);
    //     missionObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = missionMessage;
    //     missionObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = current.ToString();
    //     missionObject.transform.GetChild(1).GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = goal.ToString();
    //     missionObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{reward}";
    //     missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(PushRecieveRewardButton);
    // }

    public void InstantiateMission(int i)
    {
        if (mission.missionGroupDatas[i].missionObject != null) return;

        GameObject missionObject = Instantiate(missionPrefab, missionContent.transform);
        mission.missionGroupDatas[i].missionObject = missionObject;

        missionObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].missionMessage;
        missionObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].currentValue.ToString();
        missionObject.transform.GetChild(1).GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].goalValue.ToString();
        missionObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].reward}";
        missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(PushRecieveRewardButton);
    }

    public void PushRecieveRewardButton()
    {
        // if()
    }
}