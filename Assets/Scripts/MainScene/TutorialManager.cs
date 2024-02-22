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
    //描画キャンバス・生成用Prefab
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject tutorial;
    
    //チュートリアルの女性の画像
    [SerializeField] private Sprite[] tutorialSprites = new Sprite[8];
    [SerializeField] private Sprite speechBubbleImage;
    
    //状況ごとのチュートリアルメッセージ
    [SerializeField] private TutorialSO tutorialSO;

    //動的入力
    [SerializeField] private TextMeshProUGUI tutorialTextPrefab;
    [SerializeField] private GameObject tutorialPrefab;



    void Start()
    {
        InstantiateTutorial(3, 0);
    }


    /// <summary>
    /// spriteIndex番目の画像の女性がstringIndex番目のstringGroupのチュートリアルをするGameObjectを生成
    /// </summary>
    /// <param name="spriteIndex"></param>
    /// <param name="stringIndex"></param>
    public void InstantiateTutorial(int spriteIndex, int stringIndex)
    {
        tutorialPrefab = Instantiate(tutorial, canvas.transform);

        if (tutorialPrefab.transform.GetChild(0).gameObject == null) return;
        GameObject secretaryObj = tutorialPrefab.transform.GetChild(0).gameObject;

        if (secretaryObj.transform.GetChild(0).GetComponent<Image>() != null)
            secretaryObj.transform.GetChild(0).GetComponent<Image>().sprite = speechBubbleImage;

        if (tutorialPrefab.transform.GetChild(1).GetComponent<Button>() != null)
            tutorialPrefab.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => PushTutorial(stringIndex));

        if (secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>() != null || tutorialSO.testStringGroups.Count() <= stringIndex || stringIndex < 0)
        {
            tutorialTextPrefab = secretaryObj.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            tutorialTextPrefab.text = tutorialSO.testStringGroups[stringIndex].strings[0];
        }
    }

    //----------------

    //チュートリアルを次に進める
    public void PushTutorial(int stringIndex)
    {
        if (tutorialSO.testStringGroups.Count() <= stringIndex || stringIndex < 0)
        {
            Debug.Log("指定されたチュートリアルは用意されていません");
            return;
        }

        for (int i = 0; i < tutorialSO.testStringGroups[stringIndex].strings.Length; i++)
        {
            if (i == tutorialSO.testStringGroups[stringIndex].strings.Length - 1) Destroy(tutorialPrefab);
            tutorialTextPrefab.text = tutorialSO.testStringGroups[stringIndex].strings[i];
        }
    }
}
