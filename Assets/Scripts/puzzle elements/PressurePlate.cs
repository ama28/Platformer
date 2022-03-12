using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    private bool is_triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!is_triggered)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(false);
            is_triggered = true;
        }
    }
}
