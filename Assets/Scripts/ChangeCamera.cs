using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    private Animator cameraAnimator;
    public bool onPlayerCam = true;

    // Start is called before the first frame update
    void Start()
    {
        cameraAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        SwitchState();
    }

    private void SwitchState()
    {
        if (onPlayerCam)
            cameraAnimator.Play("PlayerCam");
        else cameraAnimator.Play("Room1Cam");
    }

}
