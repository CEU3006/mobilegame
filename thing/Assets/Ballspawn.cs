using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballspawn : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ball;
    void Start()
    {
        
    }
    public void spawnball()
    {
        GameObject ball_obj = Instantiate(ball);
        ball_obj.transform.position = transform.position;
        Ballscipt ballscrip =ball_obj.GetComponent<Ballscipt>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
