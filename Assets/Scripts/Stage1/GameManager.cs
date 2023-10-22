using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    public TimeManager timeManager;

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
                    coinText.text = SumCoin().ToString();
                    yield return new WaitForSeconds(1f);

                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    PutKento();

                    //10秒待って動きなしならドラック終了とみなす
                    StartTimeOver();
                    //ドラッグ終了まで待つ、動きがあったらTimeOver()は止める
                    yield return new WaitUntil(() => timeManager.isEndDrag);
                    EndDrag();

                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitUntil(() => canInstantiate);

                    phase = Phase.StartPhase;
                    break;
            }
        }
    }

    //timeOver用のコルーチンとリセット&実行関数
    IEnumerator timeOver;
    public void StartTimeOver()
    {
        timeOver = null;
        timeOver = timeManager.TimeOver();
        StartCoroutine(timeOver);
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                placedGameObjects.RemoveAt(i);
                // if (phase == Phase.PutPhase) CanInstantiate();
                Debug.Log("落ちたよ");
            }
    }

    //現在落下準備中のkentoPrefabをPutKentoする
    [SerializeField] private GameObject ReadyKento;
    public void PutKento()
    {
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
        ReadyKento = Instantiate(myGameObjects[Random.Range(0, 6)][Random.Range(0, 3)].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
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
        ResetKentoPrefab();
        StopCoroutine(timeOver);
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