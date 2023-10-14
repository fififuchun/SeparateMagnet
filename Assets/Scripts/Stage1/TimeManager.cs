using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

//ゲームオーバー管理
public class TimeManager : MonoBehaviour
{
    //タイマー
    private float timer;

    //怒りゲージ
    private int angerGauge;
    public int AngerGauge { get => angerGauge; }

    //怒りの最大値
    private int angerGaugeMax = 5;
    public int AngerGaugeMax { get => angerGaugeMax; }

    //怒りイメージ配列・初期は[2]から実装
    [SerializeField] private Sprite[] angryImages = new Sprite[7];
    [SerializeField] private Image angryImage;

    //デバッグ用
    public Slider slider;
    public TextMeshProUGUI text;

    void Start()
    {

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            MakeAngry();
            timer = 0;
        }

        //デバッグ用
        text.text = timer.ToString();
        slider.value = (float)AngerGauge / (float)angerGaugeMax;
    }

    public IEnumerator FinishGame(int kentoCount)
    {
        //検討が降らなくなってからしばらく待つ
        yield return new WaitForSeconds(10f);
        Debug.Log("result");
    }

    //ここでしかangerGaugeいじってないはずです
    public void MakeAngry()
    {
        angerGauge++;
        if (AngerGauge >= AngerGaugeMax - 1) return;
        angryImage.sprite = angryImages[8 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
    }

    // public void MakeCalm()
    // {
    //     angerGaugeMax++;
    // }
}
