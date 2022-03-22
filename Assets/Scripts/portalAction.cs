using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portalAction : MonoBehaviour
{
    // Start is called before the first frame update
    public bool canEnter;
    public GameObject enterIndicator;
    public string nextScene;
    void Start()
    {
        canEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canEnter) 
        {
            enterIndicator.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F))
                SceneManager.LoadScene(nextScene);
        }    
        else
            enterIndicator.SetActive(false);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canEnter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            canEnter = false;
        }

    }
}
