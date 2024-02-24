using System.Collections;
using System.Collections.Generic;
using System.Linq;
// using Unity.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Missionの中でもゲーム特有のものをここに書きたい(詳細な値設定とかオブジェクトの管理とか)
[ExecuteAlways]
public class MissionManager : MonoBehaviour
{
    [SerializeField] private Mission mission;
    [SerializeField] private MissionDataManager missionDataManager;
    [SerializeField] private DiamondCount diamondCount;

    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private GameObject missionContent;

    [SerializeField] private GameObject notificationImage;

    //Missionクラスをいじるだけ
    void OnValidate()
    {
        SetMissionInformation();
    }

    //Destroy・Instantiateはここだけ
    void Start()
    {
        SetMissionInformation();
        mission.RefreshAllMissions(missionDataManager.data.receivedMissionCounts);
        mission.onValidate.AddListener(UpdateMissions);
        mission.toYellow.AddListener(ToYellow);
        mission.toGray.AddListener(ToGray);

        InstantiateMissions();
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
        mission.SetID();
        notificationImage.SetActive(ExistAchievedMission());
        Debug.Log("ミッションを更新しました");
    }

    //MissionクラスのMissionGroupのi番目のGameObjectがいなかったら生成
    public void InstantiateMission(int i)
    {
        if (mission.missionGroupDatas[i].missionObject != null) return;

        GameObject missionObject = Instantiate(missionPrefab, missionContent.transform);
        mission.missionGroupDatas[i].missionObject = missionObject;

        UpdateMission(i);
    }

    //MissionクラスのMissionGroupのi番目のGameObjectを動的に変更
    public void UpdateMission(int i)
    {
        if (mission.CurrentMissionNum(i) < 0)
        {
            mission.missionGroupDatas[i].missionObject.SetActive(false);
            // Debug.Log($"{i}: このミッションは完了しています");
            return;
        }
        // Debug.Log($"{i}: このミッションは未完了");
        //変更しました
        mission.missionGroupDatas[i].missionObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].missionMessage;
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].currentValue.ToString();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).GetChild(4).gameObject.GetComponent<TextMeshProUGUI>().text = mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].goalValue.ToString();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(1).gameObject.GetComponent<Slider>().value = (float)mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].currentValue / (float)mission.missionGroupDatas[i].missionDatas[mission.CurrentMissionNum(i)].goalValue;
        mission.missionGroupDatas[i].missionObject.transform.GetChild(2).GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"x{mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].reward}";

        mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Button>().onClick.AddListener(() => PushRecieveRewardButton(i));
    }

    //MissionクラスのMissionGroupのi番目のMissionがクリアしていたら報酬を受け取れるようにする
    public void PushRecieveRewardButton(int i)
    {
        if (mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].missionState == MissionState.Achieved)
        {
            mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].ReceiveMissionState();
            diamondCount.GetDiamond(mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].reward);

            missionDataManager.data.receivedMissionCounts[i]++;
            Debug.Log("報酬を受け取りました");

            if (missionDataManager.data.receivedMissionCounts[i] == mission.missionGroupDatas[i].missionDatas.Count()) mission.missionGroupDatas[i].missionObject.SetActive(false);
        }
        else
        {
            Debug.Log("クリアしてね");
        }

        // UpdateMission(i);
        UpdateMissions();
    }

    //クリアしているMissionがあるかどうか
    public bool ExistAchievedMission()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            if (missionDataManager.data.receivedMissionCounts[i] == mission.missionGroupDatas[i].missionDatas.Count())
            {
                continue;
            }

            if (mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].missionState == MissionState.Achieved)
            {
                // Debug.Log(i + " " + missionDataManager.data.AchivedMissionCounts[i] + ":クリアしたミッションがあるよ");
                return true;
            }
        }
        return false;
    }

    //
    public void ToYellow()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            if (missionDataManager.data.receivedMissionCounts[i] == mission.missionGroupDatas[i].missionDatas.Count()) return;

            if (mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].missionState == MissionState.Achieved)
            {
                mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color32(255, 78, 35, 255);
            }
        }
    }

    public void ToGray()
    {
        for (int i = 0; i < mission.missionGroupDatas.Count(); i++)
        {
            if (missionDataManager.data.receivedMissionCounts[i] == mission.missionGroupDatas[i].missionDatas.Count()) return;

            if (mission.missionGroupDatas[i].missionDatas[missionDataManager.data.receivedMissionCounts[i]].missionState != MissionState.Achieved)
            {
                mission.missionGroupDatas[i].missionObject.transform.GetChild(3).gameObject.GetComponent<Image>().color = new Color32(200, 200, 200, 255);
            }
        }
    }
}