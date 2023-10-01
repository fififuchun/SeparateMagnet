using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //すべてのフォントをここに入れる
    [SerializeField] private List<GameObject> allFontGameObjects = new List<GameObject>();

    //1ゲーム限りのallFontGameObjectsから取り出したランダムな持ちフォント
    [SerializeField] private GameObject[] myGameObjects = new GameObject[6];

    //置かれたゲームオブジェクトのKentoManagerインスタンスを格納
    [SerializeField] private List<KentoManager> placedGameObjects = new List<KentoManager>();

    //phase管理
    Phase phase;
    enum Phase
    {
        //一応、一度きり
        AwakePhase,
        StartPhase,

        //次の検討の出現、一度きり
        AppearPhase,

        //落ちてる状態、いらない？
        PutPhase,

        //スピードの判定、スピードが落ち着くまで
        // JudgePhase,
    }

    //canvas
    public GameObject canvas;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        while (true)
        {
            Debug.Log(phase);
            switch (phase)
            {
                case Phase.AwakePhase:
                    PutKento();
                    phase = Phase.AppearPhase;
                    break;
                case Phase.StartPhase:
                    yield return new WaitForSeconds(1f);
                    phase = Phase.AppearPhase;
                    break;
                case Phase.AppearPhase:
                    yield return new WaitUntil(() => !placedGameObjects.Last().GetComponent<KentoManager>().enabled);
                    PutKento();
                    phase = Phase.PutPhase;
                    break;
                case Phase.PutPhase:
                    yield return new WaitForSeconds(1f);
                    phase = Phase.StartPhase;
                    break;
                // case Phase.JudgePhase:
                //     // yield return new WaitForSeconds(1f);
                //     phase = Phase.StartPhase;
                //     break;
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i] == null) placedGameObjects.RemoveAll(item => item == null);
    }

    public void PutKento()
    {
        int randomNum = Random.Range(0, 6);
        GameObject kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 600, 0) + canvas.transform.position, Quaternion.identity, canvas.transform);
        kentoPrefab.GetComponent<RectTransform>().sizeDelta *= Mathf.Pow(2, Random.Range(-1, 2));
        placedGameObjects.Add(kentoPrefab.GetComponent<KentoManager>());
    }

    // public bool CanInstantiate(KentoManager kento)
    // {
    //     if (kento.KentoSpeed > 1) return false;
    //     return true;
    // }

    public void PushRotateButton()
    {

        placedGameObjects.Last().transform.Rotate(new Vector3(0, 0, 45));
    }
}
