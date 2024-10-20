using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class Initaliseadds : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidgameid;
    [SerializeField] private string iosGameId;

    // Start is called before the first frame update
    [SerializeField] private bool Testing;
    private string gameId;

    public void OnInitializationComplete()
    {
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }

    private void Awake()
    {
#if UNITY_IOS
gamId=iosGameId;
#elif UNITY_ANDROID
        gameId = androidgameid;
#endif
        if (!Advertisement.isInitialized&&Advertisement.isSupported) 
        {
            Advertisement.Initialize(gameId, Testing, this);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
