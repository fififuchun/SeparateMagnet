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
  [SerializeField] private RectTransform[] outlineImages = new RectTransform[STAGECOUNT];


  //一旦オフにしてスクロールのスピードを殺す
  [SerializeField] private ScrollRect scrollRect;

  //ステージ増加の際は要変更
  public const int STAGECOUNT = 4;
  [SerializeField] private Image[] lockImages = new Image[STAGECOUNT];

  //「検討を重ねる」のテキスト
  [SerializeField] private TextMeshProUGUI repeatKentoText;


  public void Start()
  {
    if (Library.CharacteristicFanction(dataManager.data.haveFonts) < 2)
    {
      dataManager.data.haveFonts[(int)KentoFont.Dot - 1] = true;
      dataManager.data.haveFonts[(int)KentoFont.Gyosho - 1] = true;
    }
    for (int i = 0; i < fontViewContent.transform.childCount; i++) fontViewContent.transform.GetChild(i).gameObject.SetActive(dataManager.data.haveFonts[i]);

    for (int i = 0; i <= Mathf.CeilToInt(rankManager.Rank / 5); i++) lockImages[i].enabled = false;

    outlineImages[0].DOSizeDelta(new Vector3(600, 600), 0.5f);
    // dataManager.showFontImage.AddListener(ShowFontImage);
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
      for (int i = 0; i < STAGECOUNT; i++)
      {
        if (i == stageNum - 1) outlineImages[i].DOSizeDelta(new Vector3(600, 600), 0.5f);
        else outlineImages[i].sizeDelta = new Vector3(525, 525);
      }
    }
    else if (!scrollRect.enabled) scrollRect.enabled = true;
  }

  //持ちフォントの編集
  [SerializeField] private TextMeshProUGUI editButtonText;
  [SerializeField] private GameObject fontViewObject;
  private bool canEdit;
  public void PushEditButton()
  {
    canEdit = !canEdit;

    mainViewContent.transform.parent.parent.gameObject.SetActive(canEdit);
    fontViewObject.SetActive(!canEdit);

    if (canEdit) editButtonText.text = "編集する";
    else editButtonText.text = "編集完了";
  }

  public void PushNOTMainButton()
  {
    canEdit = false;
    mainViewContent.transform.parent.parent.gameObject.SetActive(!canEdit);
    fontViewObject.SetActive(canEdit);
  }

  //持ちフォントの表示用
  public const int NUMofFONTS = 20;

  //メイン
  [SerializeField] private Image[] fontImages = new Image[6];
  [SerializeField] private Sprite lockImage;
  [SerializeField] private Sprite transparencyImage;
  [SerializeField] private Sprite[] kentoImages = new Sprite[NUMofFONTS];

  //ショッピング
  [SerializeField] private Image fontFrameImage;
  [SerializeField] private TextMeshProUGUI buyFontFrameText;
  [SerializeField] private TextMeshProUGUI requireCoinText;
  [SerializeField] private Sprite[] buyFontFrameImages = new Sprite[4];


  public void ShowFontImage()
  {
    for (int i = 0; i < dataManager.ReleasedFontCount(); i++)
    {
      if (dataManager.data.fontNumbers[i] > 0) fontImages[i].sprite = kentoImages[dataManager.data.fontNumbers[i]];
      else if (dataManager.data.fontNumbers[i] == 0) fontImages[i].sprite = transparencyImage;
      else fontImages[i].sprite = lockImage;

      dataManager.Save(dataManager.data);
    }

    if (dataManager.ReleasedFontCount() == 6)
    {
      // Debug.Log(dataManager.ReleasedFontCount() - 3);
      fontFrameImage.sprite = buyFontFrameImages[dataManager.ReleasedFontCount() - 3];
      buyFontFrameText.text = "すべてのフォント枠を開放しました";
      requireCoinText.text = "Max";
      requireCoinText.color = Color.red;

      return;
    }

    fontFrameImage.sprite = buyFontFrameImages[dataManager.ReleasedFontCount() - 3];
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

    //fontNumbersに0(フォント枠解放済みだが、フォント未記入)が存在するなら、一番手前のfontNumbersにi番目を代入
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
