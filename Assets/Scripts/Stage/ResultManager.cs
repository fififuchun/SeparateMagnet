using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.Events;

public class ResultManager : MonoBehaviour
{
    //リザルト画面のゲームオブジェクト
    [SerializeField] private GameObject resultImage;

    //ステージ番号をMainManagerのstatic変数から動的に取得
    [SerializeField] private int stageNum;

    //リザルト画面の2種類の花火
    [SerializeField] ParticleSystem[] fireWorks = new ParticleSystem[2];

    //床のゲームオブジェクト
    [SerializeField] private GameObject[] floorObjects;

    //Floorの親オブジェクト
    [SerializeField] private GameObject floorObj;

    //床のオブジェクト
    [SerializeField] private GameObject floorObject;


    void Start()
    {
        InitializeResult();
        stageNum = MainManager.stageNum;

        if (stageNum > 0) floorObject = Instantiate(floorObjects[stageNum - 1], floorObj.transform);
        else floorObject = Instantiate(floorObjects[0], floorObj.transform);
    }

    void Update()
    {
        // countSumCoin?.Invoke();
    }

    public void InitializeResult()
    {
        resultImage.transform.parent.gameObject.SetActive(false);
        resultImage.transform.GetChild(1).gameObject.SetActive(false);
        resultImage.transform.GetChild(2).gameObject.SetActive(false);
        resultImage.transform.GetChild(3).gameObject.SetActive(false);
        resultImage.transform.GetChild(4).gameObject.SetActive(false);
    }

    public UnityEvent countSumCoin;

    public async UniTask AppearResult(int sumCoin)
    {
        await UniTask.Delay(500);
        Debug.Log("Result");
        resultImage.transform.parent.gameObject.SetActive(true);

        await UniTask.Delay(1000);
        resultImage.transform.GetChild(1).gameObject.SetActive(true);
        countSumCoin?.Invoke();
        resultImage.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{sumCoin}";

        await UniTask.Delay(1000);
        resultImage.transform.GetChild(2).gameObject.SetActive(true);
        resultImage.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"x{stageNum}";
        resultImage.transform.GetChild(3).gameObject.SetActive(true);
        fireWorks[0].gameObject.SetActive(true);
        fireWorks[1].gameObject.SetActive(true);

        await UniTask.Delay(1000);
        resultImage.transform.GetChild(4).gameObject.SetActive(true);
        resultImage.transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{sumCoin * stageNum}";
    }
}
