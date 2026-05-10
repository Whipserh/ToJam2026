using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyScript : MonoBehaviour
{

    public enum RatState { ANGRY, ATTACK, WALK, IDLE}
    public RatState currentState;
    public float idleTurnTime;
    private float turnTimer = 0;
    public detection groundDetector, seeEnemy, seeWall, attackFrame;
    
    

    void Start()
    {
    }

    public void turnAround()
    {

    }

    void Update()
    {
        
        //
        switch (currentState)
        {
            case RatState.IDLE:
                if (turnTimer >= idleTurnTime)
                    turnAround();
                else
                    turnTimer += Time.deltaTime;
                
                break;
            case RatState.WALK:

            case RatState.ANGRY:
                break;
        }
    }
}
