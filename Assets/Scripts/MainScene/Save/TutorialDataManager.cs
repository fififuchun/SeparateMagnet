using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TutorialDataManager : MonoBehaviour
{
    //json変換したTutorialSaveData
    [HideInInspector] public TutorialSaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "TutorialData.json";

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
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(TutorialSaveData data)
    {
        string json = JsonUtility.ToJson(data);
        StreamWriter wr = new StreamWriter(filepath, false);
        wr.WriteLine(json);
        wr.Close();
    }

    // jsonファイル読み込み
    TutorialSaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<TutorialSaveData>(json);
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }
}
