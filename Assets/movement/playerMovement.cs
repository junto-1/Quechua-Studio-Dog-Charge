using UnityEngine;

class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 8f;     // Velocidad constante

    [Header("Salto")]
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Velocidad constante hacia la derecha
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // Salto con click o barra espaciadora
        if (Input.GetButtonDown("Jump") || Input.GetMouseButtonDown(0))
        {
            Jump();
        }
    }

    void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}