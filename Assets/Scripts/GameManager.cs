using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //canvas
    public GameObject canvas;

    //すべてのフォントをここに入れる
    public List<GameObject> allFontGameObjects = new List<GameObject>();

    //1ゲーム限りのallFontGameObjectsから取り出したランダムな持ちフォント
    public GameObject[] myGameObjects = new GameObject[6];

    //置かれたゲームオブジェクトを追加
    public List<GameObject> placedGameObjects = new List<GameObject>();

    public bool canInstantiate;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        PutKento();
    }

    void Update()
    {
        for (int i = 0; i < placedGameObjects.Count; i++) if (placedGameObjects[i].transform.position.y < -1000)
            {
                // placedGameObjects.
        }
    }

    public void PutKento()
    {
        int randomNum = Random.Range(0, 6);
        GameObject kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 300, 0) + canvas.transform.position, Quaternion.identity, canvas.transform);
        placedGameObjects.Add(kentoPrefab);
    }

    public void PushRotateButton()
    {
        placedGameObjects.Last().transform.Rotate(new Vector3(0, 0, 45));
    }
}
