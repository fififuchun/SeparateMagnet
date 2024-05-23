using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FuchunLibrary;

public class RPGManager : MonoBehaviour
{
    // インスタンス
    // [SerializeField] private MissionDataManager dataManager;
    [SerializeField] private Mission mission;

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
        UpdateAcquireCoin();

        dataManager.ChangeMissionValue(5, Library.FirstFalseIndex(dataManager.data.isRareFonts));
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
        if (acquireCoin <= coinCount.Coin)
        {
            coinCount.GetCoin(-acquireCoin);
            dataManager.data.level[i]++;

            //ミッションのクリア状況を即位反映
            dataManager.ChangeMissionValue(1, dataManager.data.level[1]);
            dataManager.ChangeMissionValue(2, dataManager.data.level[4]);
            dataManager.ChangeMissionValue(3, dataManager.data.level[0]);
        }
        else
        {
            Debug.Log("コインがたりないよ");
        }

        UpdateAcquireCoin();
    }

}
