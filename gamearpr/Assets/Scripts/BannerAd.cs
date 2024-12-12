using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAd : MonoBehaviour
{
    [SerializeField] private string androidAdUnitid;
    [SerializeField] private string iosAdUnitId;

    // Start is called before the first frame update
    private string AdUnitId;


    private void Awake()
    {
#if UNITY_IOS
AdUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        AdUnitId = androidAdUnitid;
#endif

        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }
    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback=BannerLoaded,
            errorCallback=BannerLoadedError
        };
        Advertisement.Banner.Load(AdUnitId, options);
    }

    private void BannerLoadedError(string message)
    {
        Debug.Log("bannererror");
    }

    private void BannerLoaded()
    {
        Debug.Log("bannerloaded");
    }
    public void ShowBannerAd()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHiden
        };
        Advertisement.Banner.Show(AdUnitId, options);
    }

    private void BannerShown()
    {
    }
    private void BannerClicked()
    {
    }
    private void BannerHiden()
    {
    }
    public void HidBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
