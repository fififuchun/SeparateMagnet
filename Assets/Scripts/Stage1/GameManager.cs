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

    //検討の元データ
    [SerializeField] private Kento kentoSO;

    [SerializeField] private TextMeshProUGUI coinText;


    [Header("以上入力エリア")]
    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();

    //生成可能か、着地したらtrue
    public bool canInstantiate;
    public void CanInstantiate() { canInstantiate = true; }


    //関数の部
    void Start()
    {
        phase = Phase.StartPhase;
        StartCoroutine(Loop());

        //要修正
        myGameObjects.Add(kentoSO.fontData[0].sizeData);
        myGameObjects.Add(kentoSO.fontData[1].sizeData);
        myGameObjects.Add(kentoSO.fontData[2].sizeData);
        myGameObjects.Add(kentoSO.fontData[2].sizeData);
        myGameObjects.Add(kentoSO.fontData[3].sizeData);
        myGameObjects.Add(kentoSO.fontData[3].sizeData);
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                placedGameObjects.RemoveAt(i);
                coinText.text = SumCoin().ToString();
                Debug.Log("落ちたよ");
            }
    }

    //phase管理
    public Phase phase;
    public enum Phase
    {
        //Objectとの接触でStartPhase
        StartPhase,

        //1秒経ったらAppearPhase
        AppearPhase,

        //落ちてる状態、指を離すか10秒経ったらPutPhase
        PutPhase,

        //怒りゲージがMaxになったらEnd
        End
    }

    IEnumerator Loop()
    {
        while (true)
        {
            switch (phase)
            {
                case Phase.StartPhase:
                    coinText.text = SumCoin().ToString();
                    // GoEndPhase();
                    if (timeManager.isAnger())
                    {
                        phase = Phase.End;
                        break;
                    }

                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    //怒ってたらスルー
                    if (timeManager.isAnger())
                    {
                        phase = Phase.PutPhase;
                        break;
                    }
                    //動きなし:TimeOver / 動きあり:TimeOver Stop & isEndDrag= true
                    PutKento();
                    StartTimeOver();

                    yield return new WaitUntil(() => timeManager.isEndDrag);
                    EndDrag();
                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitUntil(() => canInstantiate);
                    phase = Phase.StartPhase;
                    // GoEndPhase();
                    break;
                case Phase.End:
                    StopCoroutine(timeOver);
                    StartCoroutine(timeManager.FinishGame());
                    yield break;
            }
        }
    }

    //timeOver用のコルーチン、リセット&実行関数
    IEnumerator timeOver;
    public void StartTimeOver()
    {
        timeOver = null;
        timeOver = timeManager.TimeOver();
        StartCoroutine(timeOver);
    }

    //怒りゲージによってEndにする
    public void GoEndPhase() { if (timeManager.isAnger()) phase = Phase.End; }

    //現在落下準備中のkentoPrefabをPutKentoする
    [SerializeField] private GameObject ReadyKento;
    public void PutKento()
    {
        canInstantiate = false;
        if (timeManager.isAnger())
        {
            Debug.Log("もうこれ以上怒れないよ");
            return;
        }

        if (Random.Range(0, 100) == 0)
        {
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
        if (timeManager.isAnger())
        {
            timeOver = null;
            StartCoroutine(timeManager.FinishGame());
            return;
        }
        if (Random.Range(0, 2) == 2) timeManager.MakeAngry();
        if (ReadyKento != null) placedGameObjects.Add(ReadyKento.GetComponent<KentoManager>());
        ReadyKento.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        timeManager.EmptyTimerText();
        ResetKentoPrefab();
        StopCoroutine(timeOver);
        Debug.Log("drag終了");
    }

    //kentoPrefabの回転
    public void PushRotateButton() { if (ReadyKento != null) ReadyKento.transform.Rotate(new Vector3(0, 0, 45)); }

    //置かれた検討の数とそれに応じたスピード
    private int putKentoCount;
    public int KentoSpeed()
    {
        putKentoCount++;
        // if (putKentoCount != 0 && putKentoCount % 10 == 0) エフェクト;

        return Mathf.FloorToInt(putKentoCount / 10) * 5 + 20;
    }

    //kentoManagerのインスタンスからコインの量を計測
    public int CoinOf(KentoManager kento)
    {
        for (int i = 0; i < kentoSO.fontData.Count(); i++) for (int j = 0; j < kentoSO.fontData[i].sizeData.Count(); j++) if (kentoSO.fontData[i].sizeData[j].KentoPrefab.name == kento.name.Split("(")[0]) return kentoSO.fontData[i].sizeData[j].score;
        return 0;
    }

    //コインの合計
    public int SumCoin()
    {
        if (placedGameObjects.Count() == 0) return 0;
        int sum = 0;
        for (int i = 0; i < placedGameObjects.Count(); i++) sum += CoinOf(placedGameObjects[i]);
        timeManager.sumCoin = sum;
        return sum;
    }
}