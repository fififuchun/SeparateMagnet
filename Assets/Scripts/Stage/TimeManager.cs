using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.Events;

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
    [SerializeField] private ResultManager resultManager;

    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //怒るときのエフェクト
    [SerializeField] private ParticleSystem angryEffect;

    //Header
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultCoinText;
    [SerializeField] private TextMeshProUGUI finishText;

    //結果表示
    [SerializeField] private GameObject resultObject;

    //Unitaskキャンセル周り
    // private CancellationTokenSource cts;


    //関数の部
    [HideInInspector] public SaveData data;
    void Awake()
    {
        data = Load(Application.dataPath + "/Data.json");

        angerGaugeMax = data.level[0] + 4;
        angryTime = data.level[1] + 10;
        angryLateTime = data.level[2] + 10;
        canHoldTime = data.level[3] + 5;
        angerRate = data.level[4] + 2;
        nextAppearTime = (float)(10 - data.level[5]) / 10;
        rareRate = 100 - data.level[6];

        // cts = new CancellationTokenSource();
    }

    SaveData Load(string path)
    {
        StreamReader rd = new StreamReader(path);               // ファイル読み込み指定
        string json = rd.ReadToEnd();                           // ファイル内容全て読み込む
        rd.Close();                                             // ファイル閉じる

        return JsonUtility.FromJson<SaveData>(json);            // jsonファイルを型に戻して返す
    }

    void Start()
    {
        angryTime = 10;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        InvokeRepeating("MakeAngry", angryLateTime, angryTime);

        resultManager.InitializeResult();
        resultManager.countSumCoin.AddListener(CountSumCoin);
    }

    void Update()
    {
        slider.value = (float)AngerGauge / (float)angerGaugeMax;
    }

    //angerGauge操作はここだけ
    public void MakeAngry()
    {
        angerGauge++;
        if (IsAnger()) return;
        Instantiate(angryEffect);
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
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
        Debug.Log("テキストを空に");
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

    public UnityEvent countSumCoin;
    public void CountSumCoin()
    {
        countSumCoin?.Invoke();
    }

    public async UniTask FinishGame(CancellationToken ct)
    {
        IsEndDrag(true);
        Debug.Log("ゲーム終了が起動");
        float startTimer = Time.time;
        canHoldTime = 5;
        timerText.color = Color.red;
        finishText.enabled = true;

        for (int i = 0; i < canHoldTime; i++)
        {
            timerText.text = Mathf.Ceil(canHoldTime + startTimer - Time.time).ToString();
            await UniTask.Delay(1000, cancellationToken: ct);
        }

        resultManager.AppearResult(sumCoin).Forget();
    }
}
