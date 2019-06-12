using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public Transform[] userShapes;

    // Private variables
    private int currentShapeIndex;
    private int currentColorIndex;
    private Transform playerParent;
    private Color currentColor;
    private Transform currShape;
    private List<Color> availableColors;
    private Slider colorSlider;

    private bool firstShapeSpawned;

    private Rect rightSide;
    private Rect leftSide;

    // Start is called before the first frame update
    void Start()
    {
        availableColors = new List<Color>();
        // Get the parent
        if (GameObject.FindGameObjectWithTag("PlayerShape") != null)
        {
            Debug.Log("Found player shape");
        }
        else
        {
            Debug.Log("Could NOT find player shape");
        }

        playerParent = GameObject.FindGameObjectWithTag("PlayerShape").transform;

        colorSlider = GameObject.Find("ColorSlider").transform.GetComponent<Slider>();

        //Establish sides for controls
        leftSide = new Rect(0, 0, Screen.width / 2, Screen.height);
        rightSide = new Rect(Screen.width / 2, 0, Screen.width / 2, Screen.height);

        firstShapeSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {

        // This is ugly but it fixes the game not starting bug
        if (!firstShapeSpawned)
        {
            SpawnFirstShape();
            firstShapeSpawned = true;
        }

        // Mobile controls
        Touch[] currentTouches = Input.touches;

        /*
        ////////////////////////////////////////
        ////  FOR TESTING ON PC ////////////////
        ////////////////////////////////////////
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("left click");
            if (leftSide.Contains(Input.mousePosition))
            {
                // Player touching left side
                Debug.Log("Player touched left side: " + Input.mousePosition.ToString());

                // First find what our next shape should be
                if ((currentShapeIndex - 1) < 0 )// If we are at the end of the list
                {
                    currentShapeIndex = userShapes.Length - 1; // Go back to the beginning
                }
                else
                {
                    currentShapeIndex -= 1;
                }
            }
            else if (rightSide.Contains(Input.mousePosition))
            {
                // Player touching right side
                Debug.Log("Player touched right side: " + Input.mousePosition.ToString());

                // First find what our next shape should be
                if ((currentShapeIndex + 1) > userShapes.Length - 1) // If we are at the end of the list
                {
                    currentShapeIndex = 0; // Go back to the beginning
                }
                else
                {
                    currentShapeIndex += 1;
                }
            }

            // Delete old shape
            Transform oldShape = playerParent.GetChild(0);
            // Handle for null
            if (oldShape != null)
            {
                // Delete the shape
                Destroy(oldShape.gameObject);
            }
            else
            {
                Debug.Log("COULD NOT FIND PLAYER SHAPE TO DELETE");
            }

            // Now spawn new shape
            currShape = Instantiate(userShapes[currentShapeIndex], playerParent);
        }
        ///////////////////////////////////////
        ///////////////////////////////////////
        */

        if (Input.touchCount > 0)
        {
            Touch shapeChanger;
            Touch colorChanger;
            
            for (int i = 0; i < Input.touchCount; i++)
            {
                // Check to see where the user is touching
                if (IsPointerOverUIObject(currentTouches[i]))
                {
                    colorChanger = currentTouches[i];
                }
                else // If they tapped somewhere on the screen that isn't a UI element
                {
                    shapeChanger = currentTouches[i];

                    if (shapeChanger.phase == TouchPhase.Began)
                    {
                        if (leftSide.Contains(shapeChanger.position))
                        {
                            // Player touching left side
                            Debug.Log("Player touched left side: " + shapeChanger.position.ToString());

                            // First find what our next shape should be
                            if ((currentShapeIndex - 1) < 0)// If we are at the end of the list
                            {
                                currentShapeIndex = userShapes.Length - 1; // Go back to the beginning
                            }
                            else
                            {
                                currentShapeIndex -= 1;
                            }
                        }
                        else if (rightSide.Contains(shapeChanger.position))
                        {
                            // Player touching right side
                            Debug.Log("Player touched right side: " + shapeChanger.position.ToString());

                            // First find what our next shape should be
                            if ((currentShapeIndex + 1) > userShapes.Length - 1) // If we are at the end of the list
                            {
                                currentShapeIndex = 0; // Go back to the beginning
                            }
                            else
                            {
                                currentShapeIndex += 1;
                            }
                        }

                        // Delete old shape
                        Transform oldShape = playerParent.GetChild(0);
                        // Handle for null
                        if (oldShape != null)
                        {
                            // Delete the shape
                            Destroy(oldShape.gameObject);
                        }
                        else
                        {
                            Debug.Log("COULD NOT FIND PLAYER SHAPE TO DELETE");
                        }

                        // Now spawn new shape
                        currShape = Instantiate(userShapes[currentShapeIndex], playerParent);
                    }
                }
            }           
        }
    }

    // Checks to see if a touch is over a UI object 
    // so a shape does not change
    private bool IsPointerOverUIObject(Touch touch)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = touch.position;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private void SpawnFirstShape()
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
        currShape = Instantiate(userShapes[currentShapeIndex], playerParent);

        currShape.GetComponent<SpriteRenderer>().color = currentColor;
    }
}
