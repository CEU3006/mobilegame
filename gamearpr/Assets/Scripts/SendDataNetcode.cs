using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SendDataNetcode : NetworkBehaviour
{
    private GameObject systemManager;

    void Awake()
    {
        systemManager = GameObject.Find("SystemManager");
    }
    [Rpc(SendTo.Server)]
    public void ballAtEndSendToSeverRpc()
    {
        atEndSentToSever();
    }
    void atEndSentToSever()
    {
        SystemManagerMulti multi = systemManager.GetComponent<SystemManagerMulti>();
        multi.UpdateFromOther();


    }
    [Rpc(SendTo.NotServer)]
    public void ballAtEndSendToClientRpc()
    {
        atEndSentToClient();
    }
    void atEndSentToClient()
    {
        SystemManagerMulti multi = systemManager.GetComponent<SystemManagerMulti>();
        multi.UpdateFromOther();
    }
    [Rpc(SendTo.NotServer)]
    public void SendDataToClientRpc(int a)
    {
        SentDataToClient(a);
    }
    public void SentDataToClient(int a)
    {
        SystemManagerMulti multi = systemManager.GetComponent<SystemManagerMulti>();
        multi.addEnemyScore(a);
    }
    [Rpc(SendTo.Server)]

    public void SendDataToHostRpc(int a)
    {
        SentDataToHost(a);
    }
    public void SentDataToHost(int a)
    {
        SystemManagerMulti multi = systemManager.GetComponent<SystemManagerMulti>();
        multi.addEnemyScore(a);
    }
}
