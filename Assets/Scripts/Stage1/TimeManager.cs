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

    //デバッグ用
    public Slider slider;
    public TextMeshProUGUI text;

    void Start()
    {
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 20)
        {
            MakeAngry();
            timer = 0;
        }

        //デバッグ用じゃなくなった
        text.text = timer.ToString();
        slider.value = (float)AngerGauge / (float)angerGaugeMax;
    }

    public void ResetPutTimer()
    {
        instantiateTime = timer;
        Debug.Log(instantiateTime);
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
        if (AngerGauge >= AngerGaugeMax) return;
        angryImage.sprite = angryImages[7 - AngerGaugeMax + AngerGauge];
        Debug.Log("今の怒り:" + AngerGauge);
    }
}
