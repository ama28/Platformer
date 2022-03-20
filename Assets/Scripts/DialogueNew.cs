using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueNew : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform canvas;
    //public bool canTalk, NPC_talking, doneTalking;
    //NPC talking represents who talks first
    
    //this script is directly attached to canvas
    public string[] player_lines;
    public float typingSpeed;
    public float textBar_showDelay;
    public int current_line;
    public string script_name;

    private GameObject my_textBar;
    private bool isTalking, jumpToEnd;
    void Start()
    {
        canvas = transform;
        my_textBar = transform.Find("TextBar").gameObject;

        //disable all canvas component
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        isTalking = false;


        //initialize the text obtaining from another script
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!isTalking & Input.GetKeyDown(KeyCode.F))
        {
            //typing effect
            StartCoroutine(typeOut(player_lines[current_line], my_textBar));
            //isTalking = true;
            //canTalk = false;
        }

        
    }

    private IEnumerator typeOut(string line, GameObject textBar)
    {
        textBar.SetActive(true);
        int sublen = 0;
        float timer = 0;
        while (sublen < line.Length)
        {
            if (Input.GetKeyUp(KeyCode.F))
                isTalking = true;

            if (isTalking & Input.GetKeyDown(KeyCode.F))
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
        isTalking = false;//finish talking
        //next line
        
        //yield return new WaitForSeconds(textBar_showDelay);

        //should switch to next line
        //disable all canvas components
        /*for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //canvas.Find("CanTalk").gameObject.SetActive(true);
            //canTalk = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            //print("Exit");
            //canTalk = false;

            //disable all canvas components
            for (int i = 0; i < canvas.childCount; i++)
            {
                canvas.GetChild(i).gameObject.SetActive(false);
            }

        }

    }
}
