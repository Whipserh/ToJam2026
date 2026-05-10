using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float coyoteTimeElapsed = 0; // resets after jumping
    private BoxCollider2D myCollider;

    public CharacterState currentCharacterState = CharacterState.idle;
    public CharacterState previousCharacterState = CharacterState.idle;

    [Header("Dash")]
    public float dashDistance;
    public float dashTime;
    //variables for dashing mechanics
    private bool DASHING; // if we are in the state of dashing
    private bool hadDashed; //if we had already dashed before value is reset
    
    private float dashElapsedTime = 0;

    [Header("Wall Jumps")]
    //wall jump varibales
    private int currentWallJumps;
    public int maxWallJumps = 2;

    private bool FALLING = false;
    
    [Header("Jumping & Physics")]
    public float fallTime;
    public float terminalVelocity, apexTime, apexHeight;
    public float coyoteTime;
    private float gravity;
    //is ground variables
    public LayerMask solidGround;//what layers act as the ground
    public Vector2 boxSize;//rough bottom hitbox
    
    //acceleration and deceleration are both positive terms
    public float acceleration, deceleration;
    private bool LEFT = false, RIGHT = false, JUMPED = false;
    private Vector2 playerInput;
    public float maxSpeed;


    [Header("Player Stats")]
    float MAX_HEALTH = 3;
    float currentHealth = 3;
    public Transform startingPos;
    public PlayerVisuals visuals;

    public AudioSource jumpsound;
    public AudioSource hitsound;

    public static PlayerController Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public enum CharacterState
    {
        idle, walk, jump, die, walkC, idleC, victoryPose
    }
    public void setVictoryPose()
    {
        currentCharacterState = CharacterState.victoryPose;
    }

    public enum FacingDirection
    {
        left, right
    }

    private FacingDirection currentFacingDirection = FacingDirection.right;
    private Rigidbody2D rb;

    public bool isCrouched = false;
    public bool previousIsCrouched = false;

    void Start()
    {;
        //setVictoryPose();
        transform.position = startingPos.position;

        currentWallJumps = 0;
        DASHING = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawCube((Vector2)transform.position + myCollider.offset, myCollider.size);
    }

    private bool respawning = false;
    void Update()
    {
        //Get player input
        


        #region Character State
        //****************************************************************************Character state

        //save the previous character state
        previousCharacterState = currentCharacterState;

        //update the current state that we are in
        if (IsDead())//if our character is dead then we don't need to look at anything else. 
        {
            currentCharacterState = CharacterState.die;
        }else if (respawning)
        {
            currentCharacterState = CharacterState.idle;
            respawning = false;
        }
        
        switch (currentCharacterState)
        {
            case CharacterState.idleC:
                if (IsWalking())
                {
                    if (isCrouched)
                        currentCharacterState = CharacterState.walkC;
                    else
                        currentCharacterState = CharacterState.walk;
                }
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }
                if (!isCrouched)
                    currentCharacterState = CharacterState.idle;
                break;
            case CharacterState.idle:
                if (IsWalking())
                {
                    if (isCrouched)
                        currentCharacterState = CharacterState.walkC;
                    else
                        currentCharacterState = CharacterState.walk;
                }
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }
                if(isCrouched)
                    currentCharacterState = CharacterState.idleC;
                break;
            case CharacterState.walk:
                
                //if we stop we idle
                if (!IsWalking())
                {
                    //if we are on the ground from walking and we are not walking
                    if (isCrouched)
                        currentCharacterState = CharacterState.idleC;
                    else
                        currentCharacterState = CharacterState.idle;
                }
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }

                //if we crouch while walking we crouch walk
                if (isCrouched)
                    currentCharacterState = CharacterState.walkC;
                break;
            case CharacterState.walkC:
                if (!IsWalking())
                {
                    //if we are on the ground from walking and we are not walking
                    if (isCrouched)
                        currentCharacterState = CharacterState.idleC;
                    else
                        currentCharacterState = CharacterState.idle;
                }
                if (!IsGrounded())
                {
                    currentCharacterState = CharacterState.jump;
                }
                //if we are crouch walking and we are no longer crouched switch back to walk
                if (!isCrouched)
                    currentCharacterState = CharacterState.walk;
                break;
            case CharacterState.jump:
                if (IsGrounded())
                {
                    if (IsWalking())
                    {
                        currentCharacterState = CharacterState.walk;
                    }
                    else
                    {
                        currentCharacterState = CharacterState.idle;
                    }
                }
                break;
            case CharacterState.die:
                //this does not have anything cause we want a very hard death moment
                break;
        }


        //any code that we implement that is frame specific

        if (!IsGrounded())//we are in the air
        {
            //Debug.Log("in the air");
            coyoteTimeElapsed = 0;
        }
        else//we landed
        {
            currentWallJumps = 0;//reset the number of wall jumps 
            hadDashed = false; // reset the dash when the player touches the floor
            JUMPED = false; //reset the jump when the player touches the ground
        }

        #endregion
        #region facing Direction
        //****************************************************************************Facing Direction

        if (!DASHING)
        //change the direction that the player is facing.
        if (Input.GetKey(KeyCode.D))
        {
            currentFacingDirection = FacingDirection.right;
        }else if (Input.GetKey(KeyCode.A))
        {
            currentFacingDirection = FacingDirection.left;
        }
        #endregion
        #region Dash Controls
        //****************************************************************************Dash controls
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashLegible()) //can only DASHING if we aren't dashing
        {
            hadDashed = true;
            Debug.Log("Dash");
            dashElapsedTime = 0;
            DASHING = true;
        }
        #endregion
        #region Movement Controls
        //*****************************************************************************Movement co
        //The input from the player needs to be determined and then passed
        //in the to the MovementUpdate which should manage the actual
        //movement of the character.
        playerInput = Vector2.zero;

        //disable the controls if we are dashing
        if (!DASHING)
        {
            if (Input.GetKey(KeyCode.D))
            {
                playerInput.x += 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                playerInput.x -= 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                isCrouched = true;
                myCollider.size = new Vector2(1.0f, 0.5f);
                myCollider.offset = new Vector2(0.0f, -0.25f);
            }
            else
            {
                if (!IsUnder())
                {
                    isCrouched = false;
                    myCollider.size = new Vector2(1.0f, 1.11f);
                    myCollider.offset = new Vector2(0.0f, 0.05f);
                }
            }

            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && (legibleJump() || legibleWallJump()))
            {
                JUMPED = true;
                jumpsound.Play();
                playerInput.y++;
            }
        }
        MovementUpdate(playerInput);

        coyoteTimeElapsed+=Time.deltaTime;
        #endregion
        previousIsCrouched = isCrouched;
    }//end update


    public bool legibleJump()
    {
        //Debug.Log(JUMPED);

        //coyote time should only start counting the moment the player is not longer on the ground
        return (!JUMPED && (IsGrounded() || (coyoteTimeElapsed < coyoteTime)));
    }

    public bool legibleWallJump()
    {
        //********************************************Get the side of the player that is touching the wall
        float direction = 0;
        if(Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.5f), 0, Vector2.left, 0.75f, solidGround))//check left
        {
            //Debug.Log("left");
            direction = 0;//left
        }else if(Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.5f), 0, Vector2.right, 0.75f, solidGround))//check right
        {
            //Debug.Log("right");
            direction = 1;//right
        }
        else//we don't see any legible position so 
        {
            return false;
        }
        //there is an object besides us if we make it HERE


        //check to see if the player is mid air
        if (!IsGrounded()&&currentWallJumps<maxWallJumps)
        {
            currentWallJumps++;
            //project the player in the opposite direction from the wall
            rb.linearVelocity= Vector2.Lerp(new Vector2(2*maxSpeed, rb.linearVelocity.y), new Vector2(-2*maxSpeed, rb.linearVelocity.y), direction);
            return true;
        }
        return false;
    }

    public bool dashLegible()
    {
        if (!hadDashed)// if we hadn't dashed AND we aren't in the middle of a dash
        {
            return true;
        }
        return false;
    }

    /**
     * 
     * */
    public int wallCollision()
    {
        int direction = -1;
        if (Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.5f), 0, Vector2.left, 0.75f, solidGround))//check left
        {
            //Debug.Log("left");
            direction = 0;//left
        }
        else if (Physics2D.BoxCast(transform.position, new Vector2(0.1f, 0.5f), 0, Vector2.right, 0.75f, solidGround))//check right
        {
            //Debug.Log("right");
            direction = 1;//right
        }
        else//we don't see any legible position so 
        {
            direction = -1;
        }
        return direction;
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        //JUMP - this is up here and not in the fixed update because the change happens in the frame not the , plus its an instant change not a change over time
        float initalJumpVelocoty = 2 * apexHeight / apexTime;
        if (playerInput.y > 0)//either the player is grounded or its been a couple of seconds since they left the ground
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, initalJumpVelocoty);
        }

        //Debug.Log(playerInput);

        //horizontal movement
        RIGHT = playerInput.x > 0;
        LEFT = playerInput.x < 0;

        //cap the max speed incase force is too strong
        if (Mathf.Abs(rb.linearVelocity.x) > maxSpeed)
        {
            float direction = new Vector2(rb.linearVelocity.x, 0).normalized.x;
            rb.linearVelocity = new Vector2(maxSpeed * direction, rb.linearVelocity.y);
        }

      

        //PLAYER TERMINAL VELOCITY
        if (rb.linearVelocity.y < -terminalVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -terminalVelocity);
        }
    }//end movement update
    


    private void FixedUpdate()
    {
        //******************************************** DASH
        dashElapsedTime += Time.fixedDeltaTime;
        //stop dash if either the elasped time is the dash time OR if they hit a wall
        if (dashElapsedTime >= dashTime || wallCollision() != -1)
        {
            DASHING = false;
        }


        //if we are dashing we move in a specific direction
        if (DASHING)
        {
            if(GetFacingDirection() == FacingDirection.right)
                rb.linearVelocity = new Vector2(dashDistance/dashTime, rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(-dashDistance / dashTime, rb.linearVelocity.y);
        }




        FALLING = rb.linearVelocity.y < 0; //we are falling if velocity is < 0

        //horizontal movement if the player pressed a button
        if (RIGHT)
        {
            //Debug.Log("right");
            rb.linearVelocity += Vector2.right * Time.fixedDeltaTime * acceleration * 20;
        }
        else if (LEFT)
        {
            //Debug.Log("left");
            rb.linearVelocity += Vector2.left * Time.fixedDeltaTime * acceleration * 20;
        }
        else // if we aren't moving then we should slow down
        {

            //Debug.Log(currentFacingDirection);
            rb.linearVelocity -= new Vector2(rb.linearVelocity.x, 0).normalized * Time.fixedDeltaTime * deceleration;
            if (Mathf.Abs(rb.linearVelocity.x) < 0.1)
            {
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            }
        }

        
        //GRAVITY
        if (rb.linearVelocity.y > 0) { // soft gravity
            gravity = -2 * apexHeight / Mathf.Pow(apexTime, 2);
        } else
        {
            gravity = -2 * apexHeight / Mathf.Pow(fallTime, 2);
        }
        if(!IsGrounded())
        rb.linearVelocity += gravity * Time.deltaTime * Vector2.up;
    }

    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    public bool IsWalking()
    {
        //if our character's horizontal speed is not 0 and they are not falling
        if (Mathf.Abs(rb.linearVelocity.x) >= 0.1f && IsGrounded())
            return true;
        return false;
    }

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, -transform.up, 0.5f, solidGround) && rb.linearVelocity.y < 0.01f;
    }
    public bool IsUnder()
    {
        return Physics2D.BoxCast(transform.position, boxSize, 0f, transform.up, 0.5f, solidGround) && rb.linearVelocity.y < 0.01f;
    }

    //referenced in the animator
    public IEnumerator OnAnimationDeathCompleet()
    {
        yield return new WaitForSeconds(0.8f);

        if (Checkpoint.lastCheckpoint != null) 
            transform.position = Checkpoint.lastCheckpoint.transform.position;
        else
            transform.position = startingPos.position;

        currentHealth = MAX_HEALTH;
        currentCharacterState = CharacterState.walk;
        visuals.updateHealthUI((int)currentHealth);

        //gameObject.SetActive(false);

        yield return null;
    }

    public FacingDirection GetFacingDirection()
    {
        return currentFacingDirection;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage * 1f;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        hitsound.Play();
        visuals.updateHealthUI(currentHealth);
    }

    IEnumerator wait()
    {
        
        yield return new WaitForSeconds(1f); // Wait 0.1s before continuing loop
    }

}
