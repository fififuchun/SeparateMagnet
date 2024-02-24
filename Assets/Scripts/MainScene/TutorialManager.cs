using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

enum TutorialImage
{
    Bow = 0,
    Cheak = 1,
    Glad = 2,
    Go = 3,
    Good = 4,
    Info = 5,
    Tele = 6,
    Wink = 7,
}

public class TutorialManager : MonoBehaviour
{
    //描画キャンバス・生成用Prefab
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tutorialPrefab;

    //何番目を出力するか
    [SerializeField, Header("ColumnNumber?")] private int matrixColumnNum;


    void Start()
    {
        InstantiateTutorial(matrixColumnNum);
    }

    /// <summary>
    /// tutorialを生成
    /// </summary>
    /// <param name="_matrixRowNum">行</param>
    public void InstantiateTutorial(int _matrixRowNum)
    {
        //tutorialPrefabを複製
        GameObject tutorialObj = Instantiate(tutorialPrefab, canvas.transform);

        //tutorialObj直下のMatrixTextが存在するなら[_matrixRowNum, 0]を生成
        if (tutorialObj.transform.GetChild(tutorialObj.transform.childCount - 1).GetComponent<MatrixText>() == null)
        {
            Debug.Log("MatrixTextコンポーネントを親ボタン直下かつ一番下のテキストオブジェクトにアタッチして下さい");
            return;
        }

        tutorialObj.transform.GetChild(tutorialObj.transform.childCount - 1).GetComponent<MatrixText>()?.Initialize(_matrixRowNum);
    }

    // json変換するデータのクラス
    [HideInInspector] public MatrixText matrixTextData;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "MatrixTextData.json";

    //-------------------------------------------------------------------
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) Save(matrixTextData);

        // ファイルを読み込んでdataに格納
        matrixTextData = Load(filepath);
    }

    //-------------------------------------------------------------------
    // jsonとしてデータを保存
    public void Save(MatrixText data)
    {
        string json = JsonUtility.ToJson(data);                 // jsonとして変換
        StreamWriter wr = new StreamWriter(filepath, false);    // ファイル書き込み指定
        wr.WriteLine(json);                                     // json変換した情報を書き込み
        wr.Close();                                             // ファイル閉じる
    }

    // jsonファイル読み込み
    MatrixText Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<MatrixText>(json);            // jsonファイルを型に戻して返す
    }

    //-------------------------------------------------------------------
    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(matrixTextData);
    }
}
