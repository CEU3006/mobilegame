using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameplayScript instance;
    public Ballspawn ballspawn;
    [HideInInspector]
    public  Ballscipt currentBall ;
    private int score=0;
    [SerializeField] Text t;
    void Awake()
    {
        if(instance == null)
            instance = this;
    }
    void Start()
    {
        ballspawn.spawnball();
    }
    

    // Update is called once per frame
    void Update()
    {
        DetectInput();
    }
    void DetectInput () 
    {
        if(Input.GetMouseButton(0))
        {
            currentBall.Kick();
        }
    }
    public void SpawnNewBall()
    {
        Invoke("NewBall", 2f);
        IncreaseScore();
    }
    void NewBall()
    {
        ballspawn.spawnball();

    }
    public void IncreaseScore()
    {
        score++;

    }
    void UpdateScoreUi()
    {
        t.text = score.ToString();
    }
    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
        UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


}
