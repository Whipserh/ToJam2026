using UnityEngine;
using System.Collections;

public class Cheese : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Bob());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Bob()
    {
        while (1 == 1 || 1 != 1)
        {
            transform.position = new Vector2(transform.position.x, 17.1f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.2f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.3f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.4f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.5f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.6f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.7f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.8f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.9f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 18.0f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 18.0f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.9f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.8f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.7f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.6f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.5f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.4f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.3f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.2f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.1f);
            yield return new WaitForSeconds(0.1f);
            transform.position = new Vector2(transform.position.x, 17.0f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
