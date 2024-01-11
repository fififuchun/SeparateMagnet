using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ChangeScene : MonoBehaviour
{
    //インスタンス
    // [SerializeField] private RankManager rankManager;

    //ステージ選択のScrollView
    // [SerializeField] private GameObject mainViewContent;

    //「検討を重ねる」のテキスト
    // [SerializeField] private TextMeshProUGUI repeatKentoText;


    //ステージ遷移
    // public void PushRepeatKentoButton()
    // {
    //     int stageNum = (int)(1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830));
    //     PlayerPrefs.Save();

    //     Debug.Log(stageNum);

    //     if (rankManager.Rank < (stageNum - 1) * 5)
    //     {
    //         repeatKentoText.text = $"ランク{(stageNum - 1) * 5}にしてね";
    //         return;
    //     }
    //     SceneManager.LoadScene($"Stage_{1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830)}");
    // }
}
