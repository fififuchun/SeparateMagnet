using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using System;

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


    void Start()
    {
        InitializeResult();
        stageNum = MainManager.stageNum;

        Instantiate(floorObjects[MainManager.stageNum]);
    }

    public void InitializeResult()
    {
        resultImage.transform.parent.gameObject.SetActive(false);
        resultImage.transform.GetChild(1).gameObject.SetActive(false);
        resultImage.transform.GetChild(2).gameObject.SetActive(false);
        resultImage.transform.GetChild(3).gameObject.SetActive(false);
        resultImage.transform.GetChild(4).gameObject.SetActive(false);
    }

    public async UniTask AppearResult(int sumCoin)
    {
        await UniTask.Delay(500);
        Debug.Log("Result");
        resultImage.transform.parent.gameObject.SetActive(true);

        await UniTask.Delay(1000);
        resultImage.transform.GetChild(1).gameObject.SetActive(true);
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
