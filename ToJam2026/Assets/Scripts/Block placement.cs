using UnityEngine;

public class Blockplacement : MonoBehaviour
{

    public GameObject Blocky;
    public int blocksLeft = 1;
    public AudioSource placesound;

    public static Blockplacement Instance { get; private set; }

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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            SpawnBlock(Vector2.up);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            SpawnBlock(Vector2.down);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            SpawnBlock(Vector2.left);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            SpawnBlock(Vector2.right);
        }
    }

    void SpawnBlock(Vector2 directions)
    {
        if (blocksLeft == 0)
        {
            return;
        }
        placesound.Play();
        Instantiate(Blocky, (Vector2)transform.position + directions, transform.rotation);
        blocksLeft -= 1;
    }
}
