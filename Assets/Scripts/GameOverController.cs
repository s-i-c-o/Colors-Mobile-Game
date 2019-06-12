using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    private PlayerSaveData playerData;

    private int playerPosition;
    private int mostRecentScore;

    private bool scoreChecked;

    // UI
    private Transform initialsInputField;

    // Start is called before the first frame update
    void Start()
    {
        initialsInputField = GameObject.Find("InitialsInputField").transform;
        initialsInputField.gameObject.SetActive(false);

        scoreChecked = false;

        playerData = PlayerDataSaver.LoadPlayerData();
        mostRecentScore = playerData.recentScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (!scoreChecked)
        {
            CheckForHighScore();

            if (playerPosition > 0)
            {
                StartCoroutine(GetPlayerName());

            }
        }

    }

    void CheckForHighScore()
    {
        playerPosition = 0;
        // Check for high score
        if (mostRecentScore > playerData.firstPlaceScore)
        {
            playerData.thirdPlaceScore = playerData.secondPlaceScore;
            playerData.secondPlaceScore = playerData.firstPlaceScore;
            playerData.firstPlaceScore = mostRecentScore;
            playerPosition = 1;
        }
        else if ((mostRecentScore <= playerData.firstPlaceScore) && (mostRecentScore > playerData.secondPlaceScore))
        {
            playerData.thirdPlaceScore = playerData.secondPlaceScore;
            playerData.secondPlaceScore = mostRecentScore;
            playerPosition = 2;
        }
        else if ((mostRecentScore <= playerData.secondPlaceScore) && (mostRecentScore > playerData.thirdPlaceScore))
        {
            playerData.thirdPlaceScore = mostRecentScore;
            playerPosition = 3;
        }
        else
        {
            // Player did not get a high score
            playerPosition = 0;
        }
        Debug.Log("Player got high score: " + playerPosition.ToString());

        scoreChecked = true;
    }

    IEnumerator GetPlayerName()
    {
        initialsInputField.gameObject.SetActive(true);
        yield return null;
    }

    public void SaveHighScore()
    {
        if (playerPosition == 1)
        {
            playerData.thirdPlaceInitials = playerData.secondPlaceInitials;
            playerData.secondPlaceInitials = playerData.firstPlaceInitials;
            playerData.firstPlaceInitials = initialsInputField.GetComponent<InputField>().text;
        }
        else if (playerPosition == 2)
        {
            playerData.thirdPlaceInitials = playerData.secondPlaceInitials;
            playerData.secondPlaceInitials = initialsInputField.GetComponent<InputField>().text;
        }
        else if (playerPosition == 3)
        {
            playerData.thirdPlaceInitials = initialsInputField.GetComponent<InputField>().text;
        }

        // Now save all the data
        PlayerDataSaver.SavePlayerData(playerData);
    }
}
