using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyPlayerData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerController = GameObject.FindGameObjectWithTag("PlayerController");
        DontDestroyOnLoad(playerController);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake()
    {
        
    }
}
