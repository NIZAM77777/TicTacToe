using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdController : MonoBehaviour
{
    private BannerView bannerView;

    // Production Banner Ad Unit ID
    private readonly string adUnitId = "ca-app-pub-2830084422237874/3259963358";

    public void LoadBanner()
    {
        // Destroy existing banner if it exists
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        // Create a new banner
        bannerView = new BannerView(
            adUnitId,
            AdSize.Banner,
            AdPosition.Bottom);

        // Register events
        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("✅ Banner Ad Loaded Successfully.");
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError($"❌ Banner failed to load: {error}");
        };

        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log($"💰 Banner earned {adValue.Value} {adValue.CurrencyCode}");
        };

        // Create request
        AdRequest request = new AdRequest();

        // Load banner
        bannerView.LoadAd(request);
    }

    public void ShowBanner()
    {
        if (bannerView != null)
        {
            bannerView.Show();
            Debug.Log("Banner shown.");
        }
    }

    public void HideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
            Debug.Log("Banner hidden.");
        }
    }

    private void OnDestroy()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
