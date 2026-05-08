using UnityEngine;
using static PlayerController;

public class Rat : MonoBehaviour
{
    int mood; //0 = don't move, 1 = move left and right, 2= chase towards mouse, 3 = walking back
    Vector2 lastspot;
    public int defultMood; //0 for don't move, 1 for moving
    public int smarts; //0 = dumb, 1 = smart //Dumb runs off and doesn't go back, smart doesn't run off and does go back
    int FacingDirection; //0 = left, 1 = right
    float waiting;

    public detection leftwalldecOBJ;
    public detection rightwalldecOBJ;
    public detection leftedgedecOBJ;
    public detection rightedgedecOBJ;
    public detection leftplayerdecOBJ;
    public detection rightplayerdecOBJ;

    bool leftwalldec;
    bool rightwalldec;
    bool leftedgedec;
    bool rightedgedec;
    bool leftplayerdec;
    bool rightplayerdec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FacingDirection = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //Triggers
        leftwalldec = leftwalldecOBJ.isinside;
        rightwalldec = rightwalldecOBJ.isinside;
        leftedgedec = leftedgedecOBJ.isinside;
        rightedgedec = rightedgedecOBJ.isinside;
        leftplayerdec = leftplayerdecOBJ.isinside;
        rightplayerdec = rightplayerdecOBJ.isinside;
        //Dumb first

        if (leftplayerdec || rightplayerdec)
        {
            mood = 2;
        }

        if (smarts == 0)
        {
            if (mood == 0)
            {
                waiting += Time.deltaTime;
                if (waiting >= 1)
                {
                    turnAround();
                    waiting = 0;
                }
            }
            if (mood == 1)
            {

            }
            if (mood == 2)
            {

            }
        }
        if (smarts == 1)
        {
            if (mood == 0)
            {
                waiting += Time.deltaTime;
                if (waiting >= 1)
                {
                    turnAround();
                    waiting = 0;
                }
            }
            if (mood == 1)
            {

            }
            if (mood == 2)
            {

            }
            if (mood == 3)
            {

            }
        }


        //check if edge
        // if so, turn around
        // else check if see enemy
        // if see enemy change to stage 2
        // else move forward 
        // or stay still


    }

    void turnAround()
    {
        if (FacingDirection == 0)
        {
            FacingDirection = 1;
        }
        else
        {
            FacingDirection = 0;
        }
    }

}
