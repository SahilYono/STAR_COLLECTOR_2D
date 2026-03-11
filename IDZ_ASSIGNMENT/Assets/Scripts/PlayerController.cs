using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Boundaries")]
    public float minX = -20f, maxX = 20f;
    public float minY = -11f, maxY = 11f;

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = 0f, v = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1f;
        }

        moveInput = new Vector2(h, v).normalized;

        if (h != 0)
            transform.localScale = new Vector3(h < 0 ? -1 : 1, 1, 1);

        if (animator != null)
            animator.SetBool("isMoving", moveInput.magnitude > 0);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;

        // Clamp within screen bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }
}