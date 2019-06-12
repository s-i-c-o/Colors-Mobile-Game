using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeController : MonoBehaviour
{
    // Public variables
    public float targetScale;
    public LevelController levelController;

    // Private variables
    private float speedAdjuster = 0.1f; // Speed adjuster needed to slowly increase speed of shrink
    private float scaleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
        scaleSpeed = levelController.scaleSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Update scale speed if necessary
        if (scaleSpeed != levelController.scaleSpeed)
        {
            scaleSpeed = levelController.scaleSpeed;
            
        }

        if (this.transform.localScale.x > 2.51f && this.transform.localScale.y > 2.51f)
        {
            shrink();
        }
    }

    // Method for shrinking objects
    private void shrink()
    {
        this.transform.localScale = Vector3.Lerp(this.transform.localScale, new Vector3(targetScale, targetScale, this.transform.localScale.z), Time.deltaTime * scaleSpeed * speedAdjuster);
        speedAdjuster += 0.01f;
    }
}
