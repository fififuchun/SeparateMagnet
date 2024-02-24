using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

// public struct MatrixTextDatas
// {

// }

public class MatrixText : MonoBehaviour
{
    //状況ごとのチュートリアルメッセージ
    [SerializeField] private MatrixTextSO matrixTextSO;

    //自分自身
    private TextMeshProUGUI matrixElementText;

    //tutorialSOの位置
    //行
    [SerializeField] private int matrixRowNum;
    //列
    [SerializeField, ReadOnly] private int matrixColumnNum;

    // json変換するデータのクラス
    // [HideInInspector] public MatrixTextData[] matrixTextDatas;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "MatrixTextData.json";


    //==================================================(50)
    //関数
    // 開始時にファイルチェック、読み込み
    void Awake()
    {
        // パス名取得
        filepath = Application.dataPath + "/" + fileName;

        // ファイルがないとき、ファイル作成
        if (!File.Exists(filepath)) Save(matrixTextSO);

        // ファイルを読み込んでdataに格納
        // matrixTextSO.stringGroups = Load(filepath);
    }

    void Start()
    {
        Initialize(matrixRowNum);
    }

    // jsonとしてデータを保存
    public void Save(MatrixTextSO data)
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

    // // jsonファイル読み込み
    MatrixTextData[] Load(string path)
    {
        // ファイル読み込み指定
        StreamReader rd = new StreamReader(path);

        // ファイル内容全て読み込む
        string json = rd.ReadToEnd();
        Debug.Log(json);

        // ファイル閉じる
        rd.Close();

        // jsonファイルを型に戻して返す
        return JsonUtility.FromJson<MatrixTextData[]>(json);
    }

    // ゲーム終了時に保存
    void OnDestroy()
    {
        Save(matrixTextSO);
    }

    /// <summary>
    /// matrixRowNumを初期化
    /// </summary>
    public void Initialize(int _matrixRowNum)
    {
        //指定された_matrixRowNumがtutorialSOの行数を超えてたらreturn
        if (_matrixRowNum < 0 || matrixTextSO.stringGroups.Count() <= _matrixRowNum)
        {
            // Destroy(transform.parent.gameObject);
            gameObject.transform.parent.gameObject.SetActive(false);
            Debug.Log("入力されたmatrixRowNumは存在しません");
            return;
        }
        //指定された_matrixRowNumがtutorialSOの行数を超えてなかったら初期化
        matrixRowNum = _matrixRowNum;

        InitializeText();
        transform.parent.GetComponent<Button>()?.onClick.AddListener(PushGoNextButton);
        // Debug.Log($"{matrixRowNum}に変更");
    }

    /// <summary>
    /// テキストを初期化
    /// </summary>
    public void InitializeText()
    {
        if (gameObject.GetComponent<TextMeshProUGUI>() == null)
        {
            Debug.Log("textコンポーネントをアタッチしてください");
            return;
        }
        matrixElementText = gameObject.GetComponent<TextMeshProUGUI>();
        Debug.Log("MatrixText: 初期化されました");

        matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }

    /// <summary>
    /// matrixTextの列を次に進める
    /// </summary>
    public void PushGoNextButton()
    {
        Debug.Log("次へ進む");
        matrixColumnNum++;
        if (matrixTextSO.stringGroups[matrixRowNum].strings.Length <= matrixColumnNum)
        {
            Debug.Log("テキストがありません");
            matrixColumnNum = 0;
            gameObject.transform.parent.gameObject.SetActive(false);
            return;
        }

        matrixElementText.text = matrixTextSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }
}
