using Unity.Collections;
using UnityEngine;
using static PlayerController;

public class Rat : MonoBehaviour
{
    int mood; //0 = don't move, 1 = move left and right, 2= chase towards mouse, 3 = walking back
    Vector2 lastspot;
    public int defultMood; //0 for don't move, 1 for moving
    public int smarts; //0 = dumb, 1 = smart //Dumb runs off and doesn't go back, smart doesn't run off and does go back
    public float speed;
    int FacingDirection; //-1 = left, 1 = right
    float waiting;

    public detection leftwalldecOBJ;
    public detection leftedgedecOBJ;
    public detection leftplayerdecOBJ;

    bool leftwalldec;
    bool leftedgedec;
    bool leftplayerdec;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FacingDirection = -1;
        mood = defultMood;
    }

    // Update is called once per frame
    void Update()
    {
        //Triggers
        leftwalldec = leftwalldecOBJ.isinside;
        leftedgedec = leftedgedecOBJ.isinside;
        leftplayerdec = leftplayerdecOBJ.isinside;
        //Dumb first

        //print("Left wall: " + leftwalldec + "Right wall: " + rightwalldec + "Left floor: " + leftedgedec + "Right floor: " + rightedgedec);

        if (leftplayerdec)
        {
            mood = 2;
        }
        switch (smarts) { 
            case 0://dumb rat -> will run off edge
            switch (mood) {
                    case 0:
                waiting += Time.deltaTime;
                if (waiting >= 1)
                {
                    turnAround();
                    waiting = 0;
                }
                        break;
                    case 1:
            
                transform.Translate(Vector3.right * speed * FacingDirection * Time.deltaTime);
                if (leftwalldec)
                {
                    turnAround();
                    //waiting += Time.deltaTime;
                }
                //if (waiting >= 1)
                {
                    //turnAround();
                    //waiting = 0;
                }
                        break;
                    case 2:
            
                transform.Translate(Vector3.right * speed * 1.5f * FacingDirection * Time.deltaTime);
                if (!leftplayerdec)
                {
                    waiting += Time.deltaTime;
                }

                if (waiting >= 0.1)
                {
                    waiting = 0;
                    mood = defultMood;
                }
                        break;
            }//inner switch
                break;
        case 1://smart rat will check to see if there is an edge before walking
                switch (mood){
                    case 0:
                        waiting += Time.deltaTime;
                        if (waiting >= 1)
                        {
                            turnAround();
                            waiting = 0;
                        }
                        break;
                    case 1:
                        if (leftedgedec == true && leftwalldec == false) //!leftwalldec || 
                        {
                            transform.Translate(Vector3.right * speed * FacingDirection * Time.deltaTime);
                        }
                        else
                        {
                            turnAround();
                            //waiting += Time.deltaTime;
                        }
                        //if (waiting >= 1)
                        {
                            //turnAround();
                            //waiting = 0;
                        }
                        break;
                    case 2:
                        if (leftedgedec == true && leftwalldec == false)
                        {
                            transform.Translate(Vector3.right * speed * 1.5f * FacingDirection * Time.deltaTime);
                        }
                        if (!leftplayerdec)
                        {
                            waiting += Time.deltaTime;
                        }

                        if (waiting >= 0.1)
                        {
                            waiting = 0;
                            mood = defultMood;
                        }
                        break;
            }
                break;
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
        transform.localScale = new Vector2(
        -transform.localScale.x,
        transform.localScale.y
        );

        if (FacingDirection == -1)
        {
            FacingDirection = 1;
        }
        else
        {
            FacingDirection = -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Something is here");
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.takeDamage(1);
        }
    }



}
