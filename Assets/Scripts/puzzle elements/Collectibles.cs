using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public Canvas can;
    public Text text_obj;
    public float speed = 3.5f;
    public float attraction_dist = 1.2f;
    private GameObject player;
    private int total_collectibles;

    // Start is called before the first frame update
    void Start()
    {
        total_collectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        // can.gameObject.SetActive(false); // set this to true when the player actually start platforming and stuff
        text_obj.text = "0/" + total_collectibles.ToString();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y),
                             new Vector2(transform.position.x, transform.position.y)) < attraction_dist)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }

        if (player.transform.position.x == transform.position.x && player.transform.position.y == transform.position.y)
        {
            GameObject[] coll = GameObject.FindGameObjectsWithTag("Collectible");
            int collected = total_collectibles - coll.Length + 1;

            text_obj.text = collected.ToString() + "/" + total_collectibles.ToString();

            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject);
    }

}
