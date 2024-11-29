using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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
    Rigidbody rb=null;
    bool inMenu= true;
    [SerializeField] ulong playerid;
    NetworkObject networkObject = null;
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        if (NetworkManager.Singleton.IsConnectedClient || NetworkManager.Singleton.IsHost)
        {
            networkObject=gameObject.GetComponent<NetworkObject>();
            if (networkObject.IsOwner)
            {

                if (NetworkManager.Singleton.IsHost)
                {
                    //rb.velocity = new Vector3(0, 0, 0);
                    //rb.angularVelocity = Vector3.zero;
                    //transform.eulerAngles = new Vector3(-180, 0, 0);
                    //transform.position = new Vector3(0, 0.11f, 4.54f);
                }
                if (SceneManager.GetActiveScene().name== "ClassicMuliPlayer")
                {
                    //inMenu=false;
                }
                else
                {
                    //inMenu = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!beenPressed&& !inMenu&& networkObject.IsOwner)
        {
            pos += ballspeed * Input.acceleration.normalized.x * Time.deltaTime;
            if (pos > maxvalue)
                pos = maxvalue;
            else if (pos < minvalue)
                pos = minvalue;
            transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
        else if( beenPressed&& !inMenu&& networkObject.IsOwner)
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
        if (!beenPressed&& networkObject.IsOwner)
        {
            beenPressed=true;
            rb.useGravity=true;
        }
    }
    public void Reset()
    {
        pos = 0;
        rb.useGravity = false;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        transform.eulerAngles = new Vector3(-180, 0, 0);
        transform.position = new Vector3(0, -0.11f, 4.54f);
        beenPressed = false;
    }
}
