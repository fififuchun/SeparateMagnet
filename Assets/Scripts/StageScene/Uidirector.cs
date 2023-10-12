using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Uidirector : MonoBehaviour
{
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

        // coinCount.GetCoin(100);
        // UpdateHeader();
    }

    void Update()
    {

    }

    public void UpdateHeader()
    {
        coinCount.UpdateCoin();
        diamondCount.UpdateDiamond();
    }
}
