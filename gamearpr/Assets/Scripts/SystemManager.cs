using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Relay;
using Unity.Netcode;
using BowlingARGame;

public class SystemManager : MonoBehaviour
{
    public List<Vector2> listOfscores = new List<Vector2>();
    bool secondPart = false;
    public Vector2 currentscore = Vector2.zero;
    List<GameObject> pinsDelete = new List<GameObject>();
    [SerializeField] public List<GameObject> textMeshPros = new List<GameObject>();
    [SerializeField] public GameObject ball;
    [SerializeField] public GameObject button;
    [SerializeField] public GameObject exitBut;
    AudioSource musicsource;
    bool Musicon;
    TestRelay relay;
    private void Start()
    {
        LoadData();
        musicsource = gameObject.GetComponent<AudioSource>();
        GameObject network = GameObject.Find("Network");
        relay = network.GetComponent<TestRelay>();

        if (Musicon)
        {
            musicsource.Play();
        }
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void ballAtEnd()
    {
        GameObject[] pins = GameObject.FindGameObjectsWithTag("pin");
        int pinsnocked = 0;
        foreach (GameObject pin in pins)
        {
            if (pin.transform.rotation.eulerAngles.x > 50 || pin.transform.rotation.eulerAngles.x < -50 || pin.transform.rotation.eulerAngles.z > 50 || pin.transform.rotation.eulerAngles.z < -50)
            {
                pinsnocked++;
                pin.gameObject.SetActive(false);
                pinsDelete.Add(pin);
            }
        }
        if (pinsnocked > 0)
        {
            if (relay.keepConnectedToGoogle)
            {
                Social.ReportProgress(GPGSIDs.achievement_score, 100, null);
            }
        }
        if (!secondPart)
        {
            secondPart = true;
            if (pinsnocked == 9)
            {
                if (relay.keepConnectedToGoogle)
                {
                    Social.ReportProgress(GPGSIDs.achievement_strike, 100, null);
                }
                pinsnocked = 10;
                currentscore.x = pinsnocked;
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
                currentscore.x = pinsnocked;

                textMeshPros[listOfscores.Count * 2].GetComponent<TextMeshProUGUI>().text = "" + currentscore.x;
            }
        }
        else
        {
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
            currentscore.y = pinsnocked;
            listOfscores.Add(currentscore);
            if (currentscore.y + currentscore.x == 9&& relay.keepConnectedToGoogle)
            {
                Social.ReportProgress(GPGSIDs.achievement_strike, 100, null);
            }
            textMeshPros[(listOfscores.Count * 2) - 1].GetComponent<TextMeshProUGUI>().text = "" + currentscore.y;
            if (listOfscores.Count >= 4)
            {
                endOfGame();
            }
        }
    }
    private void Leaderbordupdate(bool success)
    {
        if (success) { Debug.Log("update"); }
    }
    void endOfGame()
    {
        float total = 0;
        for (int i = 0; listOfscores.Count > i; i++)
        {
            total += listOfscores[i].x + listOfscores[i].y;
        }
        textMeshPros[(listOfscores.Count * 2)].GetComponent<TextMeshProUGUI>().text = "Total:" + total;
        if (relay.keepConnectedToGoogle)
        {
            PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;
            platform.IncrementAchievement(GPGSIDs.achievement_bowling_champion, 1,
                (bool success) =>
                {
                    if (!success)
                    {
                        Debug.Log("Failed to increment achievement progress.");
                    }
                }
            );

            if (SceneManager.GetActiveScene().name == "Easy")
            {
                Social.ReportScore((int)total, GPGSIDs.leaderboard_easy_mode_leaderboard, Leaderbordupdate);
            }
            else if (SceneManager.GetActiveScene().name == "Classic")
            {
                Social.ReportScore((int)total, GPGSIDs.leaderboard_classic_mode_leaderboard, Leaderbordupdate);
                Social.ReportProgress(GPGSIDs.achievement_finish_a_game_of_classic, 100, null);

            }
            else if (SceneManager.GetActiveScene().name == "Arcade")
            {
                Social.ReportProgress(GPGSIDs.achievement_arcade, 100, null);
                Social.ReportScore((int)total, GPGSIDs.leaderboard_arcade_mode_leaderboard, Leaderbordupdate);
            }
        }
        ball.SetActive(false);
        button.SetActive(false);
        exitBut.SetActive(true);
    }
    public void BackToMainMenu()
    {
        Admanager.instance.addscrip.ShowAd();
        Admanager.instance.bannerscrip.ShowBannerAd();
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

