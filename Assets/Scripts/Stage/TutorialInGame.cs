using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class TutorialInGame : MonoBehaviour
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

    //指のナビゲーション
    [SerializeField] private GameObject fingerNavi;

    void Start()
    {
        if (!tutorialDataManager.data.isFinishedTutorial[4])
        {
            InstantiateTutorial(4, 3);
            fingerNavi.SetActive(true);
        }
    }

    public void InstantiateTutorial(int _matrixRowNum, int _ladyNum)
    {
        //既にチュートリアルを見ているなら返す
        if (tutorialDataManager.data.isFinishedTutorial[_matrixRowNum]) return;
        else tutorialDataManager.data.isFinishedTutorial[_matrixRowNum] = true;

        tutorial.Initialize(_matrixRowNum);
        ladyImage.sprite = ladySprites[_ladyNum];
    }
}
