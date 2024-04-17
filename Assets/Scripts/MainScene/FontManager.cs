using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;

public class FontManager : MonoBehaviour
{
    //インスタンス化
    [SerializeField] private DataManager dataManager;
    [SerializeField] private DiamondCount diamondCount;

    //MainのFont編集画面のFontView
    [SerializeField] private GameObject buyFontView;

    //FontViewContentの20個の子オブジェクト
    [SerializeField] private CustomButton[] buyFontButtons = new CustomButton[20];

    //全部フォントを買ってるなら知らせたい
    [SerializeField] private GameObject alreadyBuyAllFont;

    //名前の昇順からフォント販売順への変換行列
    [SerializeField] private int[] switchFontNumArray = new int[20] { 3, 2, 4, 9, 5, 10, 15, 16, 7, 0, 1, 6, 8, 11, 12, 13, 14, 17, 18, 19 };
    // private int[,] switchFontNumMatrix = new int[20, 2]
    // {
    //     {0 ,9},
    //     {1 ,10},
    //     {2 ,1},
    //     {3 ,0},,
    //     {4 ,2},
    //     {5 ,4},
    //     {6 ,11},
    //     {7 ,8},
    //     {8 ,12},
    //     {9 ,3},
    //     {10,5},
    //     {11,13},
    //     {12,14},
    //     {13,15},
    //     {14,16},
    //     {15,6},
    //     {16,7},
    //     {17,17},
    //     {18,18},
    //     {19,19}
    // };

    void Start()
    {
        for (int i = 0; i < buyFontButtons.Length; i++) {
            buyFontButtons[i].gameObject.SetActive(!dataManager.data.haveFonts[i]);
            // buyFontButtons[i].onClickCallback.  //= PushBuyFontButton(i);
        }

        if (Library.CharacteristicFanction(dataManager.data.haveFonts) == dataManager.data.haveFonts.Length) alreadyBuyAllFont.SetActive(true);
        buyFontView.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 430 * Mathf.Ceil(((float)(dataManager.data.haveFonts.Length - Library.CharacteristicFanction(dataManager.data.haveFonts))) / 3f) - 10);
    }

    public void PushBuyFontButton(int i)
    {
        if (diamondCount.Diamond < 25)
        {
            //警告
            return;
        }
        diamondCount.GetDiamond(-25);
        dataManager.data.haveFonts[i] = true;
        Start();
    }
}
