using Unity.VisualScripting;
using UnityEngine;

public class move : MonoBehaviour
{
    float speed = 10f; // Speed of movement
    public GameObject circle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MovetoCircle();
        }
    }
    public void MovetoCircle()
    {
        Instantiate (circle, transform.position, Quaternion.identity);

    }
}
    
