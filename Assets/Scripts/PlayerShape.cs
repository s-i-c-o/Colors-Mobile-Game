using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShape : MonoBehaviour
{
    // Private variables
    private Transform backgroundColorToMatch;
    private Color targetColor;

    // Start is called before the first frame update
    void Start()
    {
        backgroundColorToMatch = GameObject.Find("BackgroundColorToMatch").transform;
        targetColor = backgroundColorToMatch.transform.GetComponent<SpriteRenderer>().color;

        Color startColor = new Color(targetColor.r, Random.Range(0f, 1f), targetColor.b, 1f);
        this.GetComponent<SpriteRenderer>().color = startColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
