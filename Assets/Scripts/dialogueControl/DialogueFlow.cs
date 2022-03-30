using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueFlow : MonoBehaviour
{
    public GameObject player_textBar;
    public GameObject NPC_textBar;
    public GameObject talkIndicator;

    public Rigidbody2D player_rigid2D;//for freezing player

    //this script is directly attached to canvas
    public string[] lines;
    public float typingSpeed;
   // public float textBar_showDelay;
    public int current_line;
    public string script_name;

    public bool canTalk;
    private bool isTalking;
    private bool dialogueSoundPlaying;

    public bool isTutorial;
    public GameObject rewindMessage;
    private PlayerMovement movementScript;

    void Start()
    {
        typingSpeed = 15;

        //findTextBar
        player_textBar = GameObject.FindGameObjectWithTag("TextBarPlayer");
        NPC_textBar = GameObject.FindGameObjectWithTag("TextBarNPC");
        talkIndicator = GameObject.FindGameObjectWithTag("TextIndicator");

        movementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        player_rigid2D = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();

        player_textBar.gameObject.SetActive(false);
        NPC_textBar.gameObject.SetActive(false);
        talkIndicator.SetActive(false);

        isTalking = false;
        canTalk = false;

        //initialize the text obtaining from another script
        current_line = 0;

        //load dialogue writing
        var textFile = Resources.Load<TextAsset>(script_name);
        //print(textFile.text);
        lines = textFile.text.Split('\n');

    }

    // Update is called once per frame
    void Update()
    {
        if (canTalk) {

            talkIndicator.SetActive(!isTalking && current_line == 0);//show can talk indicator

            if (!isTalking && Input.GetKeyDown(KeyCode.F) && current_line < lines.Length)
            {
                //typing effect
                //string myLine = lines[current_line];

                //skip the comments line
                while (current_line < lines.Length && lines[current_line][0] == '#')
                    current_line++;

                string myLine = lines[current_line];

                string header = myLine.Split(':')[0];
                string content = myLine.Split(':')[1];

                //disable all canvas components
                player_textBar.gameObject.SetActive(false);
                NPC_textBar.gameObject.SetActive(false);

                if (header == "p")
                {
                    StartCoroutine(typeOut(content, player_textBar, 0));
                }
                else if (header == "n")
                    StartCoroutine(typeOut(content, NPC_textBar, 1));
            
            }

            if (Input.GetKeyDown(KeyCode.F) && current_line == lines.Length)
            {
                //disable all canvas components
                player_textBar.gameObject.SetActive(false);
                NPC_textBar.gameObject.SetActive(false);

                canTalk = false;
                //maybe change reset current line counter

                if (isTutorial)
                {
                    rewindMessage.gameObject.SetActive(true);
                    movementScript.rewindTransition = true;
                }
            }

            
        }else
            talkIndicator.SetActive(false);

        if(isTalking)
            player_rigid2D.constraints = RigidbodyConstraints2D.FreezePosition;
        else
            player_rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    private IEnumerator typeOut(string line, GameObject textBar, int speaker) //speaker = 0 for player, 1 for scientist
    {
        textBar.SetActive(true);
        dialogueSoundPlaying = true;
        StartCoroutine(DialogueSound(speaker));
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
        dialogueSoundPlaying = false;
        //next line
        current_line++;

    }

    public bool FinishedTalking() {
        return current_line == lines.Length;
    }

    IEnumerator DialogueSound(int speaker)
    {
        while (dialogueSoundPlaying)
        {
            if (speaker == 0)
                FMODUnity.RuntimeManager.PlayOneShot("event:/Player Voice");
            else if (speaker == 1)
                FMODUnity.RuntimeManager.PlayOneShot("event:/Scientist Voice");
            yield return new WaitForSeconds(.18f);
        }
    }
}
