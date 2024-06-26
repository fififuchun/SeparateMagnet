using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class WarnManager : MonoBehaviour
{
    public static WarnManager instance;
    void Awake() { if (instance == null) instance = this; }

    //警告そのもの
    [SerializeField] private GameObject warningObject;

    //警告の原因
    [SerializeField] private TextMeshProUGUI warningTitleText;

    //警告の解決法
    [SerializeField] private TextMeshProUGUI warningText;

    //閉じるボタン
    [SerializeField] private CustomButton closeWarningButton;

    /// <summary>
    /// 警告を出す
    /// </summary>
    /// <param name="title">警告上部のタイトル</param>
    /// <param name="resolve">警告内部の解決方法</param>
    public void AppearWarning(string title, string resolve)
    {
        closeWarningButton.onClickCallback += DisappearWarning;

        warningTitleText.text = title;
        warningText.text = resolve;

        warningObject.SetActive(true);
    }

    public void DisappearWarning()
    {
        warningObject.SetActive(false);
    }
}
