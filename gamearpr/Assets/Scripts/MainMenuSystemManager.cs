using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystemManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject LevelSelect;
    [SerializeField] GameObject SettingsSellect;
    [SerializeField] GameObject Musicondisplay;
    [SerializeField] GameObject Musicoffdisplay;
    AudioSource musicsource;
    //musicdata datam;
    bool Musicon;
    // Start is called before the first frame update
    void Start()
    {
        LoadData();
        musicsource = gameObject.GetComponent<AudioSource>();
        
        if (Musicon)
        {
            musicsource.Play();
        }
    }

    // Update is called once per frame
    void Update()
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
        mainMenu.SetActive(true);
        LevelSelect.SetActive(false);
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
    public void saveData()
    {
        musicdata musicdata = new musicdata(Musicon);
        string json=JsonUtility.ToJson(musicdata);
        //Debug.Log(json);
        //using (StreamWriter writer = new StreamWriter(Application.persistentDataPath+Path.AltDirectorySeparatorChar+"SaveData.json")) { 
        //writer.Write(json);
        //}
        File.WriteAllText(Application.persistentDataPath+"/"+ "SaveData"+".json", json);

    }
    public void LoadData()
    {
        string path=string.Empty;
        path = "JSONFiles/Monsters/" + "SaveData" + ".json";
        TextAsset ta=Resources.Load<TextAsset>(path);
        string json = ta.text;
        //using(StreamReader reader=new StreamReader(Application.persistentDataPath + Path.AltDirectorySeparatorChar+"SaveData.json"))
        // {
        //    json = reader.ReadToEnd();
        //}
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


