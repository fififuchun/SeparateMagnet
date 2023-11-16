using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RPGManager : MonoBehaviour
{
    // CoinCountインスタンス
    [SerializeField] private CoinCount coinCount;

    // ステータス確認用
    private string[] rankNames = { "AngerLimit", "AngerTime", "LateAngerTime", "HoldTime", "AngerRate", "NextKentoTime", "RareRate" };

    // 保存データ数、現時点では7
    private const int levelCount = SaveData.levelCount;

    //初期数値
    private int[] initLevel = { 4, 10, 10, 5, 2, 10, 100 };

    // ステータス確認用のTextObject
    [SerializeField] private TextMeshProUGUI[] nextLevelAcquire = new TextMeshProUGUI[levelCount];

    // 参照するセーブデータ
    private SaveData data;

    //　パワーアップのデータを保存したScriptableObject
    [SerializeField] private RPGSO rpgData;

    //-------------------------------------------------------------------
    void Start()
    {
        // セーブデータをDataManagerから参照
        data = GetComponent<DataManager>().data;

        UpdateAcquireCoin();

        for (int i = 7; i < 7; i++) data.level[i] = initLevel[i];
    }

    public void UpdateAcquireCoin()
    {
        for (int i = 0; i < levelCount; i++)
        {
            nextLevelAcquire[i].text = rpgData.AcquireCoin(i, data.level[i] - initLevel[i]).ToString();
        }
    }

    public void PushPowerUpButton(int i)
    {
        // Debug.Log(i + "," + data.level[i] + "," + initLevel[i]);
        int acquireCoin = rpgData.AcquireCoin(i, data.level[i] - initLevel[i]);
        if (acquireCoin < coinCount.Coin)
        {
            coinCount.GetCoin(-acquireCoin);
            data.level[i] += (int)Mathf.Sign(9 - 2 * i);
            Debug.Log(data.level[i]);
            Debug.Log(rpgData.AcquireCoin(i, data.level[i] - initLevel[i]).ToString());
        }
        // else 
        UpdateAcquireCoin();
    }

}
