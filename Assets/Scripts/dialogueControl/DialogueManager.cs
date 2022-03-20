using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform canvas;
    public bool canTalk, NPC_talking, doneTalking;
    //NPC talking represents who talks first
    public string NPC_line, player_line;
    public float typingSpeed;
    public float textBar_showDelay;

    private GameObject NPC_textBar;
    private GameObject player_textBar;
    void Start()
    {
        canvas = transform.Find("Canvas");
        NPC_textBar = canvas.Find("TextBar").gameObject;

        GameObject player_canvas = GameObject.FindGameObjectWithTag("Player");
        //player_textBar = player_canvas.transform.Find("Canvas").Find("TextBar").gameObject;
        
        //disable all canvas component
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        //player_textBar.SetActive(false);
        canTalk = false;
        NPC_talking = true;
        doneTalking = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (canTalk && Input.GetKey(KeyCode.F))
        {
            //typing effect
            if(NPC_talking)
                StartCoroutine(typeOut(NPC_line, NPC_textBar));
            else
                StartCoroutine(typeOut(player_line, player_textBar));
            canTalk = false;
        }

        /*if (doneTalking && Input.GetKey(KeyCode.F))
        {
            for (int i = 0; i < canvas.childCount; i++)
            {
                canvas.GetChild(i).gameObject.SetActive(false);

            }
            doneTalking = false;
        }*/

    }

    private IEnumerator typeOut(string line, GameObject textBar)
    {
        textBar.SetActive(true);
        int sublen = 0;
        float timer = 0;
        while (sublen < line.Length)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                break;
            timer += Time.deltaTime * typingSpeed;
            sublen = Mathf.FloorToInt(timer);

            if (sublen > line.Length)
                sublen = line.Length;

            string toShow = line.Substring(0, sublen);
            //print(timer);
            textBar.transform.Find("Text").GetComponent<Text>().text = toShow;
            //print("wee");

            yield return null;
        }

        textBar.transform.Find("Text").GetComponent<Text>().text = line;

        yield return new WaitForSeconds(textBar_showDelay);
        //doneTalking = true;
        //disable all canvas components
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canvas.Find("CanTalk").gameObject.SetActive(true);
            canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            //print("Exit");
            canTalk = false;

            //disable all canvas components
            for (int i = 0; i < canvas.childCount; i++)
            {
                canvas.GetChild(i).gameObject.SetActive(false);
            }

        }


    }
}
