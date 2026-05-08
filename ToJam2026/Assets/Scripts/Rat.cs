using UnityEngine;

public class Rat : MonoBehaviour
{
    int mood; //0 = don't move, 1 = move left and right, 2= chase towards mouse
    Vector2 lastspot;
    public int defultMood; //0 for don't move, 1 for moving
    public int smarts; //0 = dumb, 1 = smart //Dumb runs off and doesn't go back, smart doesn't run off and does go back
    int FacingDirection; //0 = left, 1 = right

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
