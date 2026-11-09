#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAdController : MonoBehaviour
{
    private BannerView bannerView;
    private readonly string adUnitId = "ca-app-pub-2830084422237874/3259963358";

    public void LoadBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += () =>
            Debug.Log("Banner Ad Loaded Successfully.");

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            Debug.LogError($"Banner failed to load: {error}");

        bannerView.OnAdPaid += (AdValue adValue) =>
            Debug.Log($"Banner earned {adValue.Value} {adValue.CurrencyCode}");

        bannerView.LoadAd(new AdRequest());
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
#endif
