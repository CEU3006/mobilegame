using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;

public class AddScript : MonoBehaviour,IUnityAdsLoadListener,IUnityAdsShowListener
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
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadAdd()
    {
        Debug.Log(AdUnitId);
        Advertisement.Load(AdUnitId, this);
    }
    public void ShowAd()
    {
        Advertisement.Show(AdUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAdd();

    }
}
