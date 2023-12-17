using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RankManager : MonoBehaviour
{
    //rank i-1からrank iになるためにはexpTable[i-1]が必要（rank 1からrank 2になるためにはexpTable[2-1]、0 + 100が必要）
    private int[] expTable = new int[40] { 0,     100,   200,   300,    400,    500,    600,    700,    800,    900,
                                           1000,  1000,  1000,  1000,   1500,   2000,   2000,   2000,   2500,   2500,
                                           5000,  7500,  10000, 12500,  15000,  19000,  20000,  25000,  30000,  35000,
                                           50000, 50000, 75000, 100000, 125000, 150000, 200000, 250000, 300000, 500000 };

    //rank iを達成するためにはcoinSumがexpTotalTable[i-1]だけ必要
    private int[] expTotalTable = new int[40] { 0,      100,    300,    600,    1000,   1500,   2100,   2800,    3600,    4500,
                                                5500,   6500,   7500,   8500,   10000,  12000,  14000,  16000,   18500,   21000,
                                                26000,  33500,  43500,  56000,  71000,  90000,  110000, 135000,  165000,  200000,
                                                250000, 300000, 375000, 475000, 600000, 750000, 950000, 1200000, 1500000, 2000000 };

    //ランク、Max40
    private int rank;

    //集めたコインの総計
    private int coinSum;

    //経験値のバー
    [SerializeField] private Slider expSlider;

    //rankのtext
    [SerializeField] private TextMeshProUGUI rankText;


    //関数
    void Awake()
    {
        GetCoinSum(PlayerPrefs.GetInt("Sum", PlayerPrefs.GetInt("Coin", 0)));
    }

    public void UpdateExp()
    {
        for (int i = 0; i < expTotalTable.Length; i++) if (coinSum < expTotalTable[i])
            {
                rank = i;
                break;
            }
        // Debug.Log("rank: " + rank);
        expSlider.value = (float)(coinSum - expTotalTable[rank - 1]) / (float)expTable[rank];
        rankText.text = rank.ToString();
    }

    public void GetCoinSum(int coin)
    {
        coinSum += coin;
        UpdateExp();
        PlayerPrefs.SetInt("Sum", coinSum);
    }

    public void PushTestGetExpButton()
    {
        GetCoinSum(50);
    }
}
