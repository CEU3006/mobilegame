using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballscipt : MonoBehaviour
{
    // Start is called before the first frame update
    private float min_x = -2.9f, max_x = 2.9f;
    public float move_speed = 2f;
    private Rigidbody2D ballBody;
    private bool canMove;
     void Awake()
    {
        ballBody =GetComponent<Rigidbody2D>();
        ballBody.gravityScale = 0f;

    }
    void Start()
    {
        canMove = true;
        if (Random.Range(0,2)>0)
        {
            move_speed *= -1;
        }
        GameplayScript.instance.currentBall = this;
    }
    void MoveBall()
    {
        if (canMove)
        {
            Vector3 temp = transform.position;
            temp.x += move_speed * Time.deltaTime;
            if (temp.x < min_x || temp.x > max_x)
            {
                move_speed *= -1f;
            }
            transform.position = temp;
        }
    }
    public void Kick()
    {
        canMove=false;
        ballBody.gravityScale=Random.Range(-4,-2);
    }
    // Update is called once per frame
    void Update()
    {
        MoveBall();
    }
    void Goal()
    {
        GameplayScript.instance.SpawnNewBall();
    }
    private void Resetgame()
    {
        GameplayScript.instance.RestartGame();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Goal")
        {
            Goal();
        }
        else
        {
            Resetgame();
        }
    }

}
