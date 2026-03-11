using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public BoxCollider2D patrolArea;
    public float patrolSpeed = 2f;      // Chase = patrolSpeed x 2 
    private Vector2[] corners = new Vector2[4];
    private int cornerIndex = 0;
    private bool isChasing = false;
    private bool wasChasing = false;

    void Start()
    {
        Bounds b = patrolArea.bounds;
        corners[0] = new Vector2(b.min.x, b.max.y); 
        corners[1] = new Vector2(b.max.x, b.max.y); 
        corners[2] = new Vector2(b.max.x, b.min.y); 
        corners[3] = new Vector2(b.min.x, b.min.y); 

        cornerIndex = NearestCorner();
        transform.position = corners[cornerIndex];
    }
    void Update()
    {
        if (UIManager.Instance.IsGameOver) return;

        isChasing = patrolArea.bounds.Contains(player.position);

        // stopped chasing → go back to nearest one
        if (wasChasing && !isChasing)
            cornerIndex = NearestCorner();

        if (isChasing) Chase();
        else Patrol();

        wasChasing = isChasing;
    }
    void Patrol()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, corners[cornerIndex], patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, corners[cornerIndex]) < 0.05f)
            cornerIndex = (cornerIndex + 1) % 4;
    }
    void Chase()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, player.position, patrolSpeed * 2f * Time.deltaTime);
    }
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            UIManager.Instance.ShowGameOver();
    }
}