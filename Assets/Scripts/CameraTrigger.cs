using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public ChangeCamera cameraScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        cameraScript.onPlayerCam = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        cameraScript.onPlayerCam = true;
    }
}
