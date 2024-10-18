using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    }
    public void ClassicButton()
    {

    }
    public void ArcadeButton()
    {

    }
    public void BackButton()
    {
        mainMenu.SetActive(true);
        LevelSelect.SetActive(false);
    }
}
