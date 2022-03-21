using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuoPressurePlate : MonoBehaviour
{
    [SerializeField]
    GameObject door;

    bool playerOn = false;
    bool ghostOn = false;
    private DuoPlatesDoor door_script;

    void Start()
    {
        door_script = door.GetComponent<DuoPlatesDoor>();
    }

    void Update()
    {
        print(door_script.platesPressed);
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            // if(!playerOn)
            // {
            //     playerOn = true;
            //     door.transform.position += new Vector3(0, 1.2f, 0);
            // }
            playerOn = true;
            if(!ghostOn)
                door_script.platesPressed -= 1;
        }
        if(col.gameObject.tag == "Ghost")
        {
            ghostOn = true;
            if(!playerOn) //not the plate player is already stepping on
            {
                door_script.platesPressed -= 1;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            if(door_script.platesPressed > 0) //door has not opened but player or ghost stepped off plate
            {
                playerOn = false;
                if(!ghostOn)
                    door_script.platesPressed += 1;
            }
        }
        if(col.gameObject.tag == "Ghost")
        {
            if(door_script.platesPressed > 0) //door has not opened but player or ghost stepped off plate
            {
                ghostOn = false;
                if(!playerOn)
                    door_script.platesPressed += 1;
            }
        }
    }
}
