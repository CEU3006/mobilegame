using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjects : MonoBehaviour
{
    Rigidbody rb;
    Vector3 rannum;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rannum = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(-1, 0, 0);
        rb.angularVelocity = rannum;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="MainMenucoll")
        {
            transform.position=new Vector3(4,transform.position.y,transform.position.z);
        }
    }

}
