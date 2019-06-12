using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("PlayerController") != null)
        {
            Debug.Log("Found a player controller");

            // We found a player controller from the last play, so we need to delete it
            Destroy(GameObject.FindGameObjectWithTag("PlayerController"));
        }
        else
        {
            Debug.Log("Could NOT find player controller");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
