using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 input;

    [Header("Animation")]
    private Animator animator;
    private float lastMoveX = 0f;
    private float lastMoveY = -1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        bool isPerformingAction = IsActionStateActive();

        if (isPerformingAction)
        {
            input = Vector2.zero;
        }
        else
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            input = input.normalized;
        }

        if (input != Vector2.zero)
        {
            lastMoveX = input.x;
            lastMoveY = input.y;
        }

        if (animator != null)
        {
            animator.SetFloat("MoveX", input != Vector2.zero ? input.x : lastMoveX);
            animator.SetFloat("MoveY", input != Vector2.zero ? input.y : lastMoveY);
            animator.SetBool("Moving", input != Vector2.zero);
            
            SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null && lastMoveX != 0)
            {
                spriteRenderer.flipX = lastMoveX < 0;
            }
        }
    }

    void FixedUpdate()
    {
        if (IsActionStateActive())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = input * moveSpeed;
    }

    public void StartWatering()
    {
        if (animator != null && !IsActionStateActive() && !animator.GetBool("IsWatering"))
        {
            animator.SetBool("IsWatering", true);
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

    private bool IsActionStateActive()
    {
        if (animator == null) return false;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        return stateInfo.IsName("Water_Forward") ||
               stateInfo.IsName("Water_Backward") ||
               stateInfo.IsName("Water_Right") ||
               stateInfo.IsName("Water_Left") ||
               stateInfo.IsName("Swinging_Forward") ||
               stateInfo.IsName("Swinging_Backward") ||
               stateInfo.IsName("Swinging_Right") ||
               stateInfo.IsName("Swinging_Left") ||
               stateInfo.IsName("Laying_Down");
    }

    private IEnumerator StopWateringAfterAnimation()
    {
        yield return null; 

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float clipLength = stateInfo.length; 
        
        if (clipLength <= 0.01f)
        {
            clipLength = 1f;
        }

        yield return new WaitForSeconds(clipLength);

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
            clipLength = 1f;
        }
        
        yield return new WaitForSeconds(clipLength);
        
        if (animator != null)
        {
            animator.SetBool("IsPlanting", false);
        }
    }
}