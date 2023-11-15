using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class RPGManager : MonoBehaviour
{
    // ランキング名
    private string[] rankNames = { "AngerLimit", "AngerTime", "LateAngerTime", "HoldTime", "AngerRate", "NextKentoTime", "RareRate" };

    // ランキング数
    private const int levelCount = SaveData.levelCount;

    // ランキングのテキスト
    [SerializeField] private TextMeshProUGUI[] nextLevelAcquire = new TextMeshProUGUI[levelCount];

    // 参照するセーブデータ
    private SaveData data;

    [SerializeField] private RPGSO rpgData;

    //-------------------------------------------------------------------
    void Start()
    {
        // セーブデータをDataManagerから参照
        data = GetComponent<DataManager>().data;

        //
    }

    public void PushPowerUpAngerLimit()
    {

    }
}
