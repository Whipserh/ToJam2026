using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint lastCheckpointS;
    public static GameObject lastCheckpoint;
    public GameObject activeSprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        deactivateCheckpoint();
    }



    //makes this sprite the active sprite - TO BE TESTED
    public void activateCheckpoint()
    {
        //turn off the last checkpoint
        if (lastCheckpointS != null)
            lastCheckpointS.deactivateCheckpoint();
        else Debug.Log("no last checkpoint");

        
        activeSprite.SetActive(true);
        lastCheckpointS = this;
        lastCheckpoint = gameObject;
        
    }

    //updates the sprite of the checkpoint - DONE
    public void deactivateCheckpoint()
    {
        //turns off the active sprite
        activeSprite.SetActive(false);
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
