using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public Canvas can;
    public Text text_obj;
    private int total_collectibles;
    private bool is_picked_up = false;

    // Start is called before the first frame update
    void Start()
    {
        total_collectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        // can.gameObject.SetActive(false); // set this to true when the player actually start platforming and stuff
        text_obj.text = "0/" + total_collectibles.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!is_picked_up && collision.CompareTag("Player"))
        {
            GameObject[] coll = GameObject.FindGameObjectsWithTag("Collectible");
            int collected = total_collectibles - coll.Length + 1;
            text_obj.text = collected.ToString() + "/" + total_collectibles.ToString();

            FMODUnity.RuntimeManager.PlayOneShot("event:/Collect");

            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(transform.GetChild(0).gameObject);

            is_picked_up = true;
        }
    }


}
