using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Animator animator;

    private float lastMoveX = 0f;
    private float lastMoveY = -1f; // Default facing down (or whatever your sprite uses)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); // Animator on Farmer_Bob child
    }

    void Update()
    {
        // Get input
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        // Remember last movement direction
        if (input != Vector2.zero)
        {
            lastMoveX = input.x;
            lastMoveY = input.y;
        }

        // Set animator parameters
        if (animator != null)
        {
            animator.SetFloat("MoveX", input != Vector2.zero ? input.x : lastMoveX);
            animator.SetFloat("MoveY", input != Vector2.zero ? input.y : lastMoveY);
            animator.SetBool("Moving", input != Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * moveSpeed * Time.fixedDeltaTime);
    }
}
