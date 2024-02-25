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

    [SerializeField] private Button testButton;
    public void PushTestButton(int _matrixRowNum) { InstantiateTutorial(_matrixRowNum, 3); }

    void Start()
    {
        InstantiateTutorial(matrixRowNum, 3);
    }

    public void InstantiateTutorial(int _matrixRowNum, int _ladyNum)
    {
        tutorial.Initialize(_matrixRowNum);
        ladyImage.sprite = ladySprites[_ladyNum];
    }
}
