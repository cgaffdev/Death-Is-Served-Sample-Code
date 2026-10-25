using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTheSprite : MonoBehaviour
{
    private bool dragging = false;
    private Vector2 lastPosition;
    private float totalDistance = 0f;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        dragging = true;
        lastPosition = transform.position;
    }

    void OnMouseUp()
    {
        dragging = false;
        Debug.Log("Total Distance: " + GetTotalDistance());

    }

    void Update()
    {
        if (dragging)
        {
            // Current position in 2D
            Vector2 currentPosition = transform.position;

            // Accumulate distance moved since last frame
            totalDistance += Vector2.Distance(currentPosition, lastPosition);

            // Update last position
            lastPosition = currentPosition;
        }
    }

    // Optional: get the total distance traveled
    public float GetTotalDistance()
    {
        return totalDistance;
    }
}
