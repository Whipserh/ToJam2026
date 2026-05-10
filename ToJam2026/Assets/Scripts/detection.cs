using UnityEngine;

public class detection : MonoBehaviour
{
    public bool isinside = false;
    public LayerMask layercheck; //8


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (layercheck == 6)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isinside = true;
            }
        }
        else if (layercheck == (layercheck | 1 << collision.gameObject.layer))
        {
            isinside = true;
        }
        
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {



        if (layercheck == 6)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isinside = true;
            }
        }
        else if (layercheck == (layercheck | 1 << collision.gameObject.layer))
        {
            isinside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (layercheck == 6)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                isinside = false;
            }
        }
        else if (layercheck == (layercheck | 1 << collision.gameObject.layer))
        {
            isinside = false;
        }
    }
}
