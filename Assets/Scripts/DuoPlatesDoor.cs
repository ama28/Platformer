using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoPlatesDoor : MonoBehaviour
{
    public int platesPressed = 2;
    public bool opened = false;
    Vector3 startPos;
    public float fallSpeed;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if(platesPressed == 0 && !opened)
        {
            transform.position += new Vector3(0, 1.4f, 0);
            opened = true; 
        }
        if(platesPressed > 0 && opened)
        {
            transform.position = Vector2.MoveTowards(transform.position, startPos, fallSpeed * Time.deltaTime);
            if(transform.position == startPos)
            {
                opened = false;
            }
        }
    }
}
