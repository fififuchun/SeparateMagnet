using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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

        Debug.Log(data.fontNumbers[0]);
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(SaveData data)
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

    public void ResetDataManager()
    {
        data.level = new int[SaveData.levelCount];
        data.fontNumbers = new int[6] { 0, 0, 0, -1, -1, -1 };
        for (int i = 0; i < data.isRareFonts.Length; i++) data.isRareFonts[i] = false;
        for (int i = 0; i < data.haveFonts.Length; i++) data.haveFonts[i] = false;
    }
}
