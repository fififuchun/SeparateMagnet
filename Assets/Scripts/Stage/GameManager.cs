using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
// using System;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    public TimeManager timeManager;

    //canvas
    public GameObject canvas;

    //kentoPrefabの親オブジェクト
    [SerializeField] private GameObject kentos;

    //ゲームごとの使い捨てのゲームオブジェクトをここに入れる
    private GameObject[,] myGameObjects = new GameObject[3, 6];

    //検討の元データ
    [SerializeField] private Kento kentoSO;

    //ヘッダーのコインテキスト
    [SerializeField] private TextMeshProUGUI coinText;

    //検討出現時のエフェクト
    [SerializeField] private ParticleSystem[] appearEffects = new ParticleSystem[6];

    //音楽
    [SerializeField] private AudioSource audioSource;

    //MatrixTextButtonのGameObject
    [SerializeField] GameObject matrixTextPatentObj;

    //data
    [SerializeField] DataManager dataManager;


    [Header("以上入力エリア")]
    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();

    //生成可能か、着地したらtrue
    public bool canInstantiate;
    public void CanInstantiate() { canInstantiate = true; }

    //Unitaskキャンセル周り
    //ループとFinish以外のCancellationTokenSource
    CancellationTokenSource cts;
    //ループ処理用のCancellationTokenSource
    CancellationTokenSource cts_loop;
    //ゲーム終了時用のCancellationTokenSource
    CancellationTokenSource cts_finish;

    //
    List<int> debugIntList = new List<int>();


    //関数の部
    async void Start()
    {
        phase = Phase.StartPhase;
        await UniTask.WaitUntil(() => !matrixTextPatentObj.activeSelf);

        cts = new CancellationTokenSource();
        cts_loop = new CancellationTokenSource();
        cts_finish = new CancellationTokenSource();
        Loop(cts_loop.Token).Forget();

        timeManager.resultManager.countSumCoin.AddListener(UpdateSumCoin);

        //myGameObjectsを初期化 GetLength(1)= 6, GetLength(0)= 3
        for (int i = 0; i < myGameObjects.GetLength(1); i++)
        {
            int k = Random.Range(0, 20);
            for (int j = 0; j < myGameObjects.GetLength(0); j++)
            {
                //fontNumbersが空・未解放の場合はランダムなフォントを代入
                if (dataManager.data.fontNumbers[i] <= 0)
                {
                    myGameObjects[j, i] = kentoSO.sizeData[j].kentoPrefabs[k];
                    continue;
                }

                //fontNumbersが1以上ならそれを代入
                myGameObjects[j, i] = kentoSO.sizeData[j].kentoPrefabs[dataManager.data.fontNumbers[i] - 1];
            }

            if (dataManager.data.fontNumbers[i] <= 0) debugIntList.Add(k);
            else debugIntList.Add(dataManager.data.fontNumbers[i]);
        }

        for (int i = 0; i < 6; i++)
        {
            Debug.Log($"List[{i}]: " + debugIntList[i]);
        }
    }

    void OnDestroy()
    {
        // GameObject破棄時にキャンセル実行
        cts?.Cancel();
        cts_loop?.Cancel();
        cts_finish?.Cancel();
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

    async UniTask Loop(CancellationToken ct_loop)
    {
        while (!timeManager.IsAnger())
        {
            switch (phase)
            {
                case Phase.StartPhase:
                    coinText.text = SumCoin().ToString();
                    // ResetKentoPrefab();
                    Instantiate(appearEffects[timeManager.data.level[5]]);

                    // Debug.Log($"エフェクト出現、{timeManager.NextAppearTime}秒待機");
                    await UniTask.Delay((int)timeManager.NextAppearTime * 1000, cancellationToken: ct_loop);

                    phase = Phase.AppearPhase;
                    break;

                case Phase.AppearPhase:
                    audioSource.Play();
                    PutKento();

                    //時間記録
                    float currentTime = Time.time;

                    cts = new CancellationTokenSource();

                    //動きなし:Count10Seconds / 動きあり: cts.TokenをStartPhaseでCancel & isEndDrag= true
                    timeManager.Count10Seconds(cts.Token).Forget();
                    await UniTask.WaitUntil(() => timeManager.isEndDrag || currentTime + timeManager.CanHoldTime < Time.time, cancellationToken: ct_loop);

                    EndDrag();
                    phase = Phase.PutPhase;
                    break;

                case Phase.PutPhase:
                    cts.Cancel();

                    await UniTask.WaitUntil(() => canInstantiate || readyKento.transform.position.y < -900, cancellationToken: ct_loop);
                    ResetKentoPrefab();

                    phase = Phase.StartPhase;
                    break;
            }
        }

        //怒りゲージMax以降の動き
        phase = Phase.End;
        coinText.text = SumCoin().ToString();
        timeManager.FinishGame(cts_finish.Token).Forget();
    }

    //現在落下準備中のkentoPrefabをPutKentoする
    [SerializeField] private GameObject readyKento;
    public void PutKento()
    {
        canInstantiate = false;
        if (timeManager.IsAnger())
        {
            Debug.Log("もうこれ以上怒れないよ");
            return;
        }

        if (Random.Range(0, 2/*timeManager.RareRate*/) == 0)
        {
            readyKento = Instantiate(kentoSO.sizeData[kentoSO.sizeData.Count() - 1].kentoPrefabs[MainManager.stageNum - 1], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
            //ミッション用
            timeManager.data.isRareFonts[MainManager.stageNum - 1] = true;
            return;
        }

        //Prefabを生成してListに追加・修正
        int randomSizeNumber = Random.Range(0, 3);
        readyKento = Instantiate(myGameObjects[randomSizeNumber, Random.Range(0, 6)], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
        readyKento.GetComponent<KentoManager>().score = kentoSO.sizeData[randomSizeNumber].score;
    }

    //kentoPrefabの中身をnullに戻す
    public void ResetKentoPrefab() { readyKento = null; }

    //ドラッグ終了
    public void EndDrag()
    {
        if (timeManager.IsAnger())
        {
            readyKento.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
            if (readyKento != null) placedGameObjects.Add(readyKento.GetComponent<KentoManager>());
            Debug.Log("Drag強制終了");
            return;
        }
        if (Random.Range(0, timeManager.AngerRate) == 0) timeManager.MakeAngry();
        if (readyKento != null) placedGameObjects.Add(readyKento.GetComponent<KentoManager>());
        readyKento.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        timeManager.EmptyTimerText();
        Debug.Log("Drag終了");
    }

    //kentoPrefabの回転
    public void PushRotateButton() { if (phase == Phase.AppearPhase) readyKento.transform.Rotate(new Vector3(0, 0, 45)); }

    //置かれた検討の数とそれに応じたスピード
    private int putKentoCount;
    public GameObject tmpObject;
    public int KentoSpeed()
    {
        putKentoCount++;
        if (putKentoCount != 0 && putKentoCount % 5 == 0) Instantiate(tmpObject, canvas.transform);
        return Mathf.FloorToInt(putKentoCount / 5) * 5 + 20;
    }

    //コインの合計
    public int SumCoin()
    {
        if (placedGameObjects.Count() == 0) return 0;
        int sum = 0;
        for (int i = 0; i < placedGameObjects.Count(); i++) sum += placedGameObjects[i].score;
        timeManager.sumCoin = sum;
        return sum;
    }

    public void UpdateSumCoin()
    {
        timeManager.sumCoin = SumCoin();
    }
}