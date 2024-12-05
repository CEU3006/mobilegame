using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class SystemManagerMulti : MonoBehaviour
{
    public List<Vector2> listOfscores = new List<Vector2>();
    bool secondPart = false;
    public Vector2 currentscore = Vector2.zero;
    int currentEntryOfEnemyScore = 0;
    List<GameObject> pinsDelete= new List<GameObject>();
    [SerializeField]public List< GameObject> textMeshPros = new List<GameObject>();
    [SerializeField] public List<GameObject> textMeshProsForEnemy = new List<GameObject>();

    [SerializeField] public GameObject ball;
    [SerializeField] public GameObject button;
    [SerializeField] public GameObject exitBut;
    SendDataNetcode getDataAndSend;
    public GameObject net;
    TestRelay netCode;
    bool playerOneTurn=true;
    int playerNum;
    AudioSource musicsource;
    [SerializeField]GameObject playerprefab;
    [SerializeField] GameObject conePreFab;
    [SerializeField] Vector3[] ConeLocs;
    bool Musicon;
    public ARSession arSession;
    bool doonce=true;
    [SerializeField]GameObject sesionAR;
    Moving ballscrip;
    private void Start()
    {
        if (!NetworkManager.Singleton.IsHost) 
        {
            sesionAR.SetActive(true);
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
            Vector3 ballspawn = new Vector3(0, 30f, 4.54f);
            GameObject ball_= Instantiate(playerprefab);
            ball_.transform.position = ballspawn;
            NetworkObject networkObject_ = ball_.GetComponent<NetworkObject>();
            ulong clientid = NetworkManager.Singleton.ConnectedClientsList[1].ClientId;
            networkObject_.SpawnAsPlayerObject(clientid);
            ballscrip = ball.GetComponent<Moving>();
            getDataAndSend = ball.GetComponent<SendDataNetcode>();
            for (int i=0; i<ConeLocs.Count();i++)
            {
                GameObject cone = Instantiate(conePreFab);
                cone.transform.position = ConeLocs[i];
                NetworkObject networkObjectCone = cone.GetComponent<NetworkObject>();
                networkObjectCone.Spawn();
            }
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
                    ballscrip = ball.GetComponent<Moving>();
                    getDataAndSend = ball.GetComponent<SendDataNetcode>();


                }
            }
        }
        

    }
    private void Update()
    {
        if (doonce && NetworkManager.Singleton.IsHost && IsPlayerSceneLoaded())
        {
            Debug.Log("done");
            doonce=false;
            sesionAR.SetActive(true);
        }
    }
    private bool IsPlayerSceneLoaded()
    {
        GameObject playerObject = NetworkManager.Singleton.ConnectedClientsList[1].PlayerObject.gameObject;

        if (playerObject.activeInHierarchy)
        {
            return true;
        }
        return false;
    }
    public void ballAtEnd(GameObject ball_)
    {
        Moving moving2ndBall = ball_.GetComponent<Moving>();
        Debug.Log("here");

        if (playerOneTurn && playerNum == 1 && moving2ndBall.playerid==ballscrip.playerid)
        {
            Debug.Log("here");

            if (ballscrip.firstTurn)
            {
                Debug.Log("here");
                ballscrip.Reset();
                playrturn();
               
                getDataAndSend.ballAtEndSendToClientRpc();

               
            }
            else
            {
                Debug.Log("2nd");
                ballscrip.ResetMulti();
                playrturn();

                StartCoroutine(DoNextFrameHostOrPinsWillExplode());
                Debug.Log("cheking here");


               
                

            }

        }
        else if (!playerOneTurn && playerNum == 2 && moving2ndBall.playerid == ballscrip.playerid)
        {
            if (ballscrip.firstTurn)
            {
                ballscrip.Reset();
                playrturn();
                Debug.Log("here");
                
                getDataAndSend.ballAtEndSendToSeverRpc();

            }
            else
            {
                ballscrip.ResetMulti();
                playrturn();
                playerOneTurn = true;
                getDataAndSend.ballAtEndSendToSeverRpc();

                Debug.Log("here");

            }
        }
    }
    IEnumerator DoNextFrameHostOrPinsWillExplode()
    {
        // Wait until the next frame
        Debug.Log("this");

        yield return null;
        GameObject[] pins2 = GameObject.FindGameObjectsWithTag("pin");
        Debug.Log("this");
        foreach (GameObject pin in pins2)
        {
            Debug.Log("works maybe");
            
            
            NetworkObject cone = pin.GetComponent<NetworkObject>();
            cone.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsList[1].ClientId);
            yield return null;

        }
        yield return null;

        getDataAndSend.ballAtEndSendToClientRpc();
        yield return null;

        playerOneTurn = false;


    }
   

    bool otherPlayerSecoundTurn=false;
    public void UpdateFromOther()
    {
         if (otherPlayerSecoundTurn)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GameObject[] pins2 = GameObject.FindGameObjectsWithTag("pin");
                Debug.Log("works maybe");
                foreach (GameObject pin in pins2)
                {
                    NetworkObject cone = pin.GetComponent<NetworkObject>();
                    cone.ChangeOwnership(NetworkManager.Singleton.ConnectedClientsList[0].ClientId);
                   
                }

            }
            GameObject[] pins = GameObject.FindGameObjectsWithTag("pin");
            Debug.Log("works maybe");
            foreach (GameObject pin in pins)
            {
                pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                pin_script.Reset();


            }

            Debug.Log("this is working");
            ballscrip.Reset();
            ballscrip.firstTurn=true; 
            otherPlayerSecoundTurn = false;
            Debug.Log("here");
            playerOneTurn = !playerOneTurn;
        }
        else
        {
            otherPlayerSecoundTurn=true;
        }
    }
  
    void playrturn()
{
    GameObject[] pins = GameObject.FindGameObjectsWithTag("pin");
    int pinsnocked = 0;
    foreach (GameObject pin in pins)
    {
        if (pin.transform.rotation.eulerAngles.x > 50 || pin.transform.rotation.eulerAngles.x < -50 || pin.transform.rotation.eulerAngles.z > 50 || pin.transform.rotation.eulerAngles.z < -50)
        {
            pinsnocked++;
                pin.transform.position = new Vector3(10, -10, 10);
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
                textMeshPros[listOfscores.Count * 2].GetComponent<TextMeshProUGUI>().text = "" + currentscore.x;
                if(NetworkManager.Singleton.IsHost)
                {
                    getDataAndSend.SendDataToClientRpc((int)currentscore.x);
                }
                else
                {
                    getDataAndSend.SendDataToHostRpc((int)currentscore.x);
                }
                secondPart = false;
                foreach (GameObject pin in pinsDelete)
                {
                    pin.gameObject.SetActive(true);
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
                currentscore.x = pinsnocked;

                textMeshPros[listOfscores.Count * 2].GetComponent<TextMeshProUGUI>().text = "" + currentscore.x;
                if (NetworkManager.Singleton.IsHost)
                {
                    getDataAndSend.SendDataToClientRpc((int)currentscore.x);
                }
                else
                {
                    getDataAndSend.SendDataToHostRpc((int)currentscore.x);
                }
            }
        }
        else
        {
            secondPart = false;
            foreach (GameObject pin in pinsDelete)
            {
                pin.gameObject.SetActive(true);
                
            }
            pinsDelete.Clear();
            currentscore.y = pinsnocked;
            listOfscores.Add(currentscore);
            textMeshPros[(listOfscores.Count * 2) - 1].GetComponent<TextMeshProUGUI>().text = "" + currentscore.y;
            if (NetworkManager.Singleton.IsHost)
            {
                getDataAndSend.SendDataToClientRpc((int)currentscore.y);
            }
            else
            {
                getDataAndSend.SendDataToHostRpc((int)currentscore.y);
            }
            if (listOfscores.Count >= 4)
            {
                endOfGame();
            }
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
        if (NetworkManager.Singleton.IsHost)
        {
            getDataAndSend.SendDataToClientRpc((int)total);
        }
        else
        {
            getDataAndSend.SendDataToHostRpc((int)total);
        }
        ball.SetActive(false);
        button.SetActive(false);
        exitBut.SetActive(true);
    }
    public void BackToMainMenu()
    {
        Admanager.instance.addscrip.ShowAd();
        SceneManager.LoadScene(0);
    }
    public void addEnemyScore(int a)
    {
        if(currentEntryOfEnemyScore==8)
        {
            textMeshProsForEnemy[currentEntryOfEnemyScore].GetComponent<TextMeshProUGUI>().text = "Total" + a;

        }
        else
        {
            textMeshProsForEnemy[currentEntryOfEnemyScore].GetComponent<TextMeshProUGUI>().text = "" + a;

        }
        currentEntryOfEnemyScore++;
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

