using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

/// <summary>
/// This script manages updating the visuals of the character based on the values that are passed to it from the PlayerController.
/// NOTE: You shouldn't make changes to this script when attempting to implement the functionality for the W10 journal.
/// </summary>
public class PlayerVisuals : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer bodyRenderer;
    public PlayerController playerController;

    private int isWalkingHash, isGroundedHash, onDieHash;

    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        //the reason why we choose to use hash is because it is less task expensive
        //it is expensive to pass a string to a hash, so if we do that every frame things can start to slow down. 
        isWalkingHash = Animator.StringToHash("IsWalking");
        isGroundedHash = Animator.StringToHash("IsGrounded");
        onDieHash = Animator.StringToHash("OnDeath");
    }

    // Update is called once per frame
    void Update()
    {
        VisualsUpdate();
    }

    public GameObject heart1, heart2, heart3;
    public void updateHealthUI(float health)
    {
        health = Mathf.Clamp(health, 0, 3);
        switch (health)
        {
            case 0:
                heart1.SetActive(false);
                heart2.SetActive(false);
                heart3.SetActive(false);
                print("Health 0");
                break;
            case 1:
                heart1.SetActive(true);
                heart2.SetActive(false);
                heart3.SetActive(false);
                print("Health 1");
                break;
            case 2:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(false);
                print("Health 2");
                break;
            case 3:
                heart1.SetActive(true);
                heart2.SetActive(true);
                heart3.SetActive(true);
                print("Health 3");
                break;
        }
    }
    
    private void VisualsUpdate()
    {
        /**
        animator.SetBool(isWalkingHash, playerController.IsWalking());
        animator.SetBool(isGroundedHash, playerController.IsGrounded());
        
       if(playerController.IsDead())
        animator.SetTrigger(onDieHash);
        **/

        if(playerController.previousCharacterState != playerController.currentCharacterState)
        {
            switch (playerController.currentCharacterState)
            {
                case CharacterState.idle:
                    if (playerController.previousCharacterState == CharacterState.jump)
                        cameraController.Shake(0.2f, 0.2f);
                    animator.CrossFade("Idle", 0f);
                    break;
                case CharacterState.walk:
                    if (playerController.previousCharacterState == CharacterState.jump)
                        cameraController.Shake(0.2f, 0.2f);
                    animator.CrossFade("Walking", 0f);
                    break;
                case CharacterState.jump:
                    animator.CrossFade("Jumping", 0f);
                    break;
                case CharacterState.die:
                    animator.CrossFade("Die", 0f);
                    break;
            }
        }//end if


        switch (playerController.GetFacingDirection())
        {
            case PlayerController.FacingDirection.left:
                bodyRenderer.flipX = false;
                break;
            case PlayerController.FacingDirection.right:
            default:
                bodyRenderer.flipX = true;
                break;
        }
    }
}
