using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI coinCounter;
    public int coins;
    int maxCoins;


    public static UIController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coins = 0;
        maxCoins = 200;//better in the future
        coinCounter.text = "Coins: " + coins + "/" + maxCoins;
    }

    // Update is called once per frame
    public void addcoin()
    {
        coins++;
        coinCounter.text = "Coins: " + coins + "/" + maxCoins;
    }
}
