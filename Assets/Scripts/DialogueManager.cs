using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform canvas;
    private GameObject textBar;
    public bool canTalk, talking;
    public string line;
    public float typingSpeed;

    void Start()
    {
        canvas = transform.Find("Canvas");
        textBar = canvas.Find("TextBar").gameObject;
        
        //disable all canvas components
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        canTalk = false;
        talking = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (canTalk && Input.GetKey(KeyCode.F))
        {
            //enable textbar
            textBar.SetActive(true);
            //typing effect
            StartCoroutine(typeOut());
            canTalk = false;
        }

        //if(talking)
        
    }

    private IEnumerator typeOut()
    {
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
            textBar.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = toShow;
            //print("wee");

            yield return null;

        }

        textBar.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = line;
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
