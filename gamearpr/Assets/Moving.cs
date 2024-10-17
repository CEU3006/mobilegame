using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : MonoBehaviour
{
    private Quaternion correctQuaternion;
    float maxvalue = 1;
    float minvalue = -1;
    float ballspeed = 1f;
    float pos = 0;
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        Debug.Log("Gyro Enabled");
        correctQuaternion = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        pos += ballspeed * Input.gyro.attitude.z * Time.deltaTime;
        if (pos > maxvalue)
            pos = maxvalue;
        else if (pos < minvalue)
            pos = minvalue;
        transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        //transform.rotation = correctQuaternion* GyrotoUnity(Input.gyro.attitude);
    }
    //private Quaternion GyrotoUnity(Quaternion q)
    //{
    //    return new Quaternion(q.x, q.y, -q.z, -q.w);
    //}
}
