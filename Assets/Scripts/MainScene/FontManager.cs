using System.Collections;
using System.Collections.Generic;
using Shapes2D;
using UnityEngine;
using FuchunLibrary;
using System.Linq;

public class FontManager : MonoBehaviour
{
    //インスタンス化
    [SerializeField] private DataManager dataManager;
    [SerializeField] private DiamondCount diamondCount;

    //MainのFont編集画面のFontView
    [SerializeField] private GameObject buyFontView;

    //FontViewContentの20個の子オブジェクト
    [SerializeField] private CustomButton[] buyFontButtons = new CustomButton[MainManager.NUMofFONTS];

    //全部フォントを買ってるなら知らせたい
    // [SerializeField] private GameObject alreadyBuyAllFont;


    void Start()
    {
        // for (int i = 0; i < buyFontButtons.Length; i++) buyFontButtons[i].gameObject.SetActive(!dataManager.data.haveFonts[i]);

        //ショッピングビューのフォント購入ボタンに下の関数をアタッチ
        // for (int i = 0; i < buyFontButtons.Length; i++) buyFontButtons[i].onClickCallback += () => { PushBuyFontButton(i); };

        //forでまとめたい
        buyFontButtons[0].onClickCallback += () => { PushBuyFontButton(0); };
        buyFontButtons[1].onClickCallback += () => { PushBuyFontButton(1); };
        buyFontButtons[2].onClickCallback += () => { PushBuyFontButton(2); };
        buyFontButtons[3].onClickCallback += () => { PushBuyFontButton(3); };
        buyFontButtons[4].onClickCallback += () => { PushBuyFontButton(4); };
        buyFontButtons[5].onClickCallback += () => { PushBuyFontButton(5); };
        buyFontButtons[6].onClickCallback += () => { PushBuyFontButton(6); };
        buyFontButtons[7].onClickCallback += () => { PushBuyFontButton(7); };
        buyFontButtons[8].onClickCallback += () => { PushBuyFontButton(8); };
        buyFontButtons[9].onClickCallback += () => { PushBuyFontButton(9); };
        buyFontButtons[10].onClickCallback += () => { PushBuyFontButton(10); };
        buyFontButtons[11].onClickCallback += () => { PushBuyFontButton(11); };
        buyFontButtons[12].onClickCallback += () => { PushBuyFontButton(12); };
        buyFontButtons[13].onClickCallback += () => { PushBuyFontButton(13); };
        buyFontButtons[14].onClickCallback += () => { PushBuyFontButton(14); };
        buyFontButtons[15].onClickCallback += () => { PushBuyFontButton(15); };
        buyFontButtons[16].onClickCallback += () => { PushBuyFontButton(16); };
        buyFontButtons[17].onClickCallback += () => { PushBuyFontButton(17); };
        buyFontButtons[18].onClickCallback += () => { PushBuyFontButton(18); };
        buyFontButtons[19].onClickCallback += () => { PushBuyFontButton(19); };

        // if (Library.CharacteristicFanction(dataManager.data.haveFonts) == dataManager.data.haveFonts.Length) alreadyBuyAllFont.SetActive(true);
        // buyFontView.GetComponent<RectTransform>().sizeDelta = new Vector2(880, 430 * Mathf.Ceil(((float)(dataManager.data.haveFonts.Length - Library.CharacteristicFanction(dataManager.data.haveFonts))) / 3f) - 10);
    }

    public void PushBuyFontButton(int i)
    {
        if (diamondCount.Diamond < 25)
        {
            WarnManager.instance.AppearWarning("ダイヤが足りません！", "このフォントを買うには\n25ダイヤ稼いできてください！");
            return;
        }

        diamondCount.GetDiamond(-25);
        Debug.Log(i);
        dataManager.data.haveFonts[i] = true;
        dataManager.Save();


    }
}
