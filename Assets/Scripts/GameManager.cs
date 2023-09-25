using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject canvas;
    public List<GameObject> allFontGameObjects = new List<GameObject>();
    public GameObject[] myGameObjects = new GameObject[6];

    public List<GameObject> placedGameObjects = new List<GameObject>();

    public bool canInstantiate;

    void Start()
    {
        canvas = GameObject.Find("Canvas");
        PutKento();
    }

    void Update()
    {

    }

    public void PutKento()
    {
        int randomNum = Random.Range(0, 6);
        GameObject kentoPrefab = Instantiate(myGameObjects[randomNum], new Vector3(0, 300, 0) + canvas.transform.position, Quaternion.identity, canvas.transform);
        placedGameObjects.Add(kentoPrefab);
    }
}
