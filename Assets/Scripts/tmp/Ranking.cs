using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ranking : MonoBehaviour
{
    // 値
    // ランキング名
    string[] rankNames = { "AngerLimit", "AngerTime", "LateAngerTime", "HoldTime", "AngerRate", "NextKentoTime", "RareRate" };

    // ランキング数
    const int levelCount = SaveData.levelCount;

    // コンポーネント取得用
    // ランキングのテキスト
    [SerializeField] private TextMeshProUGUI[] rankTexts = new TextMeshProUGUI[levelCount];

    // 参照するセーブデータ
    SaveData data;

    //-------------------------------------------------------------------
    void Start()
    {
        data = GetComponent<DataManager>().data;

        Debug.Log(data.level[0]);
    }

    public void PushPowerUpAngerLimit(){

    }
}
