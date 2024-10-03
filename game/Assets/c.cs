using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class c : MonoBehaviour
{
    private Quaternion correctQuaternion;
    
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = false;
        Debug.Log("Gyro Enabled");
        correctQuaternion = Quaternion.Euler(90f, 0f, 0f); 
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = correctQuaternion* GyrotoUnity(Input.gyro.attitude);
    }
    private Quaternion GyrotoUnity(Quaternion q)
    {
        return new Quaternion(q.x,q.y,-q.z,-q.w);
    }
}
