using UnityEngine;

public class Blocky : MonoBehaviour
{
    float deathTimer = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer < 0)
        {
            Blockplacement.Instance.blocksLeft += 1;
            Destroy(gameObject);
        }
    }
}
