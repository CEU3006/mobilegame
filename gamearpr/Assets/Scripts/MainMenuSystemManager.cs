using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystemManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject LevelSelect;
    // Start is called before the first frame update
    void Start()
    {
        
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
}
