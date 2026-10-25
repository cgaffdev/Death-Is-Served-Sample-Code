using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneClick : MonoBehaviour
{

    // Changes the cursor based on hovering over the collider
    // If the user clicks on it, destroys the object.

    void OnMouseEnter()
    {
        CursorManager.instance.CursorClick();
    }

    void OnMouseExit()
    {
        CursorManager.instance.CursorDefault();
    }

    void OnMouseDown()
    {
        CursorManager.instance.CursorDefault();
        Destroy(gameObject);
    }
}
