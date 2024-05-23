using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    //dataManagerインスタンス
    [SerializeField] private DataManager dataManager;

    //コインプロパティ
    // private int coin;
    public int Coin { get => dataManager.data.coin; }

    //ヘッダーのコインテキスト
    [SerializeField] private TextMeshProUGUI coinText;


    public void GetCoin(int i)
    {
        dataManager.data.coin += i;
        UpdateCoin();

        dataManager.Save();
    }

    public void UpdateCoin()
    {
        coinText.text = Coin.ToString();
    }
}
