using UnityEngine;

public class Spike : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object entered: " + collision.gameObject.layer);

        // Example: Check by tag
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.currentHealth -= 10;
        }
    }
}
