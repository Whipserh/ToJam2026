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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layercheck)
        {
            isinside = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layercheck)
        {
            isinside = false;
        }
    }
}
