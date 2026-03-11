using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public BoxCollider2D detectionArea; // Same box collider as the patrol area

    [Header("Speed")]
    public float patrolSpeed = 2f;
    // Chase speed = patrolSpeed * 2 (auto)

    // Patrol border points (TL -> TR -> BR -> BL -> TL clockwise)
    private Vector2[] borderPoints = new Vector2[4];
    private int currentPatrolIndex = 0;

    private enum State { Patrol, Chase }
    private State state = State.Patrol;

    private bool playerInside = false;

    void Start()
    {
        BuildBorderPoints();
        // Start at nearest border point
        currentPatrolIndex = GetNearestPointIndex();
        transform.position = borderPoints[currentPatrolIndex];
    }

    void BuildBorderPoints()
    {
        Bounds b = detectionArea.bounds;
        // Clockwise: TL, TR, BR, BL
        borderPoints[0] = new Vector2(b.min.x, b.max.y); // Top-Left
        borderPoints[1] = new Vector2(b.max.x, b.max.y); // Top-Right
        borderPoints[2] = new Vector2(b.max.x, b.min.y); // Bottom-Right
        borderPoints[3] = new Vector2(b.min.x, b.min.y); // Bottom-Left
    }

    void Update()
    {
        if (UIManager.Instance != null && UIManager.Instance.IsGameOver) return;

        // Check if player is inside detection area
        playerInside = detectionArea.bounds.Contains(player.position);

        if (playerInside)
            state = State.Chase;
        else
            state = State.Patrol;

        if (state == State.Chase)
            ChasePlayer();
        else
            PatrolBorder();
    }

    void PatrolBorder()
    {
        Vector2 target = borderPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, target, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
            currentPatrolIndex = (currentPatrolIndex + 1) % 4;
    }

    void ChasePlayer()
    {
        float chaseSpeed = patrolSpeed * 2f;
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // When player leaves, snap enemy target to nearest border point
    void ReturnToNearestPoint()
    {
        currentPatrolIndex = GetNearestPointIndex();
    }

    int GetNearestPointIndex()
    {
        int nearest = 0;
        float minDist = float.MaxValue;
        for (int i = 0; i < borderPoints.Length; i++)
        {
            float d = Vector2.Distance(transform.position, borderPoints[i]);
            if (d < minDist) { minDist = d; nearest = i; }
        }
        return nearest;
    }

    // Detect state change to resume patrol from nearest point
    private State lastState = State.Patrol;
    void LateUpdate()
    {
        if (lastState == State.Chase && state == State.Patrol)
            ReturnToNearestPoint();
        lastState = state;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Enemy collided with player -> Game Over
            UIManager.Instance.ShowGameOver();
        }
    }
}