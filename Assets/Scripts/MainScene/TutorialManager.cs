using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    //何番目を出力するか
    [SerializeField, Header("RowNumber?")] private int matrixRowNum;

    //tutorial用のMatrixText
    [SerializeField] private MatrixText tutorial;

    //チュートリアルの女性
    [SerializeField] private Sprite[] ladySprites = new Sprite[8];
    [SerializeField] private Image ladyImage;

    //セーブデータ
    [SerializeField] private TutorialDataManager tutorialDataManager;

    //mainフッターの指のナビゲーション
    [SerializeField] private GameObject fingerNavi;


    void Start()
    {
        //isFinishedTutorial[0]がfalseなら、チュートリアルを出現させる
        if (!tutorialDataManager.data.isFinishedTutorial[0]) InstantiateTutorial(0, 3);
        //isFinishedTutorial[4]がfalseなら、MainのfingerNaviをtrueにする
        if (!tutorialDataManager.data.isFinishedTutorial[4]) fingerNavi.SetActive(true);

        shoppingTutorialButton.onClickCallback += PushShoppingTutorialButton;
        rpgTutorialButton.onClickCallback += PushRPGTutorialButton;
        missionTutorialButton.onClickCallback += PushMissionTutorialButton;
    }

    public void InstantiateTutorial(int _matrixRowNum, int _ladyNum)
    {
        //既にチュートリアルを見ているなら返す
        if (tutorialDataManager.data.isFinishedTutorial[_matrixRowNum]) return;
        else tutorialDataManager.data.isFinishedTutorial[_matrixRowNum] = true;

        tutorial.Initialize(_matrixRowNum);
        ladyImage.sprite = ladySprites[_ladyNum];
    }

    [SerializeField] private CustomButton shoppingTutorialButton;
    public void PushShoppingTutorialButton()
    {
        tutorial.Initialize(1);
        ladyImage.sprite = ladySprites[(int)TutorialImage.Cheak];
    }

    [SerializeField] private CustomButton rpgTutorialButton;
    public void PushRPGTutorialButton()
    {
        tutorial.Initialize(2);
        ladyImage.sprite = ladySprites[(int)TutorialImage.Good];
    }

    [SerializeField] private CustomButton missionTutorialButton;
    public void PushMissionTutorialButton()
    {
        tutorial.Initialize(3);
        ladyImage.sprite = ladySprites[(int)TutorialImage.Info];
    }
}
