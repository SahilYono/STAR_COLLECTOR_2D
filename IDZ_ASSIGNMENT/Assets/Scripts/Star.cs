using UnityEngine;

public class Star : MonoBehaviour
{
    public static int activeCount = 0;
    public static int maxStars = 5;

    [Header("Spawn Area (set this to your rectangular area bounds)")]
    public Bounds spawnArea; // Set via StarSpawner or manually

    private StarSpawner spawner;

    public void Init(StarSpawner s)
    {
        spawner = s;
    }

    void OnEnable()
    {
        activeCount++;
    }

    void OnDisable()
    {
        activeCount--;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.Instance.AddScore(10);
            spawner.Respawn(this);
        }
    }
}