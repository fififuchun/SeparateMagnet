using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum TutorialImage
{
    Bow = 0,
    Cheak = 1,
    Glad = 2,
    Go = 3,
    Good = 4,
    Info = 5,
    Tele = 6,
    Wink = 7,
}

public class TutorialManager : MonoBehaviour
{
    //
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tutorial;

    //チュートリアルの女性の画像
    [SerializeField] private Sprite[] tutorialSprites = new Sprite[8];

    //
    // []

    //チュートリアルを生成
    public void InstantiateTutorial(int spriteIndex, int stringIndex)
    {
        GameObject tutorialPrefab = Instantiate(tutorial, canvas.transform);

        if (tutorialPrefab.transform.GetChild(0).gameObject == null) return;
        GameObject secretaryObj = tutorialPrefab.transform.GetChild(0).gameObject;

        if (secretaryObj.transform.GetChild(0).GetComponent<Image>() != null)
            secretaryObj.transform.GetChild(0).GetComponent<Image>().sprite = tutorialSprites[spriteIndex];

        if (secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>() != null || stringGroups.Count() <= stringIndex || stringIndex < 0)
            secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = stringGroups[stringIndex][0];
    }

    //----------------
    //状況ごとのチュートリアルメッセージ
    [SerializeField] private List<string[]> stringGroups = new List<string[]>();

    //チュートリアルを次に進める
    public void PushTutorial(int stringIndex)
    {
        if (stringGroups.Count() <= stringIndex || stringIndex < 0)
        {
            Debug.Log("指定されたチュートリアルは用意されていません");
            return;
        }

        for (int i = 0; i < stringGroups[stringIndex].Length; i++)
        {

        }
    }
}
