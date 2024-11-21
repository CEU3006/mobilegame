using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;

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
    [SerializeField] TextMeshProUGUI test1;
    [SerializeField] TextMeshProUGUI test2;
    TestRelay relay;
    NetworkManager netman;
    AudioSource musicsource;
    public int playersConnected;
    bool waitingforplayer=false;
    //musicdata datam;
    bool Musicon;
    // Start is called before the first frame update
    void Start()
    {
        
        //ipAddress = "0.0.0.0";
        //SetIpAddress();

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
            // Subscribe to the client connected callback
            NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerJoin;
        }
        relay=network.GetComponent<TestRelay>();
    }
    void OnPlayerJoin(ulong clientId)
    {
        Debug.Log($"Player with Client ID {clientId} has joined the game.");
    }
    //public void SetIpAddress()
   // {
   //     //string cleanedIP = System.Text.RegularExpressions.Regex.Replace(ipAddress, "[^0-9.]", "");
   //     transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
   //     transport.ConnectionData.Address = ipAddress;
   // }
    // Update is called once per frame
    void Update()
    {
        if(NetworkManager.Singleton.IsHost)
        {
            test1.text=""+NetworkManager.Singleton.ConnectedClients.Count;
            if(NetworkManager.Singleton.ConnectedClients.Count==2&& waitingforplayer)
            {
                waitingforplayer = false;
                WaitingScreen.SetActive(false);
                MultiMenuPickingScreen.SetActive(true);

            }
        }
        if (NetworkManager.Singleton.IsClient)
        {
            test2.text = "" + NetworkManager.Singleton.IsConnectedClient;
            
        }

       if (waitingforplayer)
        {
            ipAddressText.text = relay.joinCode;
            //Debug.Log((string)relay.joinCode);


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
    public void ExitButton()
    {
        Application.Quit();
    }
    public void EasyButton()
    {
        SceneManager.LoadScene(1);
    }
    public void EasyButtonMulti()
    {
        SceneManager.LoadScene(4);
    }
    public void ClassicButton()
    {
        SceneManager.LoadScene(2);
    }
    public void ArcadeButton()
    {
        SceneManager.LoadScene(3);
    }
    public void BackButton()
    {
        if (NetworkManager.Singleton.IsHost&& NetworkManager.Singleton.IsClient)
        {
            relay.disconect();
        }
        mainMenu.SetActive(true);
        LevelSelect.SetActive(false);
        MultiSelect.SetActive(false);
        MultiMenuPickingScreen.SetActive(false );
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
        if(Musicon)
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
    /*public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString();
                return ip.ToString();

            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
        /*
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddressText.text = ip.ToString();
                ipAddress = ip.ToString(); 
                return ip.ToString();

            } //if
        }
        /
    }
    */

    public void Joinbut()
    {

        ipAddress = ip.text;
        Debug.Log(ip.text);
        relay.joinReley(ipAddress);
        MultiSelect.SetActive(false);
        Connectingscreen.SetActive(true);

    }
    public void returnbacktomainfrommultiplayer()
    {
        mainMenu.SetActive(true);
        MultiSelect.SetActive(false);
    }
    public void saveData()
    {
        musicdata musicdata = new musicdata(Musicon);
        string json=JsonUtility.ToJson(musicdata);
        File.WriteAllText(Application.persistentDataPath+"/"+ "SaveData"+".json", json);

    }
    public void LoadData()
    {
        string json = null;
        using (StreamReader reader=new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar+"SaveData.json"))
         {
            json = reader.ReadToEnd();
        }
        musicdata data =JsonUtility.FromJson<musicdata>(json);
        Musicon=data.Musicon;
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
}


