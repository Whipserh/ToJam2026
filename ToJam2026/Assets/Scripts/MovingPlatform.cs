using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] Points;
    int currentPoint;
    Vector2 currentGoal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPoint = 0;
        currentGoal = Points[currentPoint].transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(currentGoal, transform.position) < 0.1f)
        {
            currentPoint += 1;
            if (currentPoint >= Points.Length)
            {
                currentPoint = 0;
            }
            currentGoal = Points[currentPoint].transform.position;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, currentGoal, 3 * Time.deltaTime);
        }
    }
}
