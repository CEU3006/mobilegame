using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ResetAreaTrigger : MonoBehaviour
{

    [SerializeField] Moving movingscript;
    [SerializeField] SystemManager systemManagerscript;
    [SerializeField] SystemManagerMulti systemmanmulti;
    bool isMulti = false;
    private void Start()
    {
        if(NetworkManager.Singleton.IsHost||NetworkManager.Singleton.IsConnectedClient)
        {
            isMulti = true;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("This is not");
        if (other.gameObject.tag == "ball"&&!isMulti)
        {
            Debug.Log("This is prob");
            systemManagerscript.ballAtEnd();
            movingscript.Reset();
        }
        else if(other.gameObject.tag == "ball" && isMulti)
        {
            Debug.Log("This is prob");
            systemmanmulti.ballAtEnd(other.gameObject);
        }
    }
}
