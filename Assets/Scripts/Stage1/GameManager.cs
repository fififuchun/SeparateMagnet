using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    [SerializeField] private TimeManager timeManager;

    //canvas
    public GameObject canvas;

    //kentoPrefabの親オブジェクト
    [SerializeField] private GameObject kentos;

    //すべてのフォントをここに入れる
    // [SerializeField] private List<GameObject> allFontGameObjects = new List<GameObject>();

    // [SerializeField] private List<List<GameObject>> allObjects = new List<List<GameObject>>();

    //1ゲーム限りのallFontGameObjectsから取り出したランダムな持ちフォント
    [SerializeField] private GameObject[] myGameObjects = new GameObject[6];
    [SerializeField] private GameObject[] myGameObjects2 = new GameObject[6];
    [SerializeField] private GameObject[] myGameObjects3 = new GameObject[6];
    [SerializeField] private GameObject[] rareObjects = new GameObject[6];

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
                    // yield return new WaitUntil(() => canInstantiate);
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    // canInstantiate = false;
                    PutKento();
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
                if (phase == Phase.PutPhase) CanInstantiate();
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
            // StartCoroutine(timeManager.FinishGame(placedGameObjects.Count()));
        }

        //Prefabを生成してListに追加
        int randomNum = Random.Range(0, 6);
        kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
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
        IsEndDrag(true);
        ResetKentoPrefab();
        StopCoroutine(TimeOver());
        Debug.Log("drag終了");
    }

    //kentoPrefabの回転
    public void PushRotateButton()
    {
        if (kentoPrefab != null) kentoPrefab.transform.Rotate(new Vector3(0, 0, 45));
    }

    //置かれた検討の数
    private int putKentoCount;
    public int KentoSpeed()
    {
        putKentoCount++;
        // if (putKentoCount != 0 && putKentoCount % 10 == 0) エフェクト;

        return Mathf.FloorToInt(putKentoCount / 10) * 5 + 20;
    }
}