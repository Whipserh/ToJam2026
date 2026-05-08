using UnityEngine;

public class detection : MonoBehaviour
{
    public bool isinside;
    public int layercheck; //8

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
        print("Anything");
        if (collision.gameObject.layer == layercheck)
        {
            isinside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        print("Anything");
        if (collision.gameObject.layer == layercheck)
        {
            isinside = false;
        }
    }
}
