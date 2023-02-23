using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingBird : MonoBehaviour
{
    [SerializeField] Transform[] Points;        //all the points that need to be followed
    [SerializeField] private float moveSpeed;   // move speed of bird
    private int pointsIndex;                    //points index
    
    void Start()
    {
        transform.position = Points[pointsIndex].transform.position;//when starting the game it will go to the first point
    }     
    
    // Update is called once per frame
    void Update()
    {
        if (pointsIndex <= Points.Length)
        {
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);
            //keep counting new points
            if (transform.position == Points[pointsIndex].transform.position)
            {
                pointsIndex += 1;
            }
            //reset all points and start from point 1
            if (pointsIndex == Points.Length)
            {
                pointsIndex = 0;
            }
        }
    }
}