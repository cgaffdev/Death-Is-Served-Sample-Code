using UnityEngine;


// Requires a RB 2D component, allowing the sprite to move using Unity's physics system
// This is to respect and interact with other 2d colliders.
[RequireComponent(typeof(Rigidbody2D))]
public class SpriteDragCollision : MonoBehaviour
{

    /// This code has a LOT of room for improvement.
    /// The originally design of it is to allow the player to move a sprite and keep respects to collisions. 
    /// When dragging a sprite in unity to the mouse, if you simply set the sprite's position to the mouse it will ignore colliders
    /// Thus, the script's purpose is to: respect colliders for sprites to not pass through them, follow the mouse smoothly, prevent sprites from rotating.
    /// 
    /// A big improvement to this could would be setting the mouse position as well when the sprite can't move to prevent cursor sliding. 


    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private bool dragging = false;
    bool isMouseClick = false;


    private Vector3 offset;

    [SerializeField] private float followSpeed = 15f; // higher = tighter follow
    private Vector3 originalPosition;

    private bool disabled = false;
    private int spriteLayer;

    void Awake()
    {
        // Save sprite's original position and sort order
        spriteLayer = spriteRenderer.sortingOrder;
        originalPosition = transform.position;

        // Retrieve references
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;


        // Disables gravity so sprite's don't fall
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    
    void OnMouseDown()
    {
        // Prevents interaction if 
        if (disabled) return;

        isMouseClick = true;
        dragging = true;

        // Converts screen to world position
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mouseWorld.x, mouseWorld.y, transform.position.z);

        if (isMouseHover)
        {
            isMouseHover=true;
            CursorManager.instance.CursorDrag();
        }
    }


    void OnMouseUp()
    {
        dragging = false;
        isMouseClick = false;


        rb.velocity = Vector2.zero;                 // stops remaining physical movement
        transform.position = originalPosition;      // Returns sprite to original pos
        spriteRenderer.sortingOrder = spriteLayer;  // Resets the sprite to it's original layer
    }

    bool isMouseHover = false;

    // Function to send information to the cursor to visually update 
    void OnMouseOver()
    {
        if (isMouseHover && isMouseClick)
        {
            CursorManager.instance.CursorDrag();
        }
        else
        {
            CursorManager.instance.CursorHover();
            
        }
        isMouseHover=true;
    }

 
    void OnMouseExit()
    {

        isMouseHover=false;
        CursorManager.instance.CursorDefault();

    }

    void Update()
    {
        // Locks the sprite's z position
        transform.position = new Vector3(transform.position.x, transform.position.y, originalPosition.z);
        spriteRenderer.transform.position=  new Vector3( spriteRenderer.transform.position.x,  spriteRenderer.transform.position.y, originalPosition.z);

    }


    void FixedUpdate()
    {
        // When the variable disabled == true, it ceases movement and returns it to the original position
        if (disabled)
        {
            rb.velocity = Vector2.zero;
            transform.position = originalPosition; 
            return;
        }

        transform.rotation = Quaternion.identity; // lock rotation

        if (dragging)
        {
            Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 targetPosition2D = new Vector2(mouseWorld.x + offset.x, mouseWorld.y + offset.y);

            // Smooth movement
            Vector2 direction = targetPosition2D - rb.position;
            rb.velocity = direction * followSpeed;

            // Update position using Rigidbody2D without touching z
            transform.position = new Vector3(rb.position.x, rb.position.y, transform.position.z);
        }
        else
        {
            rb.velocity = Vector2.zero; // Stops movement
            transform.position = new Vector3(rb.position.x, rb.position.y, transform.position.z); // Keeps the transform ligned up with the RB
        }
    }

    // If a script externally tells this one to disable, it keeps the stops movement and makes the frozen position as it's new origin.
    public void Disable(bool status)
    {
        disabled = status;

        if (disabled)
        {
            originalPosition=transform.position;
            
            // Restores original data
            rb.velocity = Vector2.zero;
            transform.position = originalPosition;
            spriteRenderer.sortingOrder = spriteLayer;
            OnMouseUp(); // Forces the mouse off the object
        }
    }
}
