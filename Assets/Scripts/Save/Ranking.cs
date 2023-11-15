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
        data = GetComponent<DataManager>().data;// セーブデータをDataManagerから参照

        // for (int i = 0; i < rankCnt; i++)
        // {
        //     // Transform rankChilds = GameObject.Find("RankTexts").transform.GetChild(i);
        //     // rankTexts[i] = GameObject.Find("RankTexts").transform.GetChild(i).GetComponent<TextMeshProUGUI>();
        // }
        Debug.Log(data.level[0]);

        // if(data)
    }

    //-------------------------------------------------------------------
    // void Update()
    // {
    //     DispRank();
    // }

    // ランキング表示
    // void DispRank()
    // {
    //     for (int i = 0; i < rankCnt; i++)
    //     {
    //         // rankTexts[i].text = rankNames[i] + " : " + data.rank[i];
    //     }
    // }

    public void PushPowerUpAngerLimit(){

    }

    // public TMP_InputField inpFld;

    // ランキング保存
    // public void SetRank()
    // {
    //     // inpFld = GameObject.Find("InputField").GetComponent<InputField>();
    //     int score = int.Parse(inpFld.text);     // string -> int

    //     // スコアがランキング内の値よりも大きいときは入れ替え
    //     for (int i = 0; i < rankCnt; i++)
    //     {
    //         if (score > data.rank[i])
    //         {
    //             var rep = data.rank[i];
    //             data.rank[i] = score;
    //             score = rep;
    //         }
    //     }
    // }

    // ランクデータの削除
    // public void DelRank()
    // {
    //     for (int i = 0; i < rankCnt; i++)
    //     {
    //         data.rank[i] = 0;
    //     }
    // }
}
