using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Linq;

public class MainManager : MonoBehaviour
{
  //データ管理
  [SerializeField] private static DataManager dataManager;
  [SerializeField] private DiamondCount diamondCount;

  //メインシーンのアニメーション用
  [SerializeField] private GameObject mainViewContent;
  [SerializeField] private RectTransform[] stageImages;

  //一旦オフにしてスクロールのスピードを殺す
  [SerializeField] private ScrollRect scrollRect;

  //
  private const int stageCount = 4;
  [SerializeField] private Image[] lockImages = new Image[stageCount];


  void Start()
  {
    if (Library.CharacteristicFanction(dataManager.data.haveFonts) < 2)
    {
      dataManager.data.haveFonts[4] = true;
      dataManager.data.haveFonts[10] = true;
    }
    for (int i = 0; i < fontViewContent.transform.childCount; i++) fontViewContent.transform.GetChild(i).gameObject.SetActive(dataManager.data.haveFonts[i]);

    ShowFontImage();
  }

  void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      scrollRect.enabled = false;
      mainViewContent.transform.DOKill();
      mainViewContent.transform.DOMove(new Vector2(Mathf.Floor((mainViewContent.transform.position.x + 415) / 830) * 830, mainViewContent.transform.position.y), 0.5f);
    }
    else if (!scrollRect.enabled) scrollRect.enabled = true;

    // Debug.Log(dataManager.ReleasedFontCount());
  }

  //持ちフォントの編集
  [SerializeField] private TextMeshProUGUI editButtonText;
  [SerializeField] private GameObject fontviewObject;
  private bool canEdit;
  public void PushEditButton()
  {
    canEdit = !canEdit;

    mainViewContent.transform.parent.parent.gameObject.SetActive(canEdit);
    fontviewObject.SetActive(!canEdit);

    if (canEdit) editButtonText.text = "編集する";
    else editButtonText.text = "編集完了";
  }

  //持ちフォントの表示用
  //メイン
  [SerializeField] private Image[] fontImages = new Image[6];
  [SerializeField] private Sprite lockImage;
  [SerializeField] private Sprite transparencyImage;
  [SerializeField] private Sprite[] kentoImages = new Sprite[20];

  //ショッピング
  [SerializeField] private Image FontFrameImage;
  [SerializeField] private TextMeshProUGUI buyFontFrameText;
  [SerializeField] private TextMeshProUGUI requireCoinText;
  [SerializeField] private Sprite[] buyFontFrameImages = new Sprite[4];


  public void ShowFontImage()
  {
    for (int i = 0; i < dataManager.ReleasedFontCount(); i++)
    {
      if (dataManager.data.fontNumbers[i] > 0) fontImages[i].sprite = kentoImages[dataManager.data.fontNumbers[i] - 1];
      else if (dataManager.data.fontNumbers[i] == 0) fontImages[i].sprite = transparencyImage;
      else fontImages[i].sprite = lockImage;

      dataManager.Save(dataManager.data);
    }

    if (dataManager.ReleasedFontCount() == 6)
    {
      FontFrameImage.sprite = buyFontFrameImages[dataManager.ReleasedFontCount() - 3];
      buyFontFrameText.text = "すべてのフォント枠を開放しました";
      requireCoinText.text = "Max";
      requireCoinText.color = Color.red;

      return;
    }

    FontFrameImage.sprite = buyFontFrameImages[dataManager.ReleasedFontCount() - 3];
    buyFontFrameText.text = $"{dataManager.ReleasedFontCount() + 1}つ目のフォント枠を開放する";
    requireCoinText.text = $"{Mathf.Pow(10, dataManager.ReleasedFontCount() - 1)}";
  }

  //編集できるフォントの数を増やす・ショッピングシーンのフォント枠解放ボタンにアタッチ
  public void IncreaseFontCount()
  {
    if (diamondCount.Diamond < Mathf.Pow(10, dataManager.ReleasedFontCount() - 1) || dataManager.ReleasedFontCount() == 6) return;

    diamondCount.GetDiamond(-(int)Mathf.Pow(10, dataManager.ReleasedFontCount() - 1));
    dataManager.data.fontNumbers[dataManager.ReleasedFontCount()] = 0;

    ShowFontImage();
  }

  //手持ちのフォントを持って行くフォントに追加
  [SerializeField] private GameObject fontViewContent;
  public void PushAddFontButton(int i)
  {
    for (int j = 0; j < dataManager.ReleasedFontCount(); j++)
    {
      if (dataManager.data.fontNumbers[j] == i)
      {
        //警告
        return;
      }
    }

    if (dataManager.data.fontNumbers.Contains(0)) dataManager.data.fontNumbers[Library.SearchNumberIndex(dataManager.data.fontNumbers, 0)] = i;
    ShowFontImage();
  }

  public void PushDeleteFontButton(int i)
  {
    dataManager.data.fontNumbers[i] = 0;
    ShowFontImage();
  }
}
