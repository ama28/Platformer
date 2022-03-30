using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTriggerPass : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //transform.Find("Canvas").Find("CanTalk").gameObject.SetActive(true);
            //GameObject.Find("DialogueManager").GetComponent<DialogueFlow>().canTalk = true;
           // GameObject.Find("DialogueManager").GetComponent<DialogueFlow>().isTalking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            //transform.Find("Canvas").Find("CanTalk").gameObject.SetActive(false);
            //GameObject.Find("DialogueManager").GetComponent<DialogueFlow>().canTalk = false;
        }


    }
}
