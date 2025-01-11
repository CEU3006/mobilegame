using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Admanager : MonoBehaviour
{
    public AddScript addscrip;
    public Initaliseadds initaliseadds;
    public BannerAd bannerscrip;
    bool doonce = true;
    bool doonce2 = true;
    public static Admanager instance
    {
        get;private set;
    }
    private void Awake()
    {
        if (instance != null&& instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Advertisement.isInitialized&& doonce2)
        {
            addscrip.LoadAdd();

            bannerscrip.LoadBannerAd();
            doonce2=false;
        }
        if (doonce&&Advertisement.Banner.isLoaded)
        {
            doonce=false;
            Debug.Log("load ad");
            bannerscrip.ShowBannerAd();

        }
        if (SceneManager.GetActiveScene().name!= "MainMenu")
        {
            bannerscrip.HidBannerAd();
        }
    }
}
