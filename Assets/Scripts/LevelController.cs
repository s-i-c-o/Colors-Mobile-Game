using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    // Public variables
    public Transform[] shrinkingShapes;
    public SceneSwitchController sceneSwitchController;
    public float scaleSpeed;
    public PlayerController playerController;

    // Private variables
    private int currentShapeIndex;
    private int lastShapeIndex;
    private Transform shapeToMatchParent;
    private Transform shapeToMatch;
    private Vector3 matchSize;
    private Color targetColor;
    private bool gamePaused;
    private bool firstShapeSpawned;

    // Private Audio
    private AudioSource loseLifeSound;
    private AudioSource music;

    // Private UI Variables
    private Text pointsText;
    private Text feedbackText;
    private bool feedbackTextFading;
    private Slider colorSlider;
    private Transform blurPanel;

    // Player variables
    private int playerPoints;
    private int playerLives;
    private Transform backgroundColorToMatch;
    private Transform playerShapeObject;
    private Transform playerShape;
    private Color currentPlayerColor;
    private Transform life3;
    private Transform life2;
    private PlayerSaveData playerData;

    // Start is called before the first frame update
    void Start()
    {

        // Setup parent objects
        shapeToMatchParent = GameObject.FindGameObjectWithTag("ShrinkingShape").transform;
        playerShapeObject = GameObject.FindGameObjectWithTag("PlayerShape").transform;
        pointsText = GameObject.FindGameObjectWithTag("Points").transform.GetComponent<Text>();
        feedbackText = GameObject.Find("Feedback").transform.GetComponent<Text>();
        colorSlider = GameObject.Find("ColorSlider").transform.GetComponent<Slider>();
        backgroundColorToMatch = GameObject.Find("BackgroundColorToMatch").transform;
        targetColor = backgroundColorToMatch.transform.GetComponent<SpriteRenderer>().color;
        life3 = GameObject.Find("life3").transform;
        life2 = GameObject.Find("life2").transform;

        // Pause menu Setup
        blurPanel = GameObject.Find("BlurPanel").transform;
        blurPanel.gameObject.SetActive(false);

        // Audio
        loseLifeSound = GameObject.Find("LoseLifeSound").transform.GetComponent<AudioSource>();
        music = GameObject.Find("Music").transform.GetComponent<AudioSource>();

        // Load Data for saving
        playerData = PlayerDataSaver.LoadPlayerData();

        // Start game
        firstShapeSpawned = false;
        playerPoints = 0;
        playerLives = 3;
        pointsText.text = playerPoints.ToString();
        gamePaused = false;
        /*
        // Make sure time is set correctly if we exited from the game earlier
        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }*/

        // Setup feedback
        feedbackText.text = "";
        feedbackText.canvasRenderer.SetAlpha(0);

        lastShapeIndex = 0;
        scaleSpeed = 0.2f;
        matchSize = new Vector3(2.5f, 2.5f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstShapeSpawned)
        {
            SpawnShapeAndColor();
            firstShapeSpawned = true;
        }

        Color newColor = new Color(targetColor.r, colorSlider.value, targetColor.b, 1f);

        // Handle out of bounds exception before game gets started
        if (playerShapeObject.childCount > 0)
        {
            playerShape = playerShapeObject.GetChild(0);
            // TODO: Find way of handling two RGB values for more complexity
            
            playerShape.GetComponent<SpriteRenderer>().color = newColor;
        }

        if (shapeToMatch != null)
        {
            if ((shapeToMatch.transform.localScale.x < 2.53f) && (shapeToMatch.transform.localScale.y < 2.53f))
            {
                float colorDifference = targetColor.g - newColor.g;

                // Check for shape match based on tag
                if (playerShape.tag == shapeToMatch.tag)
                {
                    Debug.Log("<color=red>CORRECT MATCH</color>");

                    // Award Points
                    DeterminePoints(true, colorDifference);
                }
                else
                {
                    Debug.Log("<color=red>NOT A MATCH</color>");
                    DeterminePoints(false, colorDifference);
                }

                // The shape needs to be destroyed and unparented from the parent object
                Destroy(shapeToMatch.gameObject);
                shapeToMatchParent.DetachChildren();

                // Spawn a new shape and color
                SpawnShapeAndColor();
                // Align RGB slider
                colorSlider.value = playerShape.GetComponent<SpriteRenderer>().color.g;

                // Speed things up a bit
                scaleSpeed += 0.05f;
                Debug.Log("Scale speed now at:" + scaleSpeed);

                // Check if wee need to remove a life
                if(playerLives == 2)
                {
                    life3.gameObject.SetActive(false);
                }
                if (playerLives == 1)
                {
                    life2.gameObject.SetActive(false);
                }
                if (playerLives < 1)
                {
                    // End the Game
                    EndGame();
                }
            }
        }
    }

    void SpawnShapeAndColor()
    {
        // Pick a random shape to start with and instantiate it
        float randomShapeIndex = Random.Range(0.0f, 1.0f);
        if (randomShapeIndex > 0.8f)
        {
            currentShapeIndex = 0;
        }
        else if (randomShapeIndex <= 0.8f && randomShapeIndex > 0.6f)
        {
            currentShapeIndex = 1;
        }
        else if (randomShapeIndex <= 0.6f && randomShapeIndex > 0.4f)
        {
            currentShapeIndex = 2;
        }
        else if (randomShapeIndex <= 0.4f && randomShapeIndex > 0.2f)
        {
            currentShapeIndex = 3;
        }
        else
        {
            currentShapeIndex = 4;
        }
        if (currentShapeIndex == lastShapeIndex) // This is to not spawn the same shape twice
        {
            currentShapeIndex += 1;
        }
        Instantiate(shrinkingShapes[currentShapeIndex], shapeToMatchParent);
        lastShapeIndex = currentShapeIndex;

        shapeToMatch = shapeToMatchParent.GetChild(0);
        // Assign our target color
        float newR = Random.Range(0f,1f);
        float newG = Random.Range(0f, 1f);
        float newB = Random.Range(0f, 1f);
        backgroundColorToMatch.GetComponent<SpriteRenderer>().color = new Color(newR, newG, newB, 1f);
        targetColor = backgroundColorToMatch.transform.GetComponent<SpriteRenderer>().color;
    }

    void DeterminePoints(bool shapeMatch, float colorDifference)
    {
        int pointsToAward = 0;

        pointsToAward += GetFeedback(shapeMatch, colorDifference);

        UpdatePlayerPoints(pointsToAward);
    }

    int GetFeedback(bool match, float colorDifference)
    {
        int feedbackRating = 0;

        // First add rating points for shape match
        if (match) // If the player got a Shape match
        {
            feedbackRating += 30;
        }
        else // If the player didn't get a match
        {
            feedbackRating -= 20;
            playerLives--;
            loseLifeSound.Play();
        }

        // Now check for how close they got to the color
        if ((colorDifference < 0.00002f) && (colorDifference > 0.00001f))
        {
            Debug.Log("Color within 0.00001 and 0.00002");
            feedbackRating += 50;
        }
        else if ((colorDifference < 0.0002f) && (colorDifference > 0.0001f))
        {
            Debug.Log("Color within 0.0001 and 0.0002");
            feedbackRating += 40;
        }
        else if ((colorDifference < 0.002f) && (colorDifference > 0.001f))
        {
            Debug.Log("Color within 0.001 and 0.002");
            feedbackRating += 30;
        }
        else if ((colorDifference < 0.02f) && (colorDifference > 0.01f))
        {
            Debug.Log("Color within 0.01 and 0.02");
            feedbackRating += 10;
        }
        else if ((colorDifference < 0.2f) && (colorDifference > 0.1f))
        {
            Debug.Log("Color within 0.1 and 0.2");
            feedbackRating -= 10;
        }

        Debug.Log("Feedback rating: " + feedbackRating);

        // Now set the feedback based on the rating
        if (feedbackRating >= 50)
        {
            feedbackText.text = "EXCELLENT!!!";
        }
        else if ((feedbackRating < 50) && (feedbackRating >= 40))
        {
            feedbackText.text = "GREAT!!";
        }
        else if ((feedbackRating < 40) && (feedbackRating >= 30))
        {
            feedbackText.text = "NICE!";
        }
        else if ((feedbackRating < 30) && (feedbackRating >= 20))
        {
            feedbackText.text = "GREAT!";
        }
        else if ((feedbackRating < 20) && (feedbackRating >= 10))
        {
            feedbackText.text = "GOOD";
        }
        else if ((feedbackRating < 10) && (feedbackRating >= 0))
        {
            feedbackText.text = "OOF...";
        }
        else if (feedbackRating < 0)
        {
            feedbackText.text = "OUCH";
        }
        Debug.Log("feedback text: " + feedbackText.text);

        StartCoroutine(FadeFeedback());

        return feedbackRating;
    }

    IEnumerator FadeFeedback()
    {
        Debug.Log("Fade feedback running");
        feedbackText.CrossFadeAlpha(1f, 0.3f, false);
        yield return new WaitForSeconds(0.3f);
        feedbackText.CrossFadeAlpha(0f, 0.3f, false);
    }

    void UpdatePlayerPoints(int points)
    {
        int newPoints = playerPoints;
        newPoints += points;
        playerPoints = newPoints;
        pointsText.text = playerPoints.ToString();
    }

    public void PauseGame()
    {
        GameObject canvas = GameObject.Find("Canvas");
        if (!gamePaused)
        {
            // Stop the game and pause music
            Time.timeScale = 0.0f;
            music.Pause();

            // Blur the background and setup pause menu
            blurPanel.gameObject.SetActive(true);
            gamePaused = true;
        } 
        else
        {
            // Unfreeze game and start music
            Time.timeScale = 1.0f;
            music.UnPause();

            // Hide pause menu
            blurPanel.gameObject.SetActive(false);
            gamePaused = false;
        }
    }

    void EndGame()
    {
        // Save the most recent score so we can check it on the
        // game over screen for a high score
        playerData.recentScore = playerPoints;
        PlayerDataSaver.SavePlayerData(playerData);

        // Move to next scene
        sceneSwitchController.GoToGameOver();
    }
}
