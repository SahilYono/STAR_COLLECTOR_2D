using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public BoxCollider2D patrolArea;

    [Header("Speed")]
    public float patrolSpeed = 2f;      // Chase = patrolSpeed x 2 (auto)

    private Vector2[] corners = new Vector2[4];
    private int cornerIndex = 0;
    private bool isChasing = false;
    private bool wasChasing = false;

    // ── Init ──────────────────────────────────────────────
    void Start()
    {
        Bounds b = patrolArea.bounds;
        corners[0] = new Vector2(b.min.x, b.max.y); // Top-Left
        corners[1] = new Vector2(b.max.x, b.max.y); // Top-Right
        corners[2] = new Vector2(b.max.x, b.min.y); // Bottom-Right
        corners[3] = new Vector2(b.min.x, b.min.y); // Bottom-Left

        cornerIndex = NearestCorner();
        transform.position = corners[cornerIndex];
    }

    // ── Main Loop ─────────────────────────────────────────
    void Update()
    {
        if (UIManager.Instance.IsGameOver) return;

        isChasing = patrolArea.bounds.Contains(player.position);

        // Just stopped chasing → go back to nearest corner
        if (wasChasing && !isChasing)
            cornerIndex = NearestCorner();

        if (isChasing) Chase();
        else Patrol();

        wasChasing = isChasing;
    }

    // ── Patrol clockwise along 4 corners ─────────────────
    void Patrol()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, corners[cornerIndex], patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, corners[cornerIndex]) < 0.05f)
            cornerIndex = (cornerIndex + 1) % 4;
    }

    // ── Chase player at 2x speed ──────────────────────────
    void Chase()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, player.position, patrolSpeed * 2f * Time.deltaTime);
    }

    // ── Returns index of closest corner ──────────────────
    int NearestCorner()
    {
        int nearest = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < corners.Length; i++)
        {
            float d = Vector2.Distance(transform.position, corners[i]);
            if (d < minDist) { minDist = d; nearest = i; }
        }
        return nearest;
    }

    // ── Collision with player → Game Over ─────────────────
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            UIManager.Instance.ShowGameOver();
    }
}