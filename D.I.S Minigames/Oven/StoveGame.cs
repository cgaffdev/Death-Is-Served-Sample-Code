using System.Collections;
using System.Collections.Generic;
using FunkyCode;
using UnityEngine;

public class StoveGame : MonoBehaviour
{

    // Colliders and objects used to control where the sprite (foodObject) can go
    [SerializeField] List<BoxCollider2D> topColliders;
    [SerializeField] List<GameObject> wallList;
    [SerializeField] List<GameObject> temporaryCollisionBlockers; // temporary fix zones
    [SerializeField] SpriteRenderer foodObject;

    Item tempItem;

    CircleCollider2D foodCollider;
    public bool isDoorReady = true;
    bool isDoorClosed = true;

    void Awake(){

        // Creates a copy of the held player item in order to leave the original alone
        foodObject.sprite = PlayerManager.instance.GetHeldItem().sprite;

        tempItem = ScriptableObject.Instantiate(PlayerManager.instance.GetHeldItem());
        tempItem.AddTag(CookingManager.Tags.Baked);
        tempItem.RemoveTag(CookingManager.Tags.Raw);


        foodCollider = foodObject.GetComponent<CircleCollider2D>();
    }

    bool endOfGame = false;
    void LateUpdate(){
        
        if (endOfGame) return;

        // Checks to see which zone the food is in
        bool isOutside = IsOutside(); // only call once

        // Update collision boundaries depending on the door and food positions
        if (isOutside && !isDoorClosed){
            ToggleWalls(true);
            SiwtchColliders(false);
            IsCooking();
        }
        else{
            ToggleWalls(false);
            SiwtchColliders(true);
        }

        // Temporariyl disable 
        if (isCooking){
            SiwtchColliders(false);

        }

        // Complete the minigame when food reaches plate
        if (foodCollider.IsTouching(plateSpace))
        {
            foodObject.GetComponent<SpriteDragCollision>().Disable(true);
            endOfGame = true;
            if(gameEnded ==false) StartCoroutine(EndGame());

        }
    }

    void SiwtchColliders(bool status){
        foreach (GameObject obj in temporaryCollisionBlockers){
            obj.SetActive(status);
        }
    }

    void ToggleWalls(bool status){
        foreach (GameObject obj in wallList){
            obj.SetActive(status);
        }
    }


    // OPens or closes the oven and handles the cooking state
    public void AccessDoor(){

        isDoorClosed = !isDoorClosed;
        isDoorReady = false;

        // Start cooking when closing door
        if (isCooking && isDoorClosed && !cookingFinished){
            StartCoroutine(BakingTimer(cookingTime));
            StartCoroutine(SmokeFill(cookingTime));
        }

        // Only show final smoke sprite once cooking is finished AND door opens
        if (!isDoorClosed && cookingFinished){
            isCooking = false;

            smokeRenderer.sprite = smokeFrames[smokeFrames.Count - 1];
            foodObject.GetComponent<SpriteDragCollision>().Disable(false);

        }
    }


    // Checks whether the food is touching the oven's upper boudnary collider
    // Manages the food's visual sorting order.
    bool IsOutside(){

        foreach (BoxCollider2D collider in topColliders)
        {
            if (foodCollider.IsTouching(collider))
            {
                if (isCooking){
                    foodObject.sortingOrder = 3; // keep in front during or after cooking
                }
                else
                {
                    foodObject.sortingOrder = 6; // behind walls
                }
                return false;
            }
        }


        // Outside
        if (!isDoorClosed || isCooking || cookingFinished)
        {
            foodObject.sortingOrder = 3;
        }



        return true;
    }

    [SerializeField] BoxCollider2D cookSpace;
    [SerializeField] BoxCollider2D plateSpace;
    bool isCooking = false;


    // Locks the food in the oven once it reaches the cooking space
    void IsCooking(){
        if (foodCollider.IsTouching(cookSpace))
        {
            isCooking = true;

            foodObject.GetComponent<SpriteDragCollision>().Disable(true);
            isDoorReady = true;
            cookSpace.gameObject.SetActive(false);
        }

    }


    // Ends then closes the game with completition, rewarding the player.
    bool gameEnded = false;
    IEnumerator EndGame(){
        // Play sound
        gameEnded = true;
        SoundPlayer.Instance.PlaySound(SoundPlayer.Instance.gameComplete);

        foodObject.GetComponent<SpriteRenderer>().sprite = null;
        plateSpace.GetComponent<SpriteRenderer>().sprite = tempItem.sprite;
        Destroy(tempItem);

        yield return new WaitForSeconds(.5f);
        CookingManager.instance.OnOvenGather(); // Rewards the player
        PlayerManager.instance.FreezePlayer(false); // Returns movement to the player
        Destroy(gameObject);
    }


    [SerializeField] SpriteRenderer timerRenderer;
    [SerializeField] SpriteRenderer smokeRenderer;
    [SerializeField] List<Sprite> cookingFrames;
    [SerializeField] List<Sprite> smokeFrames;
    [SerializeField] float cookingTime = 10f; // total time in seconds

    bool cookingFinished = false;


    // Procressing the cooking timer animation
    IEnumerator BakingTimer(float duration){
        if (cookingFrames == null || cookingFrames.Count == 0 || timerRenderer == null)
            yield break;

        float frameTime = duration / cookingFrames.Count;

        for (int i = 0; i < cookingFrames.Count; i++)
        {
            timerRenderer.sprite = cookingFrames[i];
            yield return new WaitForSeconds(frameTime);
        }

        cookingFinished = true;
        isDoorReady = true;
        plateSpace.gameObject.SetActive(true);

        foodObject.sprite = CookingManager.instance.GetPlatelessSprite(tempItem);

    }

    //Plays the smoke aniumation, the final frame is reversed for the door opening
    IEnumerator SmokeFill(float duration){
        if (smokeFrames == null || smokeFrames.Count == 0 || smokeRenderer == null)
            yield break;

        float frameTime = duration / smokeFrames.Count;

        // Exclude last sprite
        for (int i = 0; i < smokeFrames.Count - 1; i++)
        {
            smokeRenderer.sprite = smokeFrames[i];
            yield return new WaitForSeconds(frameTime);
        }
    }

    // Closes the Mini-Game out without completition
    public void Close(){
        // Play sound
        if (gameEnded == true) return;
                SoundPlayer.Instance.PlayButton();

        PlayerManager.instance.FreezePlayer(false);

        Destroy(gameObject);
    }

}
