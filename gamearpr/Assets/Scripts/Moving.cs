using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Moving : MonoBehaviour
{
    float maxvalue = 1;
    float minvalue = -1;
    float ballspeed = 1f;
    float pos = 0;
    float ballspeedWhilerolling = 0.5f;
    bool beenPressed = false;
    bool turn=false;
    Rigidbody rb=null;
    bool inMenu= true;

    public bool firstTurn=true;
    [SerializeField] public int playerid;
    NetworkObject networkObject = null;
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost)
        {
            doonce = false;
            networkObject = gameObject.GetComponent<NetworkObject>();

            if (networkObject.IsOwner)
            {

                if (NetworkManager.Singleton.IsHost)
                {
                    turn = true;
                    playerid = 1;
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = Vector3.zero;
                    //transform.eulerAngles = new Vector3(-180, 0, 0);
                    //transform.position = new Vector3(0, 0.11f, 4.54f);
                }
                else
                {
                    playerid = 2;

                }
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    inMenu = false;
                }
                else
                {
                    inMenu = true;
                }
            }
        }
        else
        {
            inMenu = false;
            turn = true;
        }

    }

    bool doonce = true;
    // Update is called once per frame
    void Update()
    {
        if(!beenPressed&& !inMenu&& (networkObject == null||networkObject.IsOwner)&&turn)
        {
            pos += ballspeed * Input.acceleration.normalized.x * Time.deltaTime;
            if (pos > maxvalue)
                pos = maxvalue;
            else if (pos < minvalue)
                pos = minvalue;
            transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
        else if( beenPressed&& !inMenu&& (networkObject == null || networkObject.IsOwner)&& turn)
        {
            float numz = Input.acceleration.normalized.z;
            if (numz > 0.4)
            {
                numz = 0.4f;
            }
            if (numz < -0.4)
            {
                numz = -0.4f;
            }
            rb.velocity = new Vector3(ballspeedWhilerolling* Input.acceleration.normalized.x, rb.velocity.y, ballspeed * numz + 1);
        }
        
    }
    public void Buttonclicked()
    {
        if (!beenPressed&& (networkObject==null||networkObject.IsOwner)&& turn)
        {
            beenPressed=true;
            rb.useGravity=true;
        }
    }
    public void Reset()
    {
        firstTurn=!firstTurn;
        pos = 0;
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        transform.eulerAngles = new Vector3(-180, 0, 0);
        transform.position = new Vector3(0, -0.11f, 4.54f);
        beenPressed = false;
        turn = true;
        
    }
    public void ResetMulti()
    {
        pos = 0;
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        transform.eulerAngles = new Vector3(-180, 0, 0);
        transform.position = new Vector3(0, 30f, 4.54f);
        beenPressed = true;
        turn = false;

    }
}
