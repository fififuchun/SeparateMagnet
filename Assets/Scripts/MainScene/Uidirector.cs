using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Uidirector : MonoBehaviour
{
    //コイン・ダイアモンド計数スクリプトのインスタンス化
    [SerializeField] private CoinCount coinCount;
    [SerializeField] private DiamondCount diamondCount;
    [SerializeField] private RankManager rankManager;

    void Start()
    {
        // GetTax();
        UpdateHeader();
    }

    public void UpdateHeader()
    {
        coinCount.UpdateCoin();
        diamondCount.UpdateDiamond();
        rankManager.UpdateExp();
    }

    //iは倍率(1,3,5)
    public void BuyCoinDiamond(int i)
    {
        if (diamondCount.Diamond < i * 10)
        {
            Debug.Log("ダイヤが足りないよ");
            return;
        }
        coinCount.GetCoin(i * 100 + (i - 1) * 25);
        diamondCount.GetDiamond(-i * 10);
        UpdateHeader();
    }

    public void GetTax()
    {


        // int stageCoin = PlayerPrefs.GetInt("TmpCoin", 0);
        // coinCount.GetCoin(stageCoin);
        // rankManager.GetSumCoin(stageCoin);
        // PlayerPrefs.SetInt("TmpCoin", 0);

        // PlayerPrefs.SetInt("Coin", coinCount.Coin);
    }
}
