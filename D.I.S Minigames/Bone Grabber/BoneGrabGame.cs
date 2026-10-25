using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneGrabGame : MonoBehaviour
{

    [SerializeField] Transform BoneContainer;


    // If there are no more bones within the container, game is flagged to end
    void LateUpdate()
    {
        if (BoneContainer.childCount <= 0)
        {
           if(gameEnd==false) StartCoroutine(EndGame());
        }
    }

    // Ends then closes the game with completition, rewarding the player.
    bool gameEnd = false;
    IEnumerator EndGame()
    {
        // Play sound
        gameEnd = true;
        SoundPlayer.Instance.PlaySound(SoundPlayer.Instance.gameComplete);

        yield return new WaitForSeconds(.5f);
        CookingManager.instance.OnBoneGathered();   // Rewards Player
        PlayerManager.instance.FreezePlayer(false); // Returns movement to the player
        Destroy(gameObject);
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
}
