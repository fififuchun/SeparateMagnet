using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatrixText : MonoBehaviour
{
    //状況ごとのチュートリアルメッセージ
    [SerializeField] private MatrixTextSO tutorialSO;

    //自分自身
    private TextMeshProUGUI matrixElementText;

    //tutorialSOの位置
    //行
    [SerializeField, ReadOnly] private int matrixRowNum;
    // public MatrixText(int _matrixRowNum) { matrixRowNum = _matrixRowNum; }
    //列
    [SerializeField, ReadOnly] private int matrixColumnNum;


    //==================================================(50)
    //関数

    /// <summary>
    /// matrixRowNumを初期化
    /// </summary>
    public void Initialize(int _matrixRowNum)
    {
        //指定された_matrixRowNumがtutorialSOの行数を超えてたらreturn
        if (_matrixRowNum < 0 || tutorialSO.stringGroups.Count() <= _matrixRowNum)
        {
            Destroy(transform.parent.gameObject);
            Debug.Log("入力されたmatrixRowNumは存在しません");
            return;
        }
        //指定された_matrixRowNumがtutorialSOの行数を超えてなかったら初期化
        matrixRowNum = _matrixRowNum;

        InitializeText();
        transform.parent.GetComponent<Button>()?.onClick.AddListener(PushGoNextButton);
        // Debug.Log($"{matrixRowNum}に変更");
    }

    /// <summary>
    /// テキストを初期化
    /// </summary>
    public void InitializeText()
    {
        if (gameObject.GetComponent<TextMeshProUGUI>() == null)
        {
            Debug.Log("textコンポーネントをアタッチしてください");
            return;
        }
        matrixElementText = gameObject.GetComponent<TextMeshProUGUI>();
        Debug.Log("MatrixText: 初期化されました");

        matrixElementText.text = tutorialSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }

    /// <summary>
    /// matrixTextの列を次に進める
    /// </summary>
    public void PushGoNextButton()
    {
        Debug.Log("次へ進む");
        matrixColumnNum++;
        if (tutorialSO.stringGroups[matrixRowNum].strings.Length <= matrixColumnNum)
        {
            Debug.Log("テキストがありません");
            Destroy(gameObject.transform.parent.gameObject);
            return;
        }

        matrixElementText.text = tutorialSO.stringGroups[matrixRowNum].strings[matrixColumnNum];
    }
}
