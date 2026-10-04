using UnityEngine;
using GoogleMobileAds.Api;

public class InterstitialAdController : MonoBehaviour
{
    private InterstitialAd interstitialAd;

    // Production Interstitial Ad Unit ID
    private readonly string adUnitId = "ca-app-pub-2830084422237874/4823317309";

    private int completedGames = 0;

    public void TryShowInterstitial()
    {
        completedGames++;

        Debug.Log($"Completed Games: {completedGames}");

        if (completedGames % 3 == 0)
        {
            ShowInterstitial();
        }
    }

    public void LoadInterstitial()
    {
        // Destroy old ad
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading Interstitial...");

        AdRequest request = new AdRequest();

        InterstitialAd.Load(adUnitId, request, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null || ad == null)
            {
                Debug.LogError("Interstitial failed to load: " + error);
                return;
            }

            Debug.Log("✅ Interstitial Loaded Successfully.");

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Interstitial Closed.");

                interstitialAd.Destroy();
                interstitialAd = null;

                // Automatically load the next ad
                LoadInterstitial();
            };

            interstitialAd.OnAdFullScreenContentFailed += (AdError adError) =>
            {
                Debug.LogError("Interstitial failed to show: " + adError);

                interstitialAd.Destroy();
                interstitialAd = null;

                LoadInterstitial();
            };
        });
    }

    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing Interstitial...");
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial not ready.");
        }
    }

    private void OnDestroy()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }
}
