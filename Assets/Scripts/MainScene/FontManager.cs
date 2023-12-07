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
    [SerializeField] private GameObject[] fontContents = new GameObject[20];

    //全部フォントを買ってるなら知らせたい
    [SerializeField] private GameObject alreadyBuyAllFont;

    void Start()
    {
        for (int i = 0; i < fontContents.Length; i++) fontContents[i].SetActive(!dataManager.data.haveFonts[i]);
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





    // public Font font;

    // public enum Font
    // {
    //     //1からに修正したい
    //     Gothic = 0,
    //     Gothic_Bold = 1,
    //     Gothic_Rounded = 2,
    //     Mincho = 3,
    //     Gyosho = 4,
    //     Pop = 5,
    //     Textbook = 6,
    //     Textbook_Rounded = 7,
    //     Kaisho = 8,
    //     Chark = 9,
    //     Dot = 10,
    //     Handwritten = 11,
    //     Maka = 12,
    //     Scary = 13,
    //     Shake = 14,
    //     Sloppy = 15,
    //     Strong = 16,
    //     Thin = 17,
    //     Unknown = 18,
    //     Weak = 19,
    // }
}
