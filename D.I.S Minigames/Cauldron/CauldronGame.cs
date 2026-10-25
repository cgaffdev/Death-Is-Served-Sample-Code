using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CauldronGame : MonoBehaviour
{

    /// Thought for improvement:
    /// Have the pot slowly revert back if the user takes too long or stops stirring


    [SerializeField] MoveTheSprite objectToMove;
    [SerializeField] private List<Sprite> sprites; // List of sprites to iterate
    [SerializeField] private SpriteRenderer targetRenderer; // Renderer to update
    [SerializeField] float targetDistance;


    bool gameEnd = false;
    void Update()
    {
        if (gameEnd == false)
        {
            // Checking to see if the win condition is met
            if(sprites.Count>0 && targetRenderer!=null) SpritePerDistance();
            if (objectToMove.GetTotalDistance() >= targetDistance)  // Win condition being if the object met the target distance threshhold
            {
                gameEnd = true;
                StartCoroutine(EndGame());
            }
            
        }
    }


    // Changes the sprite based on distance
    void SpritePerDistance()
    {
         if (sprites == null || sprites.Count == 0) return;

        // Clamp distance to maxDistance
        float clampedDistance = Mathf.Min(objectToMove.GetTotalDistance(), targetDistance);

        // Calculate fraction of distance traveled
        float fraction = clampedDistance / targetDistance;

        // Map fraction to sprite index
        int index = Mathf.FloorToInt(fraction * (sprites.Count - 1));

        targetRenderer.sprite = sprites[index];
    }
    
    // Closes the Mini-Game out without completition
    public void Close()
    {
        // Play sound
        if (gameEnd == true) return;
        SoundPlayer.Instance.PlayButton();
        PlayerManager.instance.FreezePlayer(false);
        Destroy(gameObject);
    }

    // Ends then closes the game with completition, rewarding the player.
    IEnumerator EndGame()
    {
        // Play sound
        gameEnd = true;
        SoundPlayer.Instance.PlaySound(SoundPlayer.Instance.gameComplete);
        yield return new WaitForSeconds(.5f);

        // Rewards player
        CookingManager.instance.OnStewGather();
        PlayerManager.instance.FreezePlayer(false); // Returns movement to the player
        Destroy(gameObject);
    }

}
