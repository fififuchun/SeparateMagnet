using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Uidirector : MonoBehaviour
{
    //コイン・ダイアモンド計数スクリプトのインスタンス化
    private CoinCount coinCount;
    private DiamondCount diamondCount;
    private RankManager rankManager;

    void Start()
    {
        coinCount = GameObject.Find("UiDirector").GetComponent<CoinCount>();
        diamondCount = GameObject.Find("UiDirector").GetComponent<DiamondCount>();
        rankManager = GameObject.Find("UiDirector").GetComponent<RankManager>();

        GetTax();
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

    public void DebugButton()
    {
        coinCount.GetCoin(100);
        diamondCount.GetDiamond(100);
    }

    public void GetTax()
    {
        int stageCoin = PlayerPrefs.GetInt("TmpCoin", 0);
        coinCount.GetCoin(stageCoin);
        rankManager.GetCoinSum(stageCoin);
        PlayerPrefs.SetInt("TmpCoin", 0);

        PlayerPrefs.SetInt("Coin", coinCount.Coin);
    }

    public void AllResetButton()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StageScene");
    }
}
