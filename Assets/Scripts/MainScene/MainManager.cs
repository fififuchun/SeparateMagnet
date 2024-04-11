using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
  //データ管理
  [SerializeField] private DataManager dataManager;
  [SerializeField] private DiamondCount diamondCount;
  [SerializeField] private RankManager rankManager;

  //メインシーンのアニメーション用
  [SerializeField] private GameObject mainViewContent;
  [SerializeField] private RectTransform[] stageImages;
  [SerializeField] private RectTransform[] outlineImages = new RectTransform[stageCount];


  //一旦オフにしてスクロールのスピードを殺す
  [SerializeField] private ScrollRect scrollRect;

  //ステージ増加の際は要変更
  private const int stageCount = 4;
  [SerializeField] private Image[] lockImages = new Image[stageCount];

  //「検討を重ねる」のテキスト
  [SerializeField] private TextMeshProUGUI repeatKentoText;


  void Start()
  {
    if (Library.CharacteristicFanction(dataManager.data.haveFonts) < 2)
    {
      dataManager.data.haveFonts[4] = true;
      dataManager.data.haveFonts[10] = true;
    }
    for (int i = 0; i < fontViewContent.transform.childCount; i++) fontViewContent.transform.GetChild(i).gameObject.SetActive(dataManager.data.haveFonts[i]);

    for (int i = 0; i <= Mathf.CeilToInt(rankManager.Rank / 5); i++) lockImages[i].enabled = false;

    outlineImages[0].DOSizeDelta(new Vector3(600, 600), 0.5f);
    ShowFontImage();
  }

  void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      scrollRect.enabled = false;
      mainViewContent.transform.DOKill();
      mainViewContent.transform.DOMove(new Vector2(Mathf.Floor((mainViewContent.transform.position.x + 415) / 830) * 830, mainViewContent.transform.position.y), 0.5f);

      int stageNum = (int)(1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830));
      if (rankManager.Rank < (stageNum - 1) * 5) repeatKentoText.text = $"ランク{(stageNum - 1) * 5}で解放";
      else repeatKentoText.text = $"検討を重ねる";

      //アニメーション
      for (int i = 0; i < stageCount; i++)
      {
        if (i == stageNum - 1)
        {
          // float ratio = 1.15f;
          outlineImages[i].DOSizeDelta(new Vector3(600, 600), 0.5f);
        }
        else
        {
          outlineImages[i].sizeDelta = new Vector3(525, 525);
        }
      }
    }
    else if (!scrollRect.enabled) scrollRect.enabled = true;
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

  //ステージ番号
  public static int stageNum;
  //ステージ遷移
  public void PushRepeatKentoButton()
  {
    //現在最前面にあるステージのナンバー
    stageNum = (int)(1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830));
    PlayerPrefs.Save();

    //ステージ遷移処理
    if (rankManager.Rank < (stageNum - 1) * 5)
    {
      WarnManager.instance.AppearWarning("ランクが足りません！", $"ステージ{stageNum}に挑戦するには\nランクを{(stageNum - 1) * 5}に\nしてください");
      return;
    }
    SceneManager.LoadScene($"Stage");
  }
}
