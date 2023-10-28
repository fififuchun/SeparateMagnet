using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    //コインプロパティ
    private int coin;
    public int Coin { get => coin; }

    //ヘッダーのコインテキスト
    [SerializeField] private TextMeshProUGUI coinText;

    void Awake()
    {
        GetCoin(PlayerPrefs.GetInt("Coin", 0));
    }

    void Update()
    {

    }

    public void GetCoin(int i)
    {
        coin += i;
        UpdateCoin();
        PlayerPrefs.SetInt("Coin", Coin);
    }

    public void UpdateCoin()
    {
        coinText.text = Coin.ToString();
    }


}
