using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    public List<Vector2> listOfscores = new List<Vector2>();
    bool secondPart = false;
    public Vector2 currentscore = Vector2.zero;
    List<GameObject> pinsDelete= new List<GameObject>();

    public void ballAtEnd()
    {
        GameObject[] pins = GameObject.FindGameObjectsWithTag("pin");
        int pinsnocked = 0;
        foreach (GameObject pin in pins)
        {
            if(pin.transform.rotation.eulerAngles.x > 50|| pin.transform.rotation.eulerAngles.x < -50|| pin.transform.rotation.eulerAngles.z > 50 || pin.transform.rotation.eulerAngles.z < -50)
            {
                pinsnocked++;
                pin.gameObject.SetActive(false);
                pinsDelete.Add(pin);
            }
        }
        if (!secondPart)
        {
            secondPart = true;
            currentscore.x = pinsnocked; 
        }
        else
        {
            secondPart= false;
            foreach (GameObject pin in pins)
            {
                pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                pin_script.Reset();
            }
            foreach (GameObject pin in pinsDelete)
            {
                pin.gameObject.SetActive(true);
                pinscript pin_script = pin.gameObject.GetComponent<pinscript>();
                pin_script.Reset();
            }
            pinsDelete.Clear();
            currentscore.y = pinsnocked;
            listOfscores.Add(currentscore);
            for (int i = 0; i < listOfscores.Count; i++)
            {
                Debug.Log(listOfscores[i]);
            }
        }
    } 
}
