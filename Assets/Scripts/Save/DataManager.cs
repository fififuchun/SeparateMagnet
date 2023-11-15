using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    // json変換するデータのクラス
    [HideInInspector] public SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Data.json";

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
        CheakSaveData();
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(data);
    }

    void CheakSaveData()
    {
        if (data.level[0] < 4 || data.level[0] > 7) data.level[0] = 4;
        if (data.level[1] < 5 || data.level[1] > 60) data.level[1] = 10;
        if (data.level[2] < 10 || data.level[2] > 60) data.level[2] = 10;
        if (data.level[3] < 5 || data.level[3] > 10) data.level[3] = 5;
        if (data.level[4] < 2) data.level[4] = 2;
        //減ってく
        if (data.level[5] < 5 || data.level[5] > 10) data.level[5] = 10;
        if (data.level[6] < 50 || data.level[6] > 100) data.level[6] = 100;
    }
}
