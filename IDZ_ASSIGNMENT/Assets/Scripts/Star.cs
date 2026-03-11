using UnityEngine;

// Attach to BOTH star prefabs (regular and bonus)
// Set isBonus = true on the bonus star prefab
public class Star : MonoBehaviour
{
    [Header("Type")]
    public bool isBonus = false;

    [Header("Points")]
    public int points = 10;             // Regular = 10, Bonus = 100

    // Bonus only
    [Header("Bonus Settings (ignored if not bonus)")]
    public float lifetime = 5f;
    public float scaleStepTime = 0.15f;

    private float[] pulseScales = { 2f, 1.75f, 1.5f, 1.25f, 1f, 0.75f, 1f, 1.25f, 1.5f, 1.75f };
    private int scaleIndex = 0;
    private float scaleTimer = 0f;
    private float lifeTimer = 0f;

    // Set by StarSpawner so item can call back for respawn
    [HideInInspector] public StarSpawner spawner;

    void OnEnable()
    {
        // Reset bonus state every time it activates
        if (isBonus)
        {
            lifeTimer = 0f;
            scaleTimer = 0f;
            scaleIndex = 0;
            transform.localScale = Vector3.one * pulseScales[0];
        }
    }

    void Update()
    {
        if (!isBonus) return;

        // Auto-destroy after lifetime
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        // Pulse scale animation
        scaleTimer += Time.deltaTime;
        if (scaleTimer >= scaleStepTime)
        {
            scaleTimer = 0f;
            scaleIndex = (scaleIndex + 1) % pulseScales.Length;
            transform.localScale = Vector3.one * pulseScales[scaleIndex];
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        UIManager.Instance.AddScore(points);

        if (isBonus)
            gameObject.SetActive(false);
        else
            spawner.Respawn(this);
    }
}