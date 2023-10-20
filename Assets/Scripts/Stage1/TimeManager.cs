using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading;

//ゲームオーバー管理
public class TimeManager : MonoBehaviour
{
    //タイマー
    private float timer;

    private float instantiateTime;

    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax = 4;
    public int AngerGaugeMax { get => angerGaugeMax; }

    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //
    [SerializeField] private GameObject result;

    //デバッグ用
    public Slider slider;
    public TextMeshProUGUI timerText;

    public float startTimer;

    public bool debug;

    public TextMeshProUGUI resultCoinText;



    void Start()
    {
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            MakeAngry();
            timer = 0;
        }

        // if (debug) timerText.text = Mathf.Ceil(12 - startTimer - timer).ToString();
        if (!debug) timerText.text = "";

        //デバッグ用じゃなくなった
        // timerText.text = timer.ToString();
        slider.value = (float)AngerGauge / (float)angerGaugeMax;

        resultCoinText.fontSize = 150 + 20 * Mathf.Sin(5 * Time.time);
    }

    public void ResetPutTimer()
    {
        instantiateTime = timer;
        Debug.Log(instantiateTime);
    }

    public void FinishGame()
    {
        result.SetActive(true);
    }

    //ここでしかangerGaugeいじってないはずです
    public void MakeAngry()
    {
        angerGauge++;
        if (AngerGauge >= AngerGaugeMax) return;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
    }

    // public IEnumerator StartTimer(){
    //     timer
    //     timerText=
    // }

    public void StartTimer()
    {
        startTimer = timer;
        Debug.Log(startTimer);
    }
}
