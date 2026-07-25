using UnityEngine;
using GoogleMobileAds.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { get; private set; }

    private BannerAdController bannerController;
    private InterstitialAdController interstitialController;
    public InterstitialAdController InterstitialController => interstitialController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bannerController = GetComponent<BannerAdController>();
        interstitialController = GetComponent<InterstitialAdController>();

        InitializeAdMob();
    }

    private void InitializeAdMob()
    {
        Debug.Log("Initializing Google Mobile Ads...");

        MobileAds.Initialize(initStatus =>
        {
            Debug.Log("Google Mobile Ads Initialized Successfully!");

            bannerController.LoadBanner();
            interstitialController.LoadInterstitial();
        });
    }
}