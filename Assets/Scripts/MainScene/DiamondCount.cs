using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiamondCount : MonoBehaviour
{
    //dataManagerインスタンス
    [SerializeField] private DataManager dataManager;

    //ダイアモンドプロパティ
    // private int diamond;
    public int Diamond { get => dataManager.data.diamond; }

    //ヘッダーのダイアモンドテキスト
    [SerializeField] private TextMeshProUGUI diamondText;


    public void GetDiamond(int i)
    {
        dataManager.data.diamond += i;
        UpdateDiamond();

        dataManager.Save();
    }

    public void UpdateDiamond()
    {
        diamondText.text = Diamond.ToString();
    }
}
