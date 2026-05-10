using UnityEngine;
using UnityEngine.Audio;

public class Coins : MonoBehaviour
{
    public Collider2D Player;
    public AudioSource coinsounds;

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

        // Example: Check by tag
        if (collision.gameObject.CompareTag("Player"))
        {
            UIController.Instance.addcoin();
            coinsounds.transform.parent = null;
            coinsounds.Play();
            //UIController.Instance.coins ++;
            Destroy(gameObject);
        }
    }

}
