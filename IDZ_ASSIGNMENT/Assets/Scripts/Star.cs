using UnityEngine;


public class Star : MonoBehaviour
{
    public bool isBonus = false;
    public int points = 10;             
    [Header("Bonus Settings (ignored if not bonus)")]
    public float lifetime = 5f;
    public float scaleStepTime = 0.15f;
    private float[] pulseScales = { 2f, 1.75f, 1.5f, 1.25f, 1f, 0.75f, 1f, 1.25f, 1.5f, 1.75f };
    private int scaleIndex = 0;
    private float scaleTimer = 0f;
    private float lifeTimer = 0f;

    [HideInInspector] public StarSpawner spawner;

    void OnEnable()
    {
        
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

        
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        
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