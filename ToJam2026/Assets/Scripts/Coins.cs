using UnityEngine;

public class Coins : MonoBehaviour
{
    public Collider2D Player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object entered: " + collision.gameObject.layer);

        // Example: Check by tag
        if (collision.gameObject.layer == 6)
        {
            UIController.Instance.addcoin();
            //UIController.Instance.coins ++;
            Destroy(gameObject);
        }
    }

}
