using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.IO;
// using System.Text;

public class MissionDataManager : MonoBehaviour
{
    //json変換したMissionSaveData
    [HideInInspector] public MissionSaveData data;

    //update mission
    public UnityEvent isChanged = new UnityEvent();

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "MissionData.json";

    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) Save(data);

        // ファイルを読み込んでdataに格納
        data = Load(filepath);
        Debug.Log(File.Exists(filepath));
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(MissionSaveData data)
    {
        Debug.Log("save mission data");
        string json = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }

    // jsonファイル読み込み
    MissionSaveData Load(string path)
    {
        Debug.Log("load mission data");
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<MissionSaveData>(json);
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }

    public void ChangeMissionValue(int i, float changedValue)
    {
        data.missionValues[i] = (int)changedValue;
        Debug.Log($"missionDataの{i}番目を{changedValue}に変更しました");

        Save(data);
        isChanged.Invoke();
    }

    public void ResetMissionDataManager()
    {
        data.receivedMissionCounts = new int[MissionSaveData.missionGroupCount];
        data.missionValues = new int[MissionSaveData.missionGroupCount];
    }
}
