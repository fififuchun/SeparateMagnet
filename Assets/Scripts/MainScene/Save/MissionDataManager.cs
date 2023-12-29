using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MissionDataManager : MonoBehaviour
{
    //
    public Mission mission;

    // json変換するデータのクラス
    [HideInInspector] public MissionSaveData data;

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
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(MissionSaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    MissionSaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<MissionSaveData>(json);     // jsonファイルを型に戻して返す
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }
}
