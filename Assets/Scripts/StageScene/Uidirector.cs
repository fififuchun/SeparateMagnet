using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Uidirector : MonoBehaviour
{
    //自身のインスタンス化
    // public static Uidirector instance;
    // public void Awake() { instance = this; }

    //コイン・ダイアモンド計数スクリプトのインスタンス化
    private CoinCount coinCount;
    private DiamondCount diamondCount;

    float maxExp = 100;//最大の経験値量（暫定レベルアップ時リセット）
    float currentExp = 50;//今の経験値量
    public Slider slider;

    void Start()
    {
        slider.value = currentExp / maxExp;
        Debug.Log(slider.value);

        coinCount = GameObject.Find("UiDirector").GetComponent<CoinCount>();
        diamondCount = GameObject.Find("UiDirector").GetComponent<DiamondCount>();

        int stageCoin = PlayerPrefs.GetInt("TmpCoin", 0);
        Debug.Log("TmpCoin: " + stageCoin);
        coinCount.GetCoin(stageCoin);
        PlayerPrefs.SetInt("TmpCoin", 0);

        // coinCount.GetCoin(100);
        // diamondCount.GetDiamond(100);
        UpdateHeader();
    }

    void Update()
    {

    }

    public void UpdateHeader()
    {
        coinCount.UpdateCoin();
        diamondCount.UpdateDiamond();
    }

    //iは倍率(1,3,5)
    public void BuyCoinDiamond(int i)
    {
        if (diamondCount.Diamond < i * 10)
        {
            Debug.Log("ダイヤが足りないよ");
            return;
        }
        coinCount.GetCoin(i * 100);
        diamondCount.GetDiamond(-i * 10);
        UpdateHeader();
    }

    public void DebugButton()
    {
        coinCount.GetCoin(100);
        diamondCount.GetDiamond(100);
    }
}
