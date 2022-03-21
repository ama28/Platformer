using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoPlatesDoor : MonoBehaviour
{
    public int platesPressed = 2;
    bool opened = false;

    void Update()
    {
        if(platesPressed == 0 && !opened)
        {
            transform.position += new Vector3(0, 1.2f, 0);
            opened = true; 
        }
    }
}
