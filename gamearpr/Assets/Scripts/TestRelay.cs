using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class TestRelay : MonoBehaviour
{
    // Start is called before the first frame update
    public string joinCode;
    public bool keepConnectedToGoogle=false;
    private async void  Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            AuthenticationService.Instance.SignedIn += () => {
                Debug.Log("signed in" + AuthenticationService.Instance.PlayerId);
            };
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    public  async void CreatReley()
    {
        try
        {
            Allocation allocation=await RelayService.Instance.CreateAllocationAsync(1);
            joinCode= await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(joinCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(allocation.RelayServer.IpV4, (ushort)allocation.RelayServer.Port, allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData);
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.ActiveSceneSynchronizationEnabled = true;
        }
        catch(RelayServiceException e)
        {
            joinCode = "fail";
            Debug.Log(e);
        }
    }
    public bool joinedFail=false;
    public async void joinReley(string joincode)
    {
        try
        {
            Debug.Log("TRY");
            JoinAllocation joinAllocation= await RelayService.Instance.JoinAllocationAsync(joincode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(joinAllocation.RelayServer.IpV4, 
                (ushort)joinAllocation.RelayServer.Port, 
                joinAllocation.AllocationIdBytes, 
                joinAllocation.Key, 
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            joinedFail = true;
            Debug.Log(e);
        }

    }
    public void disconect()
    {
        NetworkManager.Singleton.Shutdown();
    }
    public bool isHost()
    {
        return NetworkManager.Singleton.IsHost;
    }
}
