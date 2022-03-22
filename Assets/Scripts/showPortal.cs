using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showPortal : MonoBehaviour
{
    // Start is called before the first frame update
    private DialogueFlow dialogueManager;
    void Start()
    {
        dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueFlow>();
        //disable portal
        transform.Find("portal").gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueManager.FinishedTalking())
            transform.Find("portal").gameObject.SetActive(true);
    }
}
