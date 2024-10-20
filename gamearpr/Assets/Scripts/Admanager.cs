using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Admanager : MonoBehaviour
{
    public AddScript addscrip;
    public Initaliseadds initaliseadds;
    public static Admanager instance
    {
        get;private set;
    }
    private void Awake()
    {
        if (instance != null&& instance!=this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        addscrip.LoadAdd();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
