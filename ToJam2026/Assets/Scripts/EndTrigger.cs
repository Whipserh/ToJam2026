using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndTrigger : MonoBehaviour
{
    public GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame()
    {
        PlayerController.Instance.setVictoryPose();
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(3);

    }
}
