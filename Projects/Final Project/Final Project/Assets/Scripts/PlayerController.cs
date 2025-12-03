using System.Collections;
using UnityEngine;

// Requires a Rigidbody2D component on the same GameObject
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // --- Public Inspector Variables ---

    [Header("Movement")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 3f;

    // --- Private Variables ---

    private Rigidbody2D rb;
    private Vector2 input;

    [Header("Animation")]
    private Animator animator;
    // Stores the last non-zero movement direction for Idle animation direction
    private float lastMoveX = 0f;
    private float lastMoveY = -1f; // Default facing down

    // --- Unity Lifecycle Methods ---

    void Start()
    {
        // Get components
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        // Configuration for 2D top-down movement
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity for top-down
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
        }
        else
        {
            Debug.LogError("Rigidbody2D component not found on PlayerController!");
        }

        if (animator == null)
        {
            Debug.LogError("Animator component not found in children of PlayerController!");
        }
    }

    void Update()
    {
        // Check if the player is currently performing an action that should block movement
        bool isPerformingAction = IsActionStateActive();

        // 1. Handle Input
        if (isPerformingAction)
        {
            // Lock input while performing an action
            input = Vector2.zero;
        }
        else
        {
            // Get raw input for horizontal and vertical axes
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            // Normalize input to prevent diagonal speed increase
            input = input.normalized;
        }

        // 2. Update Last Movement Direction (for Idle/Action facing direction)
        if (input != Vector2.zero)
        {
            lastMoveX = input.x;
            lastMoveY = input.y;
        }

        // 3. Update Animator Parameters
        if (animator != null)
        {
            // Pass current input or last movement direction to set facing direction
            animator.SetFloat("MoveX", input != Vector2.zero ? input.x : lastMoveX);
            animator.SetFloat("MoveY", input != Vector2.zero ? input.y : lastMoveY);
            // Set "Moving" boolean
            animator.SetBool("Moving", input != Vector2.zero);
            
            // Flip sprite based on direction (for mirroring left/right animations)
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && lastMoveX != 0)
            {
                spriteRenderer.flipX = lastMoveX < 0; // flip when facing left
            }
        }
    }

    void FixedUpdate()
    {
        // Prevent movement during watering or other actions
        if (IsActionStateActive())
        {
            // Stop movement completely
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Move player using physics in FixedUpdate
        rb.linearVelocity = input * moveSpeed;
    }

    // --- Public Action Methods ---

    // Called by UI button, input, or an interaction system
    public void StartWatering()
    {
        if (animator != null && !IsActionStateActive() && !animator.GetBool("IsWatering"))
        {
            // Trigger the animation transition
            animator.SetBool("IsWatering", true);
            // Start coroutine to reset the flag after the animation finishes
            StartCoroutine(StopWateringAfterAnimation());
        }
    }

    public void StartSwinging()
    {
        if (animator != null && !IsActionStateActive() && !animator.GetBool("IsSwinging"))
        {
            animator.SetBool("IsSwinging", true);
            StartCoroutine(StopSwingingAfterAnimation());
        }
    }

    public void StartPlanting()
    {
        if (animator != null && !IsActionStateActive() && !animator.GetBool("IsPlanting"))
        {
            animator.SetBool("IsPlanting", true);
            StartCoroutine(StopPlantingAfterAnimation());
        }
    }

    // --- Private Helper Methods ---

    /// <summary>
    /// Checks if the player is currently in an animation state that blocks movement.
    /// You should update this with the names of all action-blocking states.
    /// </summary>
    /// <returns>True if an action state is currently playing.</returns>
    private bool IsActionStateActive()
    {
        if (animator == null) return false;

        // Check for specific action states
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        // IMPORTANT: You must match the name of the state in your Animator Controller!
        return stateInfo.IsName("Water_Forward") ||
               stateInfo.IsName("Water_Backward") ||
               stateInfo.IsName("Water_Right") ||
               stateInfo.IsName("Water_Left") ||
               stateInfo.IsName("Swinging_Forward") ||
               stateInfo.IsName("Swinging_Backward") ||
               stateInfo.IsName("Swinging_Right") ||
               stateInfo.IsName("Planting_Forward") ||
               stateInfo.IsName("Planting_Backward") ||
               stateInfo.IsName("Laying_Down") ||
               stateInfo.IsName("Planting_Right");
    }

    /// <summary>
    /// Waits for the current action animation to complete and then resets the IsWatering flag.
    /// </summary>
    private IEnumerator StopWateringAfterAnimation()
    {
        // 1. Wait a frame for the animation system to start the transition
        yield return null; 

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length; 
        
        // Safety check to ensure we got a valid length
        if (clipLength <= 0.01f)
        {
            Debug.LogWarning("Failed to get action clip length. Using 1 second fallback.");
            clipLength = 1f;
        }

        // 2. Wait for the animation to finish
        yield return new WaitForSeconds(clipLength);

        // 3. Reset the IsWatering flag
        if (animator != null)
        {
            animator.SetBool("IsWatering", false);
        }
    }

    private IEnumerator StopSwingingAfterAnimation()
    {
        yield return null;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length;
        
        if (clipLength <= 0.01f)
        {
            Debug.LogWarning("Failed to get action clip length. Using 1 second fallback.");
            clipLength = 1f;
        }
        
        yield return new WaitForSeconds(clipLength);
        
        if (animator != null)
        {
            animator.SetBool("IsSwinging", false);
        }
    }

    private IEnumerator StopPlantingAfterAnimation()
    {
        yield return null;
        
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length;
        
        if (clipLength <= 0.01f)
        {
            Debug.LogWarning("Failed to get action clip length. Using 1 second fallback.");
            clipLength = 1f;
        }
        
        yield return new WaitForSeconds(clipLength);
        
        if (animator != null)
        {
            animator.SetBool("IsPlanting", false);
        }
    }
}