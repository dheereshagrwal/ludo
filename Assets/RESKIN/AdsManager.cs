using System;
using UnityEngine;
using GoogleMobileAds.Api;
using AssemblyCSharp;

/// <summary>
/// Advertising sample.
/// This is a reference/sample script for implementing ads in your mobile game.
/// For our Google Play version we used Chartboost.
/// </summary>
public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    public bool forceShowInterstitial = false; // bool/toggle so we can check and force an interstitial at certain moments in the game

    public bool hasInterstitial = false; // is an interstitial cached?
    public bool hasRewardedVideo = false; // is a rewarded video cached?

    private BannerView bannerView;
    private InterstitialAd interstitial;
    private RewardBasedVideoAd rewardBasedVideo;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void OnEnable()
    {
        // Do stuff
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        DontDestroyOnLoad(gameObject);
        // Do stuff
#if UNITY_ANDROID
        string appId = ADS_ID.APP_ID;
#elif UNITY_IPHONE
        string appId = ADS_ID.APP_ID;
#else
            string appId = "unexpected_platform";
#endif

        MobileAds.Initialize(appId);

        //this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        // Called when the user should be rewarded for watching a video.
        //rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;

        // E.g. Cache video/interstitials, setup delegates, whatnot...
        SetAdVideoReward();

        CacheInterstitial();
        CacheVideo();

        // Register me
        //Scripts.advertising = this;
    }

    public void CacheBanner()
    {
        Debug.Log("Banner being cached");

        bannerView = new BannerView(ADS_ID.BANNER, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void CacheInterstitial()
    {
        hasInterstitial = true;
#if UNITY_ANDROID
        string adUnitId = ADS_ID.INTER;
#elif UNITY_IPHONE
        string adUnitId = ADS_ID.INTER;
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);

        Debug.Log("Inter being cached");
    }

    public void ShowBanner()
    {
        HideBanner();
        CacheBanner();
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            Debug.Log("Hide banner");
            bannerView.Destroy();
        }
    }

    public void ShowInterstitial()
    {
        Debug.Log("[AdvertisingSample] ShowInterstitial called");

#if UNITY_ANDROID || UNITY_IPHONE

        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            Debug.Log("INTER NOT LOADED LOL");
        }

#endif

        CacheInterstitial();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        interstitial.Destroy();
    }

    private void SetAdVideoReward()
    {
        // Get singleton reward based video ad reference.
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
    }

    public void CacheVideo()
    {
        string adUnitId = ADS_ID.VIDEO;
       
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameManager.Instance.playfabManager.addCoinsRequest(StaticStrings.rewardForVideoAd);
    }

    public void ShowVideo()
    {
        Debug.Log("[AdvertisingSample] ShowVideo called");

        if (rewardBasedVideo.IsLoaded())
        {
            rewardBasedVideo.Show();
        }
        CacheVideo();
    }
}