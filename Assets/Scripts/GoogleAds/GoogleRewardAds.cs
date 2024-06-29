using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.VisualScripting;


public class GoogleRewardAds : MonoBehaviour
{
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private const string _adUnitId = "ca-app-pub-3940256099942544/5224354917";//テスト用のユニットID
#elif UNITY_IPHONE
    private const string _adUnitId = "ca-app-pub-3940256099942544/1712485313";//テスト用のユニットID
#else
    private const string _adUnitId = "unused";
#endif

    //インスタンス
    [SerializeField] private CoinCount coinCount;

    //コインのイメージ
    [SerializeField] private GameObject coinObj;
    [SerializeField] private GameObject headerCoin;

    //ショップで500コイン獲得、広告見るボタン
    [SerializeField] private CustomButton watchRewardAdButton;

    //広告準備中を表示するUI
    private TextMeshProUGUI prepairingText;

    //広告準備中を表示するためのcts
    CancellationTokenSource cts;


    //リワード広告
    private RewardedAd _rewardedAd;

    //-------------------------------------------------------------------------------------
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
        });

        LoadAd();

        watchRewardAdButton.onClickCallback += ShowAd;
        cts = new CancellationTokenSource();
        prepairingText = watchRewardAdButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // prepairingText.text += ".";
    }

    /// <summary>
    /// Loads the ad.
    /// </summary>
    public void LoadAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null) DestroyAd();
        Debug.Log("Loading rewarded ad.");

        // Create our request used to load the ad.
        var adRequest = new AdRequest();

        // Send the request to load the ad.
        RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
        {
            // If the operation failed with a reason.
            // 何らかの理由で操作が失敗した場合、通信エラーなど。---------------※①
            if (error != null)
            {
                Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                return;
            }

            // If the operation failed for unknown reasons.
            // 不明な理由で操作が失敗した場合。
            // This is an unexpected error, please report this bug if it happens.
            // これは予期しないエラーです。発生した場合はこのバグを報告してください。----------※②
            if (ad == null)
            {
                Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                return;
            }

            // The operation completed successfully.
            // 操作は正常に完了しました。
            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
            _rewardedAd = ad;

            // Register to ad events to extend functionality.
            // 機能を拡張するには広告イベントに登録します。
            RegisterEventHandlers(ad);//---------------※③
        });
    }

    /// <summary>
    /// Shows the ad.
    /// </summary>
    public void ShowAd()
    {
        prepairingText.text = "広告準備中";
        isShown = true;
        RewardAdsRepairing(cts.Token).Forget();

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing rewarded ad.");//---------------※④
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log(string.Format("Rewarded ad granted a reward: {0} {1}", reward.Amount, reward.Type));
                isShown = false;

                //---------------※⑤
                //報酬受け取り
                if (SceneManager.GetActiveScene().name == "StageScene")
                {
                    coinCount.GetCoin(500);
                    EmissionAnimation.Receive(coinObj, 5, new Vector2(100, 100), watchRewardAdButton.transform.position, headerCoin.transform.position);
                    LoadAd();
                    cts?.Cancel();
                    prepairingText.text = "広告を見る";
                }
                else if (SceneManager.GetActiveScene().name == "Stage")
                {
                    SaveData.tax *= 2;
                    LoadAd();
                    cts?.Cancel();
                    SceneManager.LoadScene("StageScene");
                }
            });
        }
        else
        {
            Debug.LogError("Rewarded ad is not ready yet.");
        }
    }

    private bool isShown;

    ///「.」を演出するループ
    async public UniTask RewardAdsRepairing(CancellationToken ct)
    {
        Debug.Log("発動");
        string initialText = prepairingText.text;

        while (isShown)
        {
            await UniTask.Delay(500, cancellationToken: ct);
            prepairingText.text += ".";
            await UniTask.Delay(500, cancellationToken: ct);
            prepairingText.text += ".";
            await UniTask.Delay(500, cancellationToken: ct);
            prepairingText.text += ".";

            await UniTask.Delay(500, cancellationToken: ct);
            prepairingText.text = initialText;
        }
    }

    void OnDestroy()
    {
        cts?.Cancel();
    }

    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_rewardedAd != null)
        {
            Debug.Log("Destroying rewarded ad.");
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        // 広告が収益を上げたと推定される場合に発生します。
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        // Raised when an impression is recorded for an ad.
        // 広告のインプレッションが記録されるときに発生します。
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };

        // Raised when a click is recorded for an ad.
        // 広告のクリックが記録されたときに発生します。
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };

        // Raised when the ad opened full screen content.
        // 広告が全画面コンテンツを開いたときに発生します。
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };

        // Raised when the ad closed full screen content.
        // 広告が全画面コンテンツを閉じたときに発生します。
        // Unityエディター上では、_rewardedAd.Showよりも先に行われるので注意
        //---------------※⑥
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            // CheckEarnReward();
        };

        // Raised when the ad failed to open full screen content.
        // 広告が全画面コンテンツを開けなかった場合に発生します。
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
        };
    }
}
