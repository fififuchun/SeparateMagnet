using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialInGame : MonoBehaviour
{
    
    //チュートリアルの女性
    [SerializeField] private Image ladyImage;
    [SerializeField] private Sprite ladyImage_go;

    //tutorial用のMatrixText
    [SerializeField] private MatrixText tutorial;

    //指のナビゲーション
    [SerializeField] private GameObject fingerNavi;

    void Start()
    {
        // if (!TutorialDataManager.data.isFinishedTutorial[4])
        // {
            InstantiateTutorial(4, 3);
            fingerNavi.SetActive(true);
        // }
    }

    public void InstantiateTutorial(int _matrixRowNum, int _ladyNum)
    {
        //既にチュートリアルを見ているなら返す
        if (TutorialDataManager.data.isFinishedTutorial[_matrixRowNum]) return;
        else TutorialDataManager.data.isFinishedTutorial[_matrixRowNum] = true;

        tutorial.Initialize(_matrixRowNum);
        // ladyImage.sprite = TutorialManager.ladySprites[_ladyNum];
        ladyImage.sprite= ladyImage_go;
    }
}
