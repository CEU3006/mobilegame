using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SystemManagerMulti : MonoBehaviour
{
    public List<Vector2> listOfscores = new List<Vector2>();
    bool secondPart = false;
    public Vector2 currentscore = Vector2.zero;
    List<GameObject> pinsDelete= new List<GameObject>();
    [SerializeField]public List< GameObject> textMeshPros = new List<GameObject>();
    [SerializeField] public GameObject ball;
    [SerializeField] public GameObject button;
    [SerializeField] public GameObject exitBut;
    public GameObject net;
    TestRelay netCode;
    bool playerOneTurn;
    int playerNum;
    AudioSource musicsource;
    [SerializeField]GameObject playerprefab;
    bool Musicon;
    public ARSession arSession;
    bool doonce=true;
    [SerializeField]GameObject sesionAR;
    private void Start()
    {
        //if (arSession == null)
       // {
       //     arSession = FindObjectOfType<ARSession>();
       // }

        // If no ARSession is found, log an error and return
      //  if (arSession == null)
      //  {
      //      Debug.LogError("ARSession not found in the scene.");
      //      return;
      //  }
        if (!NetworkManager.Singleton.IsHost) 
        {
            sesionAR.SetActive(true);
            //arSession.enabled = true;  // Enable AR session for clients
            Debug.Log("AR session enabled for client.");
        }
        LoadData();
        musicsource = gameObject.GetComponent<AudioSource>();

        if (Musicon)
        {
            musicsource.Play();
        }
        net = GameObject.Find("Network");
        netCode= net.GetComponent<TestRelay>();
        if (netCode.isHost())
        {
            playerNum = 1;
        }
        else
        {
            playerNum = 2;
        }
        
        if (NetworkManager.Singleton.IsHost)
        {
            ball = Instantiate(playerprefab);
            NetworkObject networkObject = ball.GetComponent<NetworkObject>();
            networkObject.SpawnAsPlayerObject(NetworkManager.Singleton.ConnectedClientsList[0].ClientId);
            //Vector3 ballspawn = new Vector3(0, 30f, 4.54f);
            GameObject ball_= Instantiate(playerprefab);
            //ball_.transform.position = ballspawn;
            NetworkObject networkObject_ = ball_.GetComponent<NetworkObject>();
            networkObject_.SpawnAsPlayerObject(NetworkManager.Singleton.ConnectedClientsList[1].ClientId);

        }
        else
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
            foreach (GameObject ball_ in balls)
            {
                NetworkObject ballnet = ball_.GetComponent<NetworkObject>();
                if(ballnet.IsLocalPlayer)
                {
                    ball = ball_;
                }
            }
        }
        

    }
    private void Update()
    {
        if (doonce && NetworkManager.Singleton.IsHost && IsPlayerSceneLoaded(NetworkManager.Singleton.ConnectedClientsList[1].ClientId))
        {
            Debug.Log("done");
            doonce=false;
            //arSession.Reset();
            //arSession.enabled = true;  // Enable the session if it was disabled.
            sesionAR.SetActive(true);

            //arSessionstart
        }
    }
    private bool IsPlayerSceneLoaded(ulong clientId)
    {
        foreach (var client in NetworkManager.Singleton.ConnectedClientsList)
        {
            // Check if the client has a spawned object (NetworkObject) in the scene.
            if (client.ClientId == clientId)
            {
                // Get the player object associated with the client
                GameObject playerObject = client.PlayerObject.gameObject;

                // If the player object is not null and active, they are loaded into the scene
                if (playerObject != null && playerObject.activeInHierarchy)
                {
                    return true; // Player is loaded into the scene
                }
            }
        }
        return false;
    }
    public void ballAtEnd()
    {
        GameObject[] pins = GameObject.FindGameObjectsWithTag("pin");
        int pinsnocked = 0;
        foreach (GameObject pin in pins)
        {
            if(pin.transform.rotation.eulerAngles.x > 50|| pin.transform.rotation.eulerAngles.x < -50|| pin.transform.rotation.eulerAngles.z > 50 || pin.transform.rotation.eulerAngles.z < -50)
            {
                pinsnocked++;
                pin.gameObject.SetActive(false);
                pinsDelete.Add(pin);
            }
        }
        if (!secondPart)
        {
            secondPart = true;
            if (pinsnocked == 9)
            {
                pinsnocked = 10;
                currentscore.x = pinsnocked;
                //Debug.Log(listOfscores.Count);
                textMeshPros[listOfscores.Count * 2].GetComponent<TextMeshProUGUI>().text = "" + currentscore.x;
                secondPart = false;
                foreach (GameObject pin in pins)
                {
                    pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                    pin_script.Reset();
                }
                foreach (GameObject pin in pinsDelete)
                {
                    pin.gameObject.SetActive(true);
                    pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                    pin_script.Reset();
                }
                pinsDelete.Clear();
                currentscore.x = pinsnocked;
                currentscore.y = 0;
                listOfscores.Add(currentscore);
                if (listOfscores.Count >= 4)
                {
                    endOfGame();
                }
            }
            else
            {
                //Debug.Log(listOfscores.Count);
                currentscore.x = pinsnocked;

                textMeshPros[listOfscores.Count * 2].GetComponent<TextMeshProUGUI>().text = "" + currentscore.x;
            } 
        }
        else
        {
            secondPart= false;
            foreach (GameObject pin in pins)
            {
                pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                pin_script.Reset();
            }
            foreach (GameObject pin in pinsDelete)
            {
                pin.gameObject.SetActive(true);
                pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                pin_script.Reset();
            }
            pinsDelete.Clear();
            currentscore.y = pinsnocked;
            listOfscores.Add(currentscore);
            textMeshPros[(listOfscores.Count * 2)-1].GetComponent<TextMeshProUGUI>().text = "" + currentscore.y;
            if(listOfscores.Count>=4)
            {
                endOfGame();
            }
            //for (int i = 0; i < listOfscores.Count; i++)
            //{
            //    Debug.Log(listOfscores[i]);
            //}
        }
    }
    void endOfGame()
    {
        float total = 0;
        for(int i = 0; listOfscores.Count>i;i++)
        {
            total += listOfscores[i].x + listOfscores[i].y;
        }
        textMeshPros[(listOfscores.Count * 2)].GetComponent<TextMeshProUGUI>().text = "Total:" + total;
        ball.SetActive(false);
        button.SetActive(false);
        exitBut.SetActive(true);
    }
    public void BackToMainMenu()
    {
        Admanager.instance.addscrip.ShowAd();
        SceneManager.LoadScene(0);
    }
    public void AlowExit()
    {
        exitBut.SetActive(!exitBut.activeSelf);
    }
    public void LoadData()
    {
        string json = null;
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json"))
        {
            json = reader.ReadToEnd();
        }
        musicdata data = JsonUtility.FromJson<musicdata>(json);
        Musicon = data.Musicon;
        
    }
}

