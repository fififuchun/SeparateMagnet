// using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //親スクリプトの取得
    private TimeManager timeManager;

    //すべてのフォントをここに入れる
    [SerializeField] private List<GameObject> allFontGameObjects = new List<GameObject>();

    [SerializeField] private List<List<GameObject>> allObjects= new List<List<GameObject>>();

    //1ゲーム限りのallFontGameObjectsから取り出したランダムな持ちフォント
    [SerializeField] private GameObject[] myGameObjects = new GameObject[6];

    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    [SerializeField] private List<KentoManager> placedGameObjects = new List<KentoManager>();

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

    //canvas
    public GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();

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
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    PutKento();
                    //今出現したkentoがドラッグ終了するまで止めとく
                    yield return new WaitUntil(() => !placedGameObjects.Last().GetComponent<KentoManager>().enabled);
                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitForSeconds(1f);
                    phase = Phase.StartPhase;
                    break;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].gameObject.transform.position.y < -1000)
            {
                Destroy(placedGameObjects[i].gameObject);
                placedGameObjects.RemoveAt(i);
                // Debug.Log(i + "/" + placedGameObjects.Count);
            }
    }

    public void PutKento()
    {
        if (timeManager.AngerGauge >= timeManager.AngerGaugeMax)
        {
            Debug.Log("もうこれ以上怒れないよ");
            StartCoroutine(timeManager.FinishGame(placedGameObjects.Count()));
            return;
        }

        //Prefabを生成してListに追加
        int randomNum = Random.Range(0, 6);
        GameObject kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, canvas.transform);
        kentoPrefab.GetComponent<RectTransform>().sizeDelta *= Mathf.Pow(2, Random.Range(-1, 2));
        placedGameObjects.Add(kentoPrefab.GetComponent<KentoManager>());

        //50%で怒らせる
        if (Random.Range(0, 2) == 0) timeManager.MakeAngry();
    }

    public void PushRotateButton()
    {
        placedGameObjects.Last().transform.Rotate(new Vector3(0, 0, 45)); //もっと良い書き方募集中
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
