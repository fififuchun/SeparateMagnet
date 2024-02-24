using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    [SerializeField] private GameObject tutorialPrefab;

    //何番目を出力するか
    [SerializeField, Header("ColumnNumber?")] private int matrixColumnNum;


    void Start()
    {
        InstantiateTutorial(matrixColumnNum);
    }

    /// <summary>
    /// tutorialを生成
    /// </summary>
    /// <param name="_matrixRowNum">行</param>
    public void InstantiateTutorial(int _matrixRowNum)
    {
        //tutorialPrefabを複製
        GameObject tutorialObj = Instantiate(tutorialPrefab, canvas.transform);

        //tutorialObj直下のMatrixTextが存在するなら[_matrixRowNum, 0]を生成
        if (tutorialObj.transform.GetChild(tutorialObj.transform.childCount - 1).GetComponent<MatrixText>() == null)
        {
            Debug.Log("MatrixTextコンポーネントを親ボタン直下かつ一番下のテキストオブジェクトにアタッチして下さい");
            return;
        }

        // tutorialObj.transform.GetChild(tutorialObj.transform.childCount - 1).GetComponent<MatrixText>()?.Initialize(_matrixRowNum);/
    }
}
