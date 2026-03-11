using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StarSpawner : MonoBehaviour
{
    
    public BoxCollider2D spawnArea;

    public GameObject starPrefab;
    public float respawnDelay = 0.5f;

    public GameObject bonusStarPrefab;
    public float bonusMinInterval = 5f;
    public float bonusMaxInterval = 12f;

    private const int POOL_SIZE = 5;
    private List<Star> pool = new List<Star>();
    private Star bonusStar;
    private float nextBonusTime;

    void Start()
    {
        // regular star pool
        for (int i = 0; i < POOL_SIZE; i++)
        {
            Star s = Instantiate(starPrefab).GetComponent<Star>();
            s.spawner = this;
            s.gameObject.SetActive(false);
            pool.Add(s);
        }

       
        foreach (var s in pool)
            PlaceAndActivate(s);

        // bonus star 
        bonusStar = Instantiate(bonusStarPrefab).GetComponent<Star>();
        bonusStar.gameObject.SetActive(false);
        ScheduleBonus();
    }

    
    void Update()
    {
        if (UIManager.Instance.IsGameOver) return;

        if (!bonusStar.gameObject.activeSelf && Time.time >= nextBonusTime)
        {
            PlaceAndActivate(bonusStar);
            ScheduleBonus();
        }
    }

    
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