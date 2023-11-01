using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
// using Unity.VisualScripting;
// using System;
// using System.Threading;

//ゲームオーバー管理
public class TimeManager : MonoBehaviour
{
    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax = 4;//Getset
    public int AngerGaugeMax { get => angerGaugeMax; }

    //angryTime秒で国民が1怒る
    private int angryTime;

    //ゲーム終了時の最終コイン
    public int sumCoin;

    //ドラッグ終了したらtrue
    public bool isEndDrag;
    public void IsEndDrag(bool judge) { isEndDrag = judge; }

    //検討の持ち時間
    private int canHoldTime = 10;

    [Header("以下はアタッチが必要")]
    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //Header
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultCoinText;
    [SerializeField] private TextMeshProUGUI finishText;

    //結果表示
    [SerializeField] private GameObject resultObject;


    //関数の部
    void Start()
    {
        angryTime = 7;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        InvokeRepeating("MakeAngry", angryTime, angryTime);
    }

    void Update()
    {
        slider.value = (float)AngerGauge / (float)angerGaugeMax;
        resultCoinText.fontSize = 150 + 20 * Mathf.Sin(5 * Time.time);
    }

    //angerGauge操作はここだけ
    public void MakeAngry()
    {
        angerGauge++;
        if (isAnger()) return;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
    }

    //gameManagerで使う用
    public bool isAnger()
    {
        if (AngerGauge >= AngerGaugeMax) return true;
        else return false;
    }

    //timerTextを空にする
    public void EmptyTimerText() { timerText.text = ""; }

    //時間切れ
    public IEnumerator TimeOver()
    {
        float startTimer = Time.time;
        for (int i = 0; i < canHoldTime; i++)
        {
            timerText.text = Mathf.Ceil(canHoldTime + startTimer - Time.time).ToString();
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("TIME OVER");
        EmptyTimerText();
        IsEndDrag(true);
        yield break;
    }

    public IEnumerator FinishGame()
    {
        // Debug.Log("ゲーム終了が起動");
        float startTimer = Time.time;
        canHoldTime = 5;
        timerText.color = Color.red;
        finishText.enabled = true;

        for (int i = 0; i < canHoldTime; i++)
        {
            timerText.text = Mathf.Ceil(canHoldTime + startTimer - Time.time).ToString();
            yield return new WaitForSeconds(1f);
        }

        resultObject.SetActive(true);
        resultCoinText.text = sumCoin.ToString();
        PlayerPrefs.SetInt("TmpCoin", sumCoin);
        yield break;
    }
}
