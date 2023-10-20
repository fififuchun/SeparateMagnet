using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    [SerializeField] private TimeManager timeManager;

    //canvas
    public GameObject canvas;

    //kentoPrefabの親オブジェクト
    [SerializeField] private GameObject kentos;

    //ゲームごとの使い捨てのゲームオブジェクトをここに入れる
    [SerializeField] private List<List<KentoData>> myGameObjects = new List<List<KentoData>>();

    // private HashSet<int> myGameObjects = new HashSet<int>() { 0, 1, 2, 3 };

    //検討の元データ
    [SerializeField] private Kento kentoSO;

    [SerializeField] private TextMeshProUGUI coinText;


    [Header("以上入力エリア")]
    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();
    // public List<KentoData> placedGameObjects = new List<KentoData>();

    //phase管理
    public Phase phase;
    public enum Phase
    {
        //一応、一度きり
        StartPhase,

        //次の検討の出現、一度きり
        AppearPhase,

        //落ちてる状態、いらない？
        PutPhase,
    }

    //生成可能か、着地したらtrue
    public bool canInstantiate;
    public void CanInstantiate() { canInstantiate = true; }

    //ドラッグ終了したらtrue
    public bool isEndDrag;
    public void IsEndDrag(bool judge) { isEndDrag = judge; }


    //関数の部
    void Start()
    {
        phase = Phase.StartPhase;
        StartCoroutine(Loop());

        myGameObjects.Add(kentoSO.fontData[0].sizeData);
        myGameObjects.Add(kentoSO.fontData[1].sizeData);
        myGameObjects.Add(kentoSO.fontData[2].sizeData);
        myGameObjects.Add(kentoSO.fontData[2].sizeData);
        myGameObjects.Add(kentoSO.fontData[3].sizeData);
        myGameObjects.Add(kentoSO.fontData[3].sizeData);
    }

    IEnumerator Loop()
    {
        while (true)
        {
            Debug.Log(phase);
            switch (phase)
            {
                case Phase.StartPhase:
                    IsEndDrag(false);
                    coinText.text = SumCoin().ToString();
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    PutKento();
                    isEndDrag = false;
                    timeManager.debug = true;

                    //10秒待って動きなしならドラック終了とみなす
                    timeOver = null;
                    timeOver = TimeOver();
                    StartCoroutine(timeOver);
                    //ドラッグ終了まで待つ、動きがあったらTimeOver()は止める
                    yield return new WaitUntil(() => isEndDrag);
                    EndDrag();
                    timeManager.debug = false;
                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitUntil(() => canInstantiate);
                    phase = Phase.StartPhase;
                    break;
            }
        }
    }

    IEnumerator timeOver;
    //時間切れ
    IEnumerator TimeOver()
    {
        // StartCoroutine(timeManager.StartTimer());
        // timeManager.StartTimer();
        Debug.Log("ドラッグ待ちだよ");

        timeManager.timerText.text = "10";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "9";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "8";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "7";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "6";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "5";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "4";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "3";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "2";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "1";
        yield return new WaitForSeconds(1f);
        timeManager.timerText.text = "0";
        Debug.Log("TIME OVER");
        timeManager.resultCoinText.text = SumCoin().ToString();
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax) timeManager.FinishGame();
        IsEndDrag(true);
        yield break;
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                // Destroy(placedGameObjects[i].KentoPrefab);//変更
                placedGameObjects.RemoveAt(i);
                // if (phase == Phase.PutPhase) CanInstantiate();
                Debug.Log("落ちたよ");
            }
    }

    //現在落下準備中のkentoPrefabをPutKentoする
    [SerializeField] private GameObject ReadyKento;
    int randomFontNum;
    int randomSizeNum;
    public void PutKento()
    {
        timeManager.StartTimer();
        canInstantiate = false;
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax)
        {
            Debug.Log("もうこれ以上怒れないよ");
            return;
        }

        if (Random.Range(0, 100) == 0)
        {
            // int index = int.Parse(SceneManager.GetActiveScene().name.Split("_")[1]);
            ReadyKento = Instantiate(kentoSO.fontData[kentoSO.fontData.Count() - 1].sizeData[0].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
            return;
        }

        //Prefabを生成してListに追加
        randomFontNum = Random.Range(0, 6);
        randomSizeNum = Random.Range(0, 3);
        ReadyKento = Instantiate(myGameObjects[randomFontNum][randomSizeNum].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
        // Debug.Log("コイン:" + CoinOf(ReadyKento.GetComponent<KentoManager>()));

        // Debug.Log(ReadyKento.name.Split("(")[0]);
    }

    //kentoPrefabの中身をnullに戻す
    public void ResetKentoPrefab() { ReadyKento = null; }

    //ドラッグ終了
    public void EndDrag()
    {
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax) return;
        if (Random.Range(0, 2) == 2) timeManager.MakeAngry();
        if (ReadyKento != null) placedGameObjects.Add(ReadyKento.GetComponent<KentoManager>());
        ReadyKento.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        // IsEndDrag(true);
        ResetKentoPrefab();
        StopCoroutine(timeOver);
        timeOver = null;
        // timeOver=TimeOver();
        timeManager.timerText.text = "";
        Debug.Log("drag終了");
    }

    //kentoPrefabの回転
    public void PushRotateButton() { if (ReadyKento != null) ReadyKento.transform.Rotate(new Vector3(0, 0, 45)); }

    //置かれた検討の数
    private int putKentoCount;
    public int KentoSpeed()
    {
        putKentoCount++;
        // if (putKentoCount != 0 && putKentoCount % 10 == 0) エフェクト;

        return Mathf.FloorToInt(putKentoCount / 10) * 5 + 20;
    }

    // public int CoinOf()
    // {
    //     int sum = 0;
    //     for (int i = 0; i < kentoSO.fontData.Count(); i++)
    //     {
    //         // Debug.Log(kentoSO.fontData.Count());
    //         for (int j = 0; j < kentoSO.fontData[i].sizeData.Count(); j++)
    //         {
    //             // Debug.Log(kentoSO.fontData[i].sizeData.Count());
    //             // return kentoSO.fontData[i].sizeData[j].FindObject(kento);
    //             for (int k = 0; k < placedGameObjects.Count(); k++)
    //             {
    //                 if (kentoSO.fontData[i].sizeData[j].KentoPrefab.name == ReadyKento.name.Split("(")[0]) return kentoSO.fontData[i].sizeData[j].FindObject(placedGameObjects[k]);
    //                 sum += placedGameObjects[k].score;
    //             }
    //         }
    //     }
    //     return sum;
    // }

    public int CoinOf(KentoManager kento)
    {
        for (int i = 0; i < kentoSO.fontData.Count(); i++)
        {
            // Debug.Log(kentoSO.fontData.Count());
            for (int j = 0; j < kentoSO.fontData[i].sizeData.Count(); j++)
            {
                if (kentoSO.fontData[i].sizeData[j].KentoPrefab.name == kento.name.Split("(")[0])
                {
                    // Debug.Log(kentoSO.fontData[i].sizeData[j].score);
                    return kentoSO.fontData[i].sizeData[j].score;
                }
                // Debug.Log(kentoSO.fontData[i].sizeData.Count());
                // return kentoSO.fontData[i].sizeData[j].FindObject(kento);
                // for (int k = 0; k < placedGameObjects.Count(); k++)
                // {
                //     if (kentoSO.fontData[i].sizeData[j].KentoPrefab.name == ReadyKento.name.Split("(")[0]) return kentoSO.fontData[i].sizeData[j].FindObject(placedGameObjects[k]);
                //     sum += placedGameObjects[k].score;
                // }
            }
        }
        return 0;
    }

    public int SumCoin()
    {
        if (placedGameObjects.Count() == 0) return 0;
        int sum = 0;
        for (int i = 0; i < placedGameObjects.Count(); i++) sum += CoinOf(placedGameObjects[i]);
        return sum;
    }
}