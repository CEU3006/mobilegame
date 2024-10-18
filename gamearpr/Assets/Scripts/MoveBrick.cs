using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBrick : MonoBehaviour
{
    bool moveRight = true;
    float blockSpeed = 1.75f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (moveRight)
        {
            gameObject.transform.position = new Vector3(transform.position.x+blockSpeed*Time.deltaTime,transform.position.y,transform.position.z);
        }
        else if (!moveRight)
        {
            gameObject.transform.position = new Vector3(transform.position.x - blockSpeed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        if(gameObject.transform.position.x >=1&&moveRight) 
        {
            moveRight = false;
        }
        if (gameObject.transform.position.x <= -1 && !moveRight)
        {
            moveRight = true;
        }
    }
}
