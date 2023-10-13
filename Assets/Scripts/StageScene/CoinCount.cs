using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCount : MonoBehaviour
{
    //コインプロパティ
    private int coin = 0;
    public int Coin { get => coin; }

    //ヘッダーのコインテキスト
    [SerializeField] private TextMeshProUGUI coinText;

    void Start()
    {

    }

    void Update()
    {

    }

    public void GetCoin(int i)
    {
        coin += i;
        PlayerPrefs.SetInt("Coin", coin);
    }

    public void UpdateCoin()
    {
        coinText.text = Coin.ToString();
    }
}
