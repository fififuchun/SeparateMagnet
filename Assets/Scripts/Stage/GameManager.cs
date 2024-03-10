using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    public TimeManager timeManager;

    //canvas
    public GameObject canvas;

    //kentoPrefabの親オブジェクト
    [SerializeField] private GameObject kentos;

    //ゲームごとの使い捨てのゲームオブジェクトをここに入れる
    // [SerializeField] private List<List<KentoData>> myGameObjects = new List<List<KentoData>>();
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

    //
    [SerializeField] DataManager dataManager;


    [Header("以上入力エリア")]
    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();

    //生成可能か、着地したらtrue
    public bool canInstantiate;
    public void CanInstantiate() { canInstantiate = true; }


    //関数の部
    async void Start()
    {
        phase = Phase.StartPhase;
        await UniTask.WaitUntil(() => !matrixTextPatentObj.activeSelf);
        StartCoroutine(Loop());

        //myGameObjectsを初期化
        for (int i = 0; i < myGameObjects.GetLength(0); i++)
        {
            for (int j = 0; j < myGameObjects.GetLength(1); j++)
            {
                myGameObjects[i, j] = kentoSO.sizeData[i].kentoPrefabs[dataManager.data.fontNumbers[j]];
            }
        }

        //要修正
        // myGameObjects.Add(kentoSO.sizeData[0].fontData);
        // myGameObjects.Add(kentoSO.sizeData[1].fontData);
        // myGameObjects.Add(kentoSO.sizeData[2].fontData);
        // myGameObjects.Add(kentoSO.sizeData[2].fontData);
        // myGameObjects.Add(kentoSO.sizeData[3].fontData);
        // myGameObjects.Add(kentoSO.sizeData[3].fontData);
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
                    Instantiate(appearEffects[timeManager.data.level[5]]);

                    Debug.Log("現在の出現時間は：" + TimeManager.NextAppearTime);
                    yield return new WaitForSeconds(TimeManager.NextAppearTime);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    //怒ってたらスルー
                    if (timeManager.isAnger())
                    {
                        phase = Phase.PutPhase;
                        if (ReadyKento != null) ReadyKento.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
                        break;
                    }
                    audioSource.Play();
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
                    timeManager.FinishGame();
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
        if (timeManager.isAnger()) return;
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

        //要修正
        if (Random.Range(0, timeManager.RareRate) == 0)
        {
            // ReadyKento = Instantiate(kentoSO.sizeData[kentoSO.sizeData.Count() - 1].fontData[0].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
            // timeManager.data.isRareFonts[int.Parse(SceneManager.GetActiveScene().name.Split('_')[1])] = true;
            // Debug.Log($"STAGE{int.Parse(SceneManager.GetActiveScene().name.Split('_')[1])}のレアなフォント出現");
            // return;
        }

        //Prefabを生成してListに追加・修正
        // ReadyKento = Instantiate(myGameObjects[Random.Range(0, 6)][Random.Range(0, 3)].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
    }

    //kentoPrefabの中身をnullに戻す
    public void ResetKentoPrefab() { ReadyKento = null; }

    //ドラッグ終了
    public void EndDrag()
    {
        if (timeManager.isAnger())
        {
            timeOver = null;
            timeManager.FinishGame();
            return;
        }
        if (Random.Range(0, timeManager.AngerRate) == 0) timeManager.MakeAngry();
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
        // Instantiate(tmpObject, canvas.transform);
        if (putKentoCount != 0 && putKentoCount % 5 == 0) Instantiate(tmpObject, canvas.transform);
        return Mathf.FloorToInt(putKentoCount / 5) * 5 + 20;
    }

    public GameObject tmpObject;

    //kentoManagerのインスタンスからコインの量を計測
    //修正
    public int CoinOf(KentoManager kento)
    {
        // for (int i = 0; i < kentoSO.sizeData.Count(); i++) for (int j = 0; j < kentoSO.sizeData[i].fontData.Count(); j++) if (kentoSO.sizeData[i].fontData[j].KentoPrefab.name == kento.name.Split("(")[0]) return kentoSO.sizeData[i].fontData[j].score;

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