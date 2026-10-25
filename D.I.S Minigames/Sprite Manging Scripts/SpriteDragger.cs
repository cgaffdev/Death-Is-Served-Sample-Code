using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteDragger : MonoBehaviour
{
    [SerializeField] Transform SpriteTransform;
    
    private Vector2 mouseOffset;
    bool isMouseDown = false;
    private bool isDragging = false;
    
    Camera mainCamera;

    Vector3 originalPos;
    void Awake()
    {
        originalPos = SpriteTransform.position; // Establishes the original placement of the knife before player interaction.
    }

    
    void OnMouseDown()
    {
        mainCamera = Camera.main;

        // Converts the mouse's screen position into a readable world position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
            mainCamera.WorldToScreenPoint(SpriteTransform.position).z)
        );

        // Creates an offset so the the sprite doesn't teleport or jump to the mouse when grabbed.
        mouseOffset = SpriteTransform.position - mouseWorldPosition;

        isMouseDown = true;
        isDragging = true;

        CursorManager.instance.CursorDrag(); // Sets the cursor in a dragging appearance
    }

    void OnMouseDrag()
    {
        
        if (isDragging)
        {

            // Converts screen position into readable world position
            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
                new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                mainCamera.WorldToScreenPoint(SpriteTransform.position).z)
            );

            CursorManager.instance.CursorDrag(); // Keeps the cursor in a dragging appearance

            // Moves the sprite to mouse position, maintaining the offset previously set.
            SpriteTransform.position = mouseWorldPosition + (Vector3)mouseOffset;
        }
    }


    // Resets the knife and returns it to it's original position
    void OnMouseUp()
    {
        isMouseDown = false;
        isDragging = false;

        SpriteTransform.position = originalPos;

        CursorManager.instance.CursorHover();
    }

    // Changes the cursor to dynamically react to a grabble object
    void OnMouseOver()
    {
        if (isDragging || isMouseDown)
        {
            CursorManager.instance.CursorDrag();
        }
        else
        {
            CursorManager.instance.CursorHover();
        }
    }

    // sets mouse back to default when no longer hovering over item
    void OnMouseExit()
    {
        CursorManager.instance.CursorDefault();
    }

}