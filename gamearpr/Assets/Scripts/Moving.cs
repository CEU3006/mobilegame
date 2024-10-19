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
    float ballspeedWhilerolling = 0.5f;
    bool beenPressed = false;
    Rigidbody rb=null;
    // Start is called before the first frame update
    void Start()
    {
        Input.gyro.enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        Debug.Log("Gyro Enabled");
        correctQuaternion = Quaternion.Euler(90f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!beenPressed)
        {
            //Quaternion current = correctQuaternion* GyrotoUnity(Input.gyro.attitude);
            pos += ballspeed * Input.acceleration.normalized.x * Time.deltaTime;
            if (pos > maxvalue)
                pos = maxvalue;
            else if (pos < minvalue)
                pos = minvalue;
            transform.position = new Vector3(pos, transform.position.y, transform.position.z);
        }
        else if( beenPressed )
        {
            //Quaternion current = correctQuaternion * GyrotoUnity(Input.gyro.attitude);
            rb.velocity = new Vector3(ballspeedWhilerolling* Input.acceleration.normalized.x, rb.velocity.y, 1);
        }
        
    }
    public void Buttonclicked()
    {
        if (!beenPressed)
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
    //private Quaternion GyrotoUnity(Quaternion q)
    //{
    //    return new Quaternion(q.x, q.y, -q.z, -q.w);
    //}
}
