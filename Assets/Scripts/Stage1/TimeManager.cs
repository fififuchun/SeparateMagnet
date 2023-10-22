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

    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax = 4;
    public int AngerGaugeMax { get => angerGaugeMax; }

    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //Header
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultCoinText;

    //結果表示
    [SerializeField] private GameObject resultObject;
    public void FinishGame() { resultObject.SetActive(true); }

    //ドラッグ終了したらtrue
    public bool isEndDrag;
    public void IsEndDrag(bool judge) { isEndDrag = judge; }



    void Start()
    {
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 15)
        {
            MakeAngry();
            timer = 0;
        }
        
        //デバッグ用じゃなくなった
        slider.value = (float)AngerGauge / (float)angerGaugeMax;

        resultCoinText.fontSize = 150 + 20 * Mathf.Sin(5 * Time.time);
    }

    //angerGauge操作はここだけ
    public void MakeAngry()
    {
        angerGauge++;
        if (AngerGauge >= AngerGaugeMax) return;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
    }

    public IEnumerator TimeOver()
    {
        float startTimer = Time.time;
        Debug.Log("ドラッグ待ちだよ");
        for (int i = 0; i < 10; i++)
        {
            timerText.text = Mathf.Ceil(10 + startTimer - Time.time).ToString();
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("TIME OVER");
        timerText.text = "";
        if (AngerGauge >= AngerGaugeMax) FinishGame();
        IsEndDrag(true);
        yield break;
    }
}
