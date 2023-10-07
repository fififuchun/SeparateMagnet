using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//ゲームオーバー管理
public class TimeManager : MonoBehaviour
{
    //タイマー
    private float timer;

    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax = 30;
    public int AngerGaugeMax { get => angerGaugeMax; }

    //デバッグ用
    public Slider slider;
    public TextMeshProUGUI text;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5)
        {
            MakeAngry();
            timer = 0;
        }

        //デバッグ用
        text.text = timer.ToString();
        slider.value = (float)angerGauge / (float)angerGaugeMax;
    }

    public void MakeAngry()
    {
        if (angerGauge >= angerGaugeMax) return;
        angerGauge++;
        Debug.Log("今の怒り:" + angerGauge);
    }

    public void MakeCalm()
    {
        angerGaugeMax++;
    }
}
