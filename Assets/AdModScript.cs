using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

public class AdModScript : MonoBehaviour
{
    public Text adStatus;

    //string App_ID = "ca-app-pub-3940256099942544~3347511713";

    string Banner_Ad_ID = "ca-app-pub-3940256099942544/6300978111";
    string Interstitial_Ad_ID = "ca-app-pub-3940256099942544/1033173712";
    string RewardedVideo_Ad_ID = "ca-app-pub-3940256099942544/5224354917";

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    private void Start()
    {
        MobileAds.Initialize(initStatus => { });

        //MobileAds.Initialize(App_ID);
    }

    public void RequestBanner()
    {
        // Create a 320x50 banner at the top of the screen
        bannerView = new BannerView(Banner_Ad_ID, AdSize.Banner, AdPosition.Bottom);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;
    }

    public void ShowBannerAd()
    {
        // Create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request
        bannerView.LoadAd(request);
    }

    public void RequestInterstitial()
    {
        // Initialize an Interstitial Ad
        interstitialAd = new InterstitialAd(Interstitial_Ad_ID);

        // Called when an ad request has successfully loaded
        interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request has failed to loaded
        interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown
        interstitialAd.OnAdOpening += HandleOnAdOpened;
        // Called when an ad is closed
        interstitialAd.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(request);
    }

    public void ShowInterstitalAd()
    {
        if (interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
    }

    public void RequestRewardedAd()
    {
        rewardedAd = new RewardedAd(RewardedVideo_Ad_ID);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleOnAdOpened;
        // Called when an ad request failed to show.
        //this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        //this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleOnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
    
    // FOR EVENTS AND DELEGATES FOR ADS

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        adStatus.text = "Ad Loaded";
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        adStatus.text = "Ad Failed To Load";
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        adStatus.text = "Ad Opened";
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        adStatus.text = "Ad Closed";
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        adStatus.text = "Ad Leaving Application";
    }
}
