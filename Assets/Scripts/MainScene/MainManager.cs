using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using FuchunLibrary;

public class MainManager : MonoBehaviour
{
  //データ管理
  [SerializeField] private DataManager dataManager;
  [SerializeField] private DiamondCount diamondCount;
  // [SerializeField] private RankManager rankManager;

  //メインシーンのアニメーション用
  [SerializeField] private GameObject mainViewContent;
  [SerializeField] private RectTransform[] stageImages;
  [SerializeField] private RectTransform[] outlineImages = new RectTransform[STAGE_COUNT];


  //一旦オフにしてスクロールのスピードを殺す
  [SerializeField] private ScrollRect scrollRect;

  //ステージ増加の際は要変更
  public const int STAGE_COUNT = 4;
  [SerializeField] private Image[] lockImages = new Image[STAGE_COUNT];

  //「検討を重ねる」のテキスト
  [SerializeField] private TextMeshProUGUI repeatKentoText;

  //ショップのフォント枠開放ボタン
  [SerializeField] private CustomButton buyFrameButton;


  public void Start()
  {
    if (Library.CharacteristicFanction(dataManager.data.haveFonts) < 2)
    {
      dataManager.data.haveFonts[(int)KentoFont.Dot - 1] = true;
      dataManager.data.haveFonts[(int)KentoFont.Gyosho - 1] = true;
    }
    // for (int i = 0; i < fontViewContent.transform.childCount; i++) fontViewContent.transform.GetChild(i).gameObject.SetActive(dataManager.data.haveFonts[i]);

    for (int i = 0; i <= Mathf.CeilToInt(RankManager.Rank / 5); i++)
    {
      if (i >= STAGE_COUNT) break;
      lockImages[i].enabled = false;
    }

    buyFrameButton.onClickCallback += IncreaseFontCount;
    editButton.onClickCallback += () => PushEditButton(!canEdit);
    outlineImages[0].DOSizeDelta(new Vector3(600, 600), 0.5f);

    cts = new CancellationTokenSource();
    repeatKentoButton.onClickCallback += () => PushRepeatKentoButton(cts.Token);

    ShowFontImage();
    diamondCount.GetDiamond(11100);
  }

  void Update()
  {
    if (Input.GetMouseButtonUp(0))
    {
      scrollRect.enabled = false;
      mainViewContent.transform.DOKill();
      mainViewContent.transform.DOMove(new Vector2(Mathf.Floor((mainViewContent.transform.position.x + 415) / 830) * 830, mainViewContent.transform.position.y), 0.5f);

      int stageNum = (int)(1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830));
      if (RankManager.Rank < (stageNum - 1) * 5) repeatKentoText.text = $"ランク{(stageNum - 1) * 5}で解放";
      else repeatKentoText.text = $"検討を重ねる";

      //アニメーション
      for (int i = 0; i < STAGE_COUNT; i++)
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
  [SerializeField] private CustomButton editButton;
  [SerializeField] private bool canEdit;

  //trueを入れると編集可能にする、falseなら逆挙動
  public void PushEditButton(bool _canEdit)
  {
    canEdit = _canEdit;

    mainViewContent.transform.parent.parent.gameObject.SetActive(!canEdit);
    fontViewObject.SetActive(canEdit);

    if (canEdit) editButtonText.text = "編集完了";
    else editButtonText.text = "編集する";
  }

  //持ちフォントの表示用
  public const int FONT_COUNT = 20;

  //メイン
  [SerializeField] private Image[] fontImages = new Image[6];
  [SerializeField] private Sprite lockImage;
  [SerializeField] private Sprite transparencyImage;
  [SerializeField] private Sprite[] kentoImages = new Sprite[FONT_COUNT];

  //ショッピング
  [SerializeField] private Image fontFrameImage;
  [SerializeField] private TextMeshProUGUI buyFontFrameText;
  [SerializeField] private TextMeshProUGUI requireCoinText;
  [SerializeField] private Sprite[] buyFontFrameImages = new Sprite[4];


  public void ShowFontImage()
  {
    for (int i = 0; i < fontViewContent.transform.childCount; i++) fontViewContent.transform.GetChild(i).gameObject.SetActive(dataManager.data.haveFonts[i]);

    for (int i = 0; i < dataManager.ReleasedFontCount(); i++)
    {
      if (dataManager.data.fontNumbers[i] > 0) fontImages[i].sprite = kentoImages[dataManager.data.fontNumbers[i] - 1];
      else if (dataManager.data.fontNumbers[i] == 0) fontImages[i].sprite = transparencyImage;
      else fontImages[i].sprite = lockImage;

      // dataManager.Save();
    }

    dataManager.Save();

    //全開放されてるなら返す、されてないならショップに変更を加える
    if (dataManager.ReleasedFontCount() == 6)
    {
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
    //ダイヤが足りない、または全開放済なら返す
    if (dataManager.ReleasedFontCount() == 6)
    {
      WarnManager.instance.AppearWarning("おめでとう！", "もう全てのフォント枠を\n開放していますよ！");
      return;
    }
    else if (diamondCount.Diamond < Mathf.Pow(10, dataManager.ReleasedFontCount() - 1))
    {
      WarnManager.instance.AppearWarning("ダイヤが足りません！", $"このフォントを買うには\n{Mathf.Pow(10, dataManager.ReleasedFontCount() - 1)}ダイヤ稼いできてください！");
      return;
    }

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
      if (dataManager.data.fontNumbers[j] == i + 1)
      {
        //警告
        WarnManager.instance.AppearWarning("既にスタメンです！", $"このフォントは既に\n通常国会に連れていく\nフォントとして\n選ばれています");
        return;
      }
    }

    //fontNumbersに0(フォント枠解放済みだが、フォント未記入)が存在するなら、一番手前のfontNumbersにi番目を代入
    if (dataManager.data.fontNumbers.Contains(0)) dataManager.data.fontNumbers[Library.SearchNumberIndex(dataManager.data.fontNumbers, 0)] = i + 1;
    ShowFontImage();
  }

  public void PushDeleteFontButton(int i)
  {
    dataManager.data.fontNumbers[i] = 0;
    ShowFontImage();
  }

  //ステージ番号
  public static int stageNum;
  //kentoButton
  [SerializeField] private CustomButton repeatKentoButton;
  //ブラックイメージ
  [SerializeField] private Image shiftStageImage;
  //AudioManagerのインスタンス化
  [SerializeField] private AudioManager audioManager;
  //検討を重ねるボタンを押した後のUnitask破棄
  CancellationTokenSource cts;

  //ステージ遷移
  async public void PushRepeatKentoButton(CancellationToken ct)
  {
    //現在最前面にあるステージのナンバー
    stageNum = (int)(1 - Mathf.Floor((mainViewContent.transform.position.x + 415) / 830));
    PlayerPrefs.Save();

    //ステージ遷移処理
    if (RankManager.Rank < (stageNum - 1) * 5)
    {
      WarnManager.instance.AppearWarning("ランクが足りません！", $"ステージ{stageNum}に挑戦するには\nランクを{(stageNum - 1) * 5}に\nしてください");
      return;
    }

    shiftStageImage.gameObject.SetActive(true);

    await UniTask.WhenAll(
      shiftStageImage.DOFade(1f, 1f).ToUniTask(cancellationToken: ct),
      audioManager.backBGM.DOFade(0f, 1f).ToUniTask(cancellationToken: ct)
    );
    SceneManager.LoadScene("Stage");
  }

  [SerializeField] TextMeshProUGUI textMeshProUGUI;

  void OnDestroy()
  {
    cts?.Cancel();
  }
}
