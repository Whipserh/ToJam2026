using Unity.Collections;
using UnityEngine;
using static PlayerController;

public class Rat : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    int mood; //0 = don't move, 1 = move left and right, 2= chase towards mouse, 3 = walking back
    Vector2 lastspot;
    public int defultMood; //0 for don't move, 1 for moving
    public int smarts; //0 = dumb, 1 = smart //Dumb runs off and doesn't go back, smart doesn't run off and does go back
    public float speed;
    int FacingDirection; //-1 = left, 1 = right
    float waiting;

     bool leftWallDec;
    bool groundInFrontOfRat;
    bool seesPlayer;
    bool withinAttackRange;
    public AudioSource AttackSound;
    float waitsound = 0;

    private enum RateState { idle, wonder, purse}

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        FacingDirection = -1;
        mood = defultMood;
    }

    void Update()
    {
        //animation
        switch (mood) {
            case 0:
                _animator.SetBool("Walk", false);
                break;
            case 1:
                _animator.SetBool("Walk", true);
                break;
            case 2:
                _animator.SetBool("Walk", true);
                break;
        }
        Debug.DrawLine(transform.position,transform.position + (new Vector3(FacingDirection * 0.72f, -1.32f)), Color.green);

        //Triggers
        #region triggers
        
        //check for the ground LEFT DETECT
        if (Physics2D.BoxCast(transform.position + (new Vector3(FacingDirection * 0.72f, -1.32f)), new Vector2(0.6f, 0.6f), 0, Vector2.left, 0.1f, ground))//if we detect the ground
            groundInFrontOfRat = true;
        else
            groundInFrontOfRat= false;
        //check for wall WALL DETECT
        if (Physics2D.BoxCast(transform.position + (new Vector3(FacingDirection * 1.2f, .012f)) , new Vector2(0.6f, 1.8f), 0, Vector2.left, 0.1f, ground))//if we detect the ground
            leftWallDec = true;
        else
            leftWallDec = false;



        //check for player vision LEFT VISION
        if (Physics2D.BoxCast(transform.position + (new Vector3(FacingDirection * 4.02f, 0)), new Vector2(6, 1.8f), 0, Vector2.left, 0.1f, player))//if we detect the ground
            seesPlayer = true;
        else
            seesPlayer = false;
        //check for attackPlayer attack player
        if (Physics2D.BoxCast(transform.position + (new Vector3(FacingDirection * 1.5f, -0.5f)), new Vector2(1, 1), 0, Vector2.left, 0.1f, player))//if we detect the ground
            withinAttackRange = true;
        else
            withinAttackRange = false;
        
        #endregion
        //leftWallDec = leftwalldecOBJ.isinside;
        //leftedgedec = leftedgedecOBJ.isinside;
        //leftplayerdec = leftplayerdecOBJ.isinside;
        //attackPlayer = attackPlayerBox.isinside;

        if (withinAttackRange)
        {
            Debug.Log("attack player");
            _animator.SetTrigger("attack");
            waitsound += Time.deltaTime;
            if (!AttackSound.isPlaying)
            {
                if (waitsound > 0.75f)
                {
                    AttackSound.Play();
                    waitsound = 0;
                }
            }


        }

        //if we detect a player
        if (seesPlayer)
        {
            mood = 2;
        }
        
        
        //smart rat will check to see if there is an edge before walking
        switch (mood){
            case 0://stand in a spot
                waiting += Time.deltaTime;
                if (waiting >= 1)
                {
                            turnAround();
                            waiting = 0;
                }
                        break;
            case 1://walk around

                if (smarts == 0)//dumb rat runs off
                    transform.Translate(Vector3.right * speed * FacingDirection * Time.deltaTime);
                //if there is ground in front of a rat and they aren't about to walk into a wall ->Move Rat
                else if (groundInFrontOfRat && !leftWallDec) //!leftwalldec || 
                    transform.Translate(Vector3.right * speed * FacingDirection * Time.deltaTime);
                else
                    turnAround();
                        
                break;
            case 2://run after player
                
                if (!seesPlayer)//wait if the rat doesn't see player
                    waiting += Time.deltaTime;
                
                else if (smarts == 0)//dumb rat runs off
                    transform.Translate(Vector3.right * speed * 1.5f * FacingDirection * Time.deltaTime);
                //if there is ground in front of a rat and they aren't about to walk into a wall ->Move Rat 50% Faster
                else if (groundInFrontOfRat && !leftWallDec)
                    transform.Translate(Vector3.right * speed * 1.5f * FacingDirection * Time.deltaTime);

                        //if we don't see the player after a certain amount of time we 
                        if (waiting >= 0.1)
                        {
                            waiting = 0;
                            mood = defultMood;
                        }
                        break;
            }


    }//end update

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
    /**
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("Something is here");
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.takeDamage(1);
        }
    }**/
    public LayerMask player, ground;


    public void attemptPlayerHit()
    {
        
        //if the player is with range and of the boxcast
        if (Physics2D.BoxCast(transform.position + (new Vector3(FacingDirection * 1.5f, -0.5f)), new Vector2(0.5f, 1), 0, Vector2.left, 0.1f, player))
        {
            PlayerController.Instance.takeDamage(1);
        }
    }

}
