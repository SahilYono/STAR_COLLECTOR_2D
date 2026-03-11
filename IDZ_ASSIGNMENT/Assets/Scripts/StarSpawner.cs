using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Handles spawning for both regular stars (pool of 5) and bonus star (timed)
public class StarSpawner : MonoBehaviour
{
    [Header("Spawn Area")]
    public BoxCollider2D spawnArea;

    [Header("Regular Stars")]
    public GameObject starPrefab;
    public float respawnDelay = 0.5f;

    [Header("Bonus Star")]
    public GameObject bonusStarPrefab;
    public float bonusMinInterval = 5f;
    public float bonusMaxInterval = 12f;

    private const int POOL_SIZE = 5;
    private List<Star> pool = new List<Star>();
    private Star bonusStar;
    private float nextBonusTime;

    // ── Init ──────────────────────────────────────────────
    void Start()
    {
        // Build regular star pool
        for (int i = 0; i < POOL_SIZE; i++)
        {
            Star s = Instantiate(starPrefab).GetComponent<Star>();
            s.spawner = this;
            s.gameObject.SetActive(false);
            pool.Add(s);
        }

        // Spawn all regular stars immediately
        foreach (var s in pool)
            PlaceAndActivate(s);

        // Create bonus star (starts inactive)
        bonusStar = Instantiate(bonusStarPrefab).GetComponent<Star>();
        bonusStar.gameObject.SetActive(false);
        ScheduleBonus();
    }

    // ── Update (bonus timer) ──────────────────────────────
    void Update()
    {
        if (UIManager.Instance.IsGameOver) return;

        if (!bonusStar.gameObject.activeSelf && Time.time >= nextBonusTime)
        {
            PlaceAndActivate(bonusStar);
            ScheduleBonus();
        }
    }

    // ── Called by StarItem on collect ─────────────────────
    public void Respawn(Star star)
    {
        star.gameObject.SetActive(false);
        StartCoroutine(DelayedRespawn(star));
    }

    IEnumerator DelayedRespawn(Star star)
    {
        yield return new WaitForSeconds(respawnDelay);
        PlaceAndActivate(star);
    }

    // ── Helpers ───────────────────────────────────────────
    void PlaceAndActivate(Star star)
    {
        star.transform.position = RandomPoint();
        star.gameObject.SetActive(true);
    }

    void ScheduleBonus()
    {
        nextBonusTime = Time.time + Random.Range(bonusMinInterval, bonusMaxInterval);
    }

    Vector2 RandomPoint()
    {
        Bounds b = spawnArea.bounds;
        return new Vector2(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y)
        );
    }
}