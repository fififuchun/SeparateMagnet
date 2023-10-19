using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    //検討の元データ
    [SerializeField] private Kento kentoSO;

    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();

    [Header("以上入力エリア")]
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

        myGameObjects.Add(kentoSO.kento[0].kentoFont);
        myGameObjects.Add(kentoSO.kento[1].kentoFont);
        myGameObjects.Add(kentoSO.kento[2].kentoFont);
        myGameObjects.Add(kentoSO.kento[3].kentoFont);
        myGameObjects.Add(kentoSO.kento[3].kentoFont);
        myGameObjects.Add(kentoSO.kento[3].kentoFont);

        Debug.Log(myGameObjects.Count());
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
                    // CanInstantiate();
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    PutKento();
                    isEndDrag = false;

                    //10秒待って動きなしならドラック終了とみなす
                    StartCoroutine(TimeOver());
                    //ドラッグ終了まで待つ、動きがあったらTimeOver()は止める
                    yield return new WaitUntil(() => isEndDrag);
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

    //時間切れ
    IEnumerator TimeOver()
    {
        yield return new WaitForSeconds(5f);
        IsEndDrag(true);
        yield break;
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].gameObject.transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                placedGameObjects.RemoveAt(i);
                // if (phase == Phase.PutPhase) CanInstantiate();
                Debug.Log("落ちたよ");
            }
    }

    //現在落下準備中のkentoPrefabをPutKentoする
    [SerializeField] private GameObject kentoPrefab;
    public void PutKento()
    {
        canInstantiate = false;
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax)
        {
            Debug.Log("もうこれ以上怒れないよ");
            return;
        }

        //Prefabを生成してListに追加
        int randomFontNum = Random.Range(0, 6);
        int randomSizeNum = Random.Range(0, 3);
        kentoPrefab = Instantiate(myGameObjects[randomFontNum][randomSizeNum].KentoPrefab, new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
    }

    //kentoPrefabの中身をnullに戻す
    public void ResetKentoPrefab() { kentoPrefab = null; }

    //ドラッグ終了
    public void EndDrag()
    {
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax) return;
        if (Random.Range(0, 2) == 2) timeManager.MakeAngry();
        if (kentoPrefab != null) placedGameObjects.Add(kentoPrefab.GetComponent<KentoManager>());
        kentoPrefab.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        // IsEndDrag(true);
        ResetKentoPrefab();
        StopCoroutine(TimeOver());
        Debug.Log("drag終了");
    }

    //kentoPrefabの回転
    public void PushRotateButton() { if (kentoPrefab != null) kentoPrefab.transform.Rotate(new Vector3(0, 0, 45)); }

    //置かれた検討の数
    private int putKentoCount;
    public int KentoSpeed()
    {
        putKentoCount++;
        // if (putKentoCount != 0 && putKentoCount % 10 == 0) エフェクト;

        return Mathf.FloorToInt(putKentoCount / 10) * 5 + 20;
    }
}