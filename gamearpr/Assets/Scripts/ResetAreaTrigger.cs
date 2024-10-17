using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAreaTrigger : MonoBehaviour
{

    [SerializeField] Moving movingscript;
    [SerializeField] SystemManager systemManagerscript;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ball")
        {
            systemManagerscript.ballAtEnd();
            movingscript.Reset();
        }
    }
}
