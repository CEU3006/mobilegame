using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pinscript : MonoBehaviour
{
    public Vector3 startingpos;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        startingpos= transform.position;
        rb = GetComponent<Rigidbody>();
    }
    public void Reset()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.eulerAngles = Vector3.zero;
        transform.position = startingpos;
    }
}
