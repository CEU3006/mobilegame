using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SystemManager : MonoBehaviour
{
    public List<Vector2> listOfscores = new List<Vector2>();
    bool secondPart = false;
    public Vector2 currentscore = Vector2.zero;
    List<GameObject> pinsDelete= new List<GameObject>();
    [SerializeField]public List< GameObject> textMeshPros = new List<GameObject>();
    [SerializeField] public GameObject ball;
    [SerializeField] public GameObject button;
    [SerializeField] public GameObject exitBut;
    AudioSource musicsource;

    bool Musicon;
    private void Start()
    {
        LoadData();
        musicsource = gameObject.GetComponent<AudioSource>();

        if (Musicon)
        {
            musicsource.Play();
        }
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

