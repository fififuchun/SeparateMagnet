using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    // json変換するデータのクラス
    [HideInInspector] public SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Data.json";

    //
    // public static int earnCoin;

    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
#if UNITY_EDITOR
        filepath = Application.dataPath + "/" + fileName;

#elif UNITY_ANDROID
        filepath = Application.persistentDataPath + "/" + fileName;

#else
        filepath = Application.dataPath + "/" + fileName;

#endif

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) Save(data);

        // ファイルを読み込んでdataに格納
        data = Load(filepath);
        CheakSaveData();

        if (SaveData.tax >= 0)
        {
            data.coin += SaveData.tax;
            data.sumcoin += SaveData.tax;
            SaveData.tax = 0;
        }
    }


    // jsonとしてデータを保存
    public void Save(SaveData data)
    {
        // jsonとして変換
        string json = JsonUtility.ToJson(data, true);

        // ファイル書き込み指定
        StreamWriter wr = new StreamWriter(filepath, false);

        // json変換した情報を書き込み
        wr.WriteLine(json);

        // ファイル閉じる
        wr.Close();
    }

    public void Save() { Save(data); }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);

        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();

        // ファイル閉じる
        rd.Close();

        // jsonファイルを型に戻して返す
        return JsonUtility.FromJson<SaveData>(json);
    }

    public void ResetDataManager()
    {
        //General
        // data.rank = 0;
        data.diamond = 0;
        data.coin = 0;
        data.sumcoin = 0;
        SaveData.tax = 0;

        //RPG
        data.level = new int[SaveData.LEVEL_COUNT];
        data.fontNumbers = new int[6] { 0, 0, 0, -1, -1, -1 };
        for (int i = 0; i < data.isRareFonts.Length; i++) data.isRareFonts[i] = false;
        for (int i = 0; i < data.haveFonts.Length; i++) data.haveFonts[i] = false;

        //Mission
        data.receivedMissionCounts = new int[SaveData.MISSIONGROUP_COUNT];
        data.missionValues = new int[SaveData.MISSIONGROUP_COUNT];

        //Tutorial
        data.isFinishedTutorial = new bool[SaveData.TUTORIAL_COUNT];
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }

    //初期数値
    private int[] initLevel = { 0, 0, 0, 0, 0, 0, 0 };
    public int[] InitLevel { get => initLevel; }

    private int[] maxlevel = { 3, 40, 10, 5, 98, 5, 50 };
    public int[] MaxLevel { get => maxlevel; }

    void CheakSaveData()
    {
        for (int i = 0; i < data.level.Count(); i++)
        {
            if (data.level[i] < InitLevel[i]) data.level[i] = InitLevel[i];
            if (data.level[i] > MaxLevel[i]) data.level[i] = MaxLevel[i];
        }
    }

    /// <summary>
    /// 何個の枠が解放されているか、1~6のうちどれか
    /// </summary>
    /// <returns></returns>
    public int ReleasedFontCount()
    {
        int i = 0;
        while (data.fontNumbers[i] >= 0)
        {
            i++;
            if (i == 6) return i;
        }
        return i;
    }

    //update mission
    public UnityEvent updateMissions = new UnityEvent();
    public void ChangeMissionValue(int i, float changedValue)
    {
        data.missionValues[i] = (int)changedValue;
        Debug.Log($"missionDataの{i}番目を{changedValue}に変更しました");

        Save(data);

        updateMissions?.Invoke();
    }
}
