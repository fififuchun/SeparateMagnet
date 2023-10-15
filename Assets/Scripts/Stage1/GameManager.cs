using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    public TimeManager timeManager;

    //すべてのフォントをここに入れる
    // [SerializeField] private List<GameObject> allFontGameObjects = new List<GameObject>();

    // [SerializeField] private List<List<GameObject>> allObjects = new List<List<GameObject>>();

    //1ゲーム限りのallFontGameObjectsから取り出したランダムな持ちフォント
    [SerializeField] private GameObject[] myGameObjects = new GameObject[6];

    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    public List<KentoManager> placedGameObjects = new List<KentoManager>();

    //kentoPrefabの親オブジェクト
    private GameObject kentos;

    //生成可能か、着地したらtrue
    public bool canInstantiate;
    public void CanInstantiate() { canInstantiate = true; }

    //canvas
    private GameObject canvas;
    public GameObject Canvas { get => canvas; }

    //ドラッグ終了したらtrue
    public bool isEndDrag;
    public void IsEndDrag(bool judge) { isEndDrag = judge; }


    //関数の部
    //phase管理
    Phase phase;
    enum Phase
    {
        //一応、一度きり
        StartPhase,

        //次の検討の出現、一度きり
        AppearPhase,

        //落ちてる状態、いらない？
        PutPhase,
    }

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        kentos = GameObject.Find("KentoObjects");

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
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    PutKento();

                    //10秒待って動きなしならドラック終了とみなす
                    StartCoroutine(TimeOver());
                    //ドラッグ終了まで待つ、動きがあったらTimeOver()は止める
                    yield return new WaitUntil(() => isEndDrag);
                    StopCoroutine(TimeOver());

                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitUntil(() => canInstantiate);
                    phase = Phase.StartPhase;
                    break;
            }
        }
    }

    //さすがに書き直し
    IEnumerator TimeOver()
    {
        yield return new WaitForSeconds(5f);
        EndDrag();
        // IsEndDrag(true); //5秒待って動きなしならこの先を実行、まずはドラッグ終了とみなし、Phaseを進める
        // if (kentoPrefab == null) yield break; //？？
        // if (Random.Range(0, 2) == 2) timeManager.MakeAngry();
        // placedGameObjects.Add(kentoPrefab.GetComponent<KentoManager>());
        // kentoPrefab.GetComponent<KentoManager>().enabled = false;
        // kentoPrefab.gameObject.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        // CanInstantiate();
        // ResetKentoPrefab();
        // Debug.Log("時間切れ");
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].gameObject.transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                placedGameObjects.RemoveAt(i);
                CanInstantiate();
            }
    }

    [SerializeField] private GameObject kentoPrefab; //現在落下準備中のkento
    public void PutKento()
    {
        canInstantiate = false;
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax)
        {
            Debug.Log("もうこれ以上怒れないよ");
            // StartCoroutine(timeManager.FinishGame(placedGameObjects.Count()));
        }

        //Prefabを生成してListに追加
        int randomNum = Random.Range(0, 6);
        kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, kentos.transform);
    }
    public void ResetKentoPrefab() { kentoPrefab = null; }

    //ドラッグ終了
    public void EndDrag()
    {
        if (Random.Range(0, 2) == 2) timeManager.MakeAngry();
        if (kentoPrefab != null) placedGameObjects.Add(kentoPrefab.GetComponent<KentoManager>());
        kentoPrefab.GetComponent<Rigidbody2D>().gravityScale = KentoSpeed();
        IsEndDrag(true);
        ResetKentoPrefab();
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

        int kentoSpeed = Mathf.FloorToInt(putKentoCount / 10) * 5 + 20;
        return kentoSpeed;
    }
}