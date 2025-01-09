using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using TMPro;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using System;



public class MainMenuSystemManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject LevelSelect;
    [SerializeField] GameObject MultiSelect;
    [SerializeField] GameObject Connectingscreen;
    [SerializeField] GameObject WaitingScreen;
    [SerializeField] GameObject MultiMenuPickingScreen;

    public GameObject network;
    [SerializeField] GameObject SettingsSellect;
    [SerializeField] GameObject Musicondisplay;
    [SerializeField] GameObject Musicoffdisplay;
    [SerializeField] string ipAddress;
    [SerializeField] UnityTransport transport;
    [SerializeField] TextMeshProUGUI ipAddressText;
    [SerializeField] TMP_InputField ip;
    TestRelay relay;
    NetworkManager netman;
    AudioSource musicsource;
    public int playersConnected;
    bool waitingforplayer = false;
    bool Musicon;
    NetworkSceneManager sceneman;
    // Start is called before the first frame update
    private void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    void Start()
    {

       

        LoadData();
        musicsource = gameObject.GetComponent<AudioSource>();

        if (Musicon)
        {
            musicsource.Play();
        }
        network = GameObject.Find("Network");
        netman = network.GetComponent<NetworkManager>();
        if (netman != null)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDiss;
        }
        relay = network.GetComponent<TestRelay>();
        
        LogInToGooglePlay();
}
    private void LogInToGooglePlay()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }
    private void ProcessAuthentication(SignInStatus status)
    {
        if (status==SignInStatus.Success)
        {
            relay.keepConnectedToGoogle = true;
        }
        else
        {
            relay.keepConnectedToGoogle = false;

        }
    }

void OnPlayerJoin(ulong clientId)
    {
        Debug.Log($"Player with Client ID {clientId} has joined the game.");
    }
    void OnPlayerDiss(ulong clientId)
    {
        onWantingtoleave();
    }
    public void onWantingtoleave()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            MultiSelect.SetActive(true);
            Connectingscreen.SetActive(false);
            MultiMenuPickingScreen.SetActive(false);
            WaitingScreen.SetActive(false);
            NetworkManager.Singleton.Shutdown();
        }
    }
    void Update()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count == 2 && waitingforplayer)
            {
                waitingforplayer = false;
                WaitingScreen.SetActive(false);
                MultiMenuPickingScreen.SetActive(true);
                Social.ReportProgress(GPGSIDs.achievement_play_with_a_friend, 100, null);


            }
        }
        if (NetworkManager.Singleton.IsConnectedClient)
        {
            Social.ReportProgress(GPGSIDs.achievement_play_with_a_friend, 100, null);
        }
        if (relay.joinedFail)
        {
            relay.joinedFail = true;
            MultiSelect.SetActive(true);
            Connectingscreen.SetActive(false);
        }
        if (waitingforplayer)
        {
            ipAddressText.text = relay.joinCode;

        }
    }

    public void LoadMultMenuSellect()
    {

    }
    public void PlayButton()
    {
        mainMenu.SetActive(false);
        LevelSelect.SetActive(true);
    }
    public void showleaderbord()
    {
        if(relay.keepConnectedToGoogle)
        {
            Social.ShowLeaderboardUI();
        }
    }
    public void ExitButton()
    {
        Application.Quit();
    }
    public void EasyButton()
    {
        Admanager.instance.bannerscrip.HidBannerAd();
        SceneManager.LoadScene(1);
    }
    public void EasyButtonMulti()
    {
        Admanager.instance.bannerscrip.HidBannerAd();

        NetworkManager.Singleton.SceneManager.LoadScene("ClassicMuliPlayer", LoadSceneMode.Single); SceneManager.LoadScene(4);
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerDiss;

    }
    public void ClassicButton()
    {
        Admanager.instance.bannerscrip.HidBannerAd();

        SceneManager.LoadScene(2);
    }
    public void ClassicButtonMulti()
    {
        Admanager.instance.bannerscrip.HidBannerAd();

        NetworkManager.Singleton.SceneManager.LoadScene("ClassicMuliPlayer", LoadSceneMode.Single); SceneManager.LoadScene(5);
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerDiss;

    }
    public void ArcadeButton()
    {
        Admanager.instance.bannerscrip.HidBannerAd();

        SceneManager.LoadScene(3);
    }
    public void ArcadeButtonMulti()
    {
        Admanager.instance.bannerscrip.HidBannerAd();

        NetworkManager.Singleton.SceneManager.LoadScene("ClassicMuliPlayer", LoadSceneMode.Single); SceneManager.LoadScene(6);
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerDiss;

    }
    public void BackButton()
    {
        if (NetworkManager.Singleton.IsHost && NetworkManager.Singleton.IsClient)
        {
            relay.disconect();
        }
        mainMenu.SetActive(true);
        LevelSelect.SetActive(false);
        MultiSelect.SetActive(false);
        MultiMenuPickingScreen.SetActive(false);
    }
    public void SttingsButt()
    {
        SettingsSellect.SetActive(true);
        mainMenu.SetActive(false);

    }
    public void BackSettings()
    {
        SettingsSellect.SetActive(false);
        mainMenu.SetActive(true);
    }
    public void MusicButton()
    {
        Musicon = !Musicon;
        if (Musicon)
        {
            saveData();
            Musicondisplay.SetActive(true);
            Musicoffdisplay.SetActive(false);
            musicsource.Play();
        }
        else
        {
            saveData();
            Musicondisplay.SetActive(false);
            Musicoffdisplay.SetActive(true);
            musicsource.Stop();
        }
    }
    public void MultiMenuBut()
    {
        mainMenu.SetActive(false);
        MultiSelect.SetActive(true);
    }
    public void Hostbut()
    {
        MultiSelect.SetActive(false);
        WaitingScreen.SetActive(true);
        relay.CreatReley();
        ipAddressText.text = relay.joinCode;


        waitingforplayer = true;
    }

    public void Joinbut()
    {

        ipAddress = ip.text;
        Debug.Log(ip.text);
        ipAddress = ipAddress.Trim();
        if (ipAddress != "")
        {
            relay.joinedFail = false;
            MultiSelect.SetActive(false);
            Connectingscreen.SetActive(true);
            relay.joinReley(ipAddress);

        }
    }
    public void returnbacktomainfrommultiplayer()
    {
        mainMenu.SetActive(true);
        MultiSelect.SetActive(false);
    }
    public void saveData()
    {
        musicdata musicdata = new musicdata(Musicon);
        string json = JsonUtility.ToJson(musicdata);
        File.WriteAllText(Application.persistentDataPath + "/" + "SaveData" + ".json", json);

    }
    public void LoadData()
    {
        try
        {
            string json = null;

            using (StreamReader reader = new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
            {
                json = reader.ReadToEnd();
            }
            musicdata data = JsonUtility.FromJson<musicdata>(json);
            Musicon = data.Musicon;
            if (Musicon)
            {
                Musicondisplay.SetActive(true);
                Musicoffdisplay.SetActive(false);
            }
            else
            {
                Musicondisplay.SetActive(false);
                Musicoffdisplay.SetActive(true);
            }
        }
        catch
        {
            Musicon = true;
            musicdata musicdata = new musicdata(Musicon);
            string json = JsonUtility.ToJson(musicdata);
            File.WriteAllText(Application.persistentDataPath + "/" + "SaveData" + ".json", json);
        }
    }
}


