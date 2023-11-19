using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RPGManager : MonoBehaviour
{
    // インスタンス
    [SerializeField] private CoinCount coinCount;
    [SerializeField] private DataManager dataManager;

    // ステータス確認用
    // private string[] rankNames = { "AngerLimit", "AngerTime", "LateAngerTime", "HoldTime", "AngerRate", "NextKentoTime", "RareRate" };

    // 保存データ数、現時点では7
    private const int levelCount = SaveData.levelCount;

    // ステータス確認用のTextObject
    [SerializeField] private TextMeshProUGUI[] nextLevelAcquire = new TextMeshProUGUI[levelCount];

    //　パワーアップのデータを保存したScriptableObject
    [SerializeField] private RPGSO rpgData;


    //関数
    void Start()
    {
        for (int i = 0; i < 7; i++) dataManager.data.level[i] = dataManager.InitLevel[i];
        UpdateAcquireCoin();
    }

    public void UpdateAcquireCoin()
    {
        for (int i = 0; i < levelCount; i++)
        {
            if (dataManager.data.level[i] == dataManager.MaxLevel[i])
            {
                nextLevelAcquire[i].color = Color.red;
                nextLevelAcquire[i].text = "Max";
                continue;
            }
            nextLevelAcquire[i].color = Color.black;
            nextLevelAcquire[i].text = rpgData.AcquireCoin(i, dataManager.data.level[i]).ToString();
        }
        dataManager.Save(dataManager.data);
    }

    public void PushPowerUpButton(int i)
    {
        if (dataManager.data.level[i] >= dataManager.MaxLevel[i]) return;

        int acquireCoin = rpgData.AcquireCoin(i, dataManager.data.level[i]);
        if (acquireCoin < coinCount.Coin)
        {
            coinCount.GetCoin(-acquireCoin);
            dataManager.data.level[i]++;
        }
        else {
            Debug.Log("コインがたりないよ");
        }

        UpdateAcquireCoin();
    }

}
