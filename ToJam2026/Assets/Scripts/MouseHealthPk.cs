using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class MouseHealthPk : MonoBehaviour
{
    public int healthGain = 1;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.takeDamage(healthGain);
            Destroy(gameObject);
        }
        
    }
}
