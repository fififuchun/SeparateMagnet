using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

//ゲームオーバー管理
public class TimeManager : MonoBehaviour
{
    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax;
    public int AngerGaugeMax { get => angerGaugeMax; }

    //angryTime秒で国民が1怒る
    private int angryTime;
    private int angryLateTime;

    //RPGの各データ
    private int canHoldTime;
    public int CanHoldTime { get => canHoldTime; }

    private int angerRate;
    public int AngerRate { get => angerRate; }

    private float nextAppearTime;
    public float NextAppearTime { get => nextAppearTime; }

    private int rareRate;
    public int RareRate { get => rareRate; }

    //ゲーム終了時の最終コイン
    public int sumCoin;

    //ドラッグ終了したらtrue
    public bool isEndDrag;
    public void IsEndDrag(bool judge) { isEndDrag = judge; }

    [Header("以下はアタッチが必要")]
    //インスタンス
    public ResultManager resultManager;

    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //怒るときのエフェクト
    [SerializeField] private ParticleSystem angryEffect;

    //Header
    [SerializeField] private Slider angerSlider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI finishText;


    //関数の部
    [HideInInspector] public SaveData data;

    // jsonファイルのパス
    string filepath;

    // jsonファイル名
    string fileName = "Data.json";


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

        // ファイルを読み込んでdataに格納
        data = Load(filepath);

        angerGaugeMax = data.level[0] + 4;
        angryTime = data.level[1] + 10;
        angryLateTime = data.level[2] + 10;
        canHoldTime = data.level[3] + 5;
        angerRate = data.level[4] + 2;
        nextAppearTime = (float)(10 - data.level[5]) / 10;
        rareRate = 100 - data.level[6];
    }

    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);
        string json = rd.ReadToEnd();
        rd.Close();

        return JsonUtility.FromJson<SaveData>(json);
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

    void Start()
    {
        angryTime = 10;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        InvokeRepeating("MakeAngry", angryLateTime, angryTime);

        resultManager.InitializeResult();
    }

    //angerGauge操作はここだけ
    public void MakeAngry()
    {
        if (IsAnger()) return;
        angerGauge++;
        // Debug.Log("AngerGauge:" + AngerGauge);

        //エフェクト
        Instantiate(angryEffect, angryImage.transform.parent);

        //スライダー更新
        angerSlider.value = (float)AngerGauge / (float)angerGaugeMax;

        if (7 - AngerGaugeMax + AngerGauge == 6)
        {
            angryImage.sprite = angryImages[6];
            angryImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(120, 130);
        }
        else if (7 - AngerGaugeMax + AngerGauge == 7)
        {
            angryImage.sprite = angryImages[7];
            angryImage.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(240, 200);
        }
        else
        {
            angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        }
    }

    //gameManagerで使う用
    public bool IsAnger()
    {
        if (AngerGauge >= AngerGaugeMax) return true;
        else return false;
    }

    //timerTextを空にする
    public void EmptyTimerText()
    {
        timerText.text = "";
        Debug.Log("Make Text Empty");
    }

    //時間切れ
    public async UniTask Count10Seconds(CancellationToken ct)
    {
        float startTimer = Time.time;
        for (int i = 0; i < canHoldTime; i++)
        {
            timerText.text = Mathf.Ceil(canHoldTime + startTimer - Time.time).ToString();
            await UniTask.Delay(1000, cancellationToken: ct);
        }

        Debug.Log("TIME OVER");
        EmptyTimerText();
        IsEndDrag(true);
    }

    public async UniTask FinishGame(CancellationToken ct)
    {
        IsEndDrag(true);
        Debug.Log("Finish Game");
        float startTimer = Time.time;
        canHoldTime = 5;
        timerText.color = Color.red;
        finishText.enabled = true;

        for (int i = 0; i < canHoldTime; i++)
        {
            timerText.text = Mathf.Ceil(canHoldTime + startTimer - Time.time).ToString();
            await UniTask.Delay(1000, cancellationToken: ct);
        }

        Save(data);
        resultManager.AppearResult(sumCoin).Forget();
    }
}
