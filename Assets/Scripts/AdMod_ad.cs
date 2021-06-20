using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

public class AdMod_ad : MonoBehaviour
{
    //string App_ID = "ca-app-pub-3940256099942544~3347511713";

    string Banner_Ad_ID = "ca-app-pub-3940256099942544/6300978111";
    string Interstitial_Ad_ID = "ca-app-pub-3940256099942544/1033173712";
    string Rewarded_Ad_ID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    [SerializeField] Button rewardedAdButton;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

        RequestBanner();
        ShowBannerAd();

        rewardedAdButton.gameObject.SetActive(false);
        RequestRewardedAd();
    }

    public void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen
        bannerView = new BannerView(Banner_Ad_ID, AdSize.Banner, AdPosition.Bottom);
    }
    public void ShowBannerAd()
    {
        // Create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request
        bannerView.LoadAd(request);
    }

    public void RequestRewardedAd()
    {
        rewardedAd = new RewardedAd(Rewarded_Ad_ID);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when ad ad request has done
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd.IsLoaded())
        {
            rewardedAd.Show();

            rewardedAdButton.gameObject.SetActive(false);
        }
    }

    private void HandleUserEarnedReward(object sender, Reward e)
    {
        Debug.Log("Earned Reward");

        RequestRewardedAd();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        rewardedAdButton.gameObject.SetActive(true);
    }
}
