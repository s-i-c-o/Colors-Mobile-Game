using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ColorSlider : MonoBehaviour
{
    public Slider colSlider;
    public Color defaultColor; 
    private Color currentColor;
    private Color[] availableColors = { Color.blue, Color.green, Color.yellow };
    // Start is called before the first frame update
    void Start()
    {
        currentColor = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
