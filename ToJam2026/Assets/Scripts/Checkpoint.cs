using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public static Checkpoint lastCheckpointS;
    public static GameObject lastCheckpoint;
    //public GameObject activeSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
        deactivateCheckpoint();
    }



    //makes this sprite the active sprite - TO BE TESTED
    public void activateCheckpoint()
    {
        //turn off the last checkpoint
        if (lastCheckpointS != null && lastCheckpoint!= this)
            lastCheckpointS.deactivateCheckpoint();
        else Debug.Log("no last checkpoint");


        _animator.SetBool("Active", true);//activeSprite.SetActive(true);
        lastCheckpointS = this;
        lastCheckpoint = gameObject;
        
    }

    //updates the sprite of the checkpoint - DONE
    public void deactivateCheckpoint()
    {
        //turns off the active sprite
        _animator.SetBool("Active", false); //activeSprite.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("trigger");
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log("activate");
            activateCheckpoint();
        }
    }

}//end class
