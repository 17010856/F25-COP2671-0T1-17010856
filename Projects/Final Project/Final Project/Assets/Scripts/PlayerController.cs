using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
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
        rb.gravityScale = 0;     
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // Get player input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        // Update last movement direction
        if (input != Vector2.zero)
        {
            lastMoveX = input.x;
            lastMoveY = input.y;
        }

        // Update animator
        if (animator != null)
        {
            animator.SetFloat("MoveX", input != Vector2.zero ? input.x : lastMoveX);
            animator.SetFloat("MoveY", input != Vector2.zero ? input.y : lastMoveY);
            animator.SetBool("Moving", input != Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        // Move using velocity so collisions work properly
        rb.linearVelocity = input * moveSpeed;
    }
}
