using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StoveDoor : MonoBehaviour
{
    [SerializeField] GameObject AlternateObject;
    [SerializeField] StoveGame stoveGame;


    void OnMouseDown()
    {
        if (stoveGame.isDoorReady)
        {
            stoveGame.AccessDoor();
            AlternateObject.SetActive(true);
            CursorManager.instance.CursorDefault();
            gameObject.SetActive(false);
            
        }
        

    }

    void OnMouseOver()
    {
        CursorManager.instance.CursorClick();
    }

    void OnMouseExit()
    {
        CursorManager.instance.CursorDefault();
    }


}
