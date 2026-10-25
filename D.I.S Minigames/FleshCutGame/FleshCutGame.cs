using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleshCutGame : MonoBehaviour
{

    // Notes to Improve:
    // - Consider making a nested list, this allows objects to varry in slice requirement
    // - Minigames could be directly tired to an item to reward the player rather than a function for it.
    // - Minigames could follow an Abstract class

    // Sections of the food item that needs to be cut through. 
    [SerializeField] List<BoxCollider2D> LeftSlice; 
    [SerializeField] List<BoxCollider2D> RightSlice;


    [SerializeField] SpriteRenderer FleshSprite;
    [SerializeField] BoxCollider2D knifeEdge; // The collider that will be reacted to

    // Tracker to check how many slice units are activiated on each side.
    int leftState = 0;
    int rightState = 0;

    bool gameEnded = false; // Game Flag
    
    string spriteName = ""; 

    // Assures all colliders are disabled for later
    void Awake()
    {
        for (int i = 0; i < LeftSlice.Count; i++)
            LeftSlice[i].enabled = false;


        for (int i = 0; i < RightSlice.Count; i++)
            RightSlice[i].enabled = false;

    }


    // List could be updated to be a nested list, a for loop used to cycle through each nested list to accomplish the same thing.
    void LateUpdate()
    {


        // Checking to see there are any more slices left
        if (LeftSlice.Count > 0)
        {
            // If there are colliders left in the slice's list, enables the first one to be interacted with
            LeftSlice[0].enabled = true;
            if (LeftSlice[0].IsTouching(knifeEdge)) // If the knife touches a collider, it removes it from the list. 
            {
                LeftSlice.RemoveAt(0);
                leftState++;
 
            }
        }

        // Checking to see there are any more slices left
        if (RightSlice.Count > 0)
        {

            // If there are colliders left in the slice's list, enables the first one to be interacted with
            RightSlice[0].enabled = true;
            if (RightSlice[0].IsTouching(knifeEdge)) // If the knife touches a collider, it removes it from the list. 
            {
                RightSlice.RemoveAt(0);
                rightState++;
 
            }
        }

        // If each list is empty, the game is called to an end as all slices have been completely made.
        if (RightSlice.Count <= 0 && LeftSlice.Count <= 0)
        {
            Debug.Log("ALL CUT!");
            if(gameEnded==false) StartCoroutine(EndGame());
        }

        UpdateSprite();
    }

    

    // Used to construct the spritename based on states
    string GetFleshState()
    {
        return $"flesh_1_{leftState}+2_{rightState}";
    }


    // Updates the sprite based on the state
    void UpdateSprite()
    {
        spriteName = GetFleshState();
        Sprite s = Resources.Load<Sprite>("Flesh/" + spriteName);
        FleshSprite.sprite = s;
        Debug.Log("Updating Sprite");
    }

    // Closes the Mini-Game out without completition
    public void Close()
    {
        // Play sound
        if (gameEnded == true) return;
        SoundPlayer.Instance.PlayButton();
        PlayerManager.instance.FreezePlayer(false);
        Destroy(gameObject);
    }
    
    // Ends then closes the game with completition, rewarding the player.
    IEnumerator EndGame()
    {
        // Play sound
        gameEnded = true;
        SoundPlayer.Instance.PlaySound(SoundPlayer.Instance.gameComplete);
        yield return new WaitForSeconds(.5f);


        PlayerManager.instance.FreezePlayer(false); // Frees the player and allows them to move again
        CookingManager.instance.OnFleshGathered(); // Rewards player
        Destroy(gameObject);
    }

}
