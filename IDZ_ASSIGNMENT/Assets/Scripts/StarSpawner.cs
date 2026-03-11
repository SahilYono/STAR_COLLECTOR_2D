using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    [Header("Spawn Area - assign a BoxCollider2D as trigger to define area")]
    public BoxCollider2D areaCollider;

    [Header("Star Prefab")]
    public GameObject starPrefab;

    [Header("Respawn Delay")]
    public float respawnDelay = 0.5f;

    private List<Star> pool = new List<Star>();
    private const int maxStars = 5;

    void Start()
    {
        for (int i = 0; i < maxStars; i++)
        {
            GameObject go = Instantiate(starPrefab);
            Star s = go.GetComponent<Star>();
            s.Init(this);
            go.SetActive(false);
            pool.Add(s);
        }

        foreach (var star in pool)
            SpawnAt(star);
    }

    void SpawnAt(Star star)
    {
        star.transform.position = RandomPoint();
        star.gameObject.SetActive(true);
    }

    public void Respawn(Star star)
    {
        star.gameObject.SetActive(false);
        StartCoroutine(RespawnDelay(star));
    }

    IEnumerator RespawnDelay(Star star)
    {
        yield return new WaitForSeconds(respawnDelay);
        star.transform.position = RandomPoint();
        star.gameObject.SetActive(true);
    }

    Vector2 RandomPoint()
    {
        Bounds b = areaCollider.bounds;
        float x = Random.Range(b.min.x, b.max.x);
        float y = Random.Range(b.min.y, b.max.y);
        return new Vector2(x, y);
    }
}