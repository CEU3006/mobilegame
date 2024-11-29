using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class buttonRollMulti : MonoBehaviour
{
    GameObject ball;
    Moving ballscrip;
    bool doonce=true;
    private void Update()
    {
        if (doonce)
        {
            GameObject[] balls = GameObject.FindGameObjectsWithTag("ball");
            foreach (GameObject ball_ in balls)
            {
                NetworkObject ballnet = ball_.GetComponent<NetworkObject>();
                if (ballnet.IsLocalPlayer)
                {
                    ball = ball_;
                    ballscrip = ball.GetComponent<Moving>();
                }
            }
        }
    }
    public void onClickofRoll()
    {
        ballscrip.Buttonclicked();
    }
}
