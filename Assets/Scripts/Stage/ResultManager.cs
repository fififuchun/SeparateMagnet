using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ResultManager : MonoBehaviour
{
    //
    [SerializeField] private GameObject resultImage;

    //
    private int stageNum;

    //
    [SerializeField] ParticleSystem[] fireWorks = new ParticleSystem[2];

    void Start()
    {
        InitializeResult();

        stageNum = int.Parse(SceneManager.GetActiveScene().name.Split('_')[1]);
        // Debug.Log(stageNum);
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
