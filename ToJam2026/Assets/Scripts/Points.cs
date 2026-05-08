using UnityEngine;

public class Points : MonoBehaviour
{
    void Awake()
    {
        // Detaches child from parent, keeping world position
        transform.SetParent(null, true);
    }
}
