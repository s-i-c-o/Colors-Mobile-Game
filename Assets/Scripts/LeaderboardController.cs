using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardController : MonoBehaviour
{
    public Transform firstPlaceText;
    public Transform secondPlaceText;
    public Transform thirdPlaceText;
    public Transform firstPlaceInitials;
    public Transform secondPlaceInitials;
    public Transform thirdPlaceInitials;


    private PlayerSaveData playerData;

    // Start is called before the first frame update
    void Start()
    {
        // Load Player Data
        playerData = PlayerDataSaver.LoadPlayerData();

        // Populate Fields
        firstPlaceText.GetComponent<Text>().text = playerData.firstPlaceScore.ToString();
        secondPlaceText.GetComponent<Text>().text = playerData.secondPlaceScore.ToString();
        thirdPlaceText.GetComponent<Text>().text = playerData.thirdPlaceScore.ToString();

        firstPlaceInitials.GetComponent<Text>().text = playerData.firstPlaceInitials.ToUpper();
        secondPlaceInitials.GetComponent<Text>().text = playerData.secondPlaceInitials.ToUpper();
        thirdPlaceInitials.GetComponent<Text>().text = playerData.thirdPlaceInitials.ToUpper();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
