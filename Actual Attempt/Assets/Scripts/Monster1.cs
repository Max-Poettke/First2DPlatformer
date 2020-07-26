using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int wayPointIndex = 0;
    public float moveSpeed = 2f; 

    private void Start()
    {
        transform.position = wayPoints[wayPointIndex].transform.position;
    }
    void Update()
    {
        Move();
    }

    void Move()
    {
            var targetPos = wayPoints[wayPointIndex].transform.position;
            var moveBy = moveSpeed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveBy);
            if (transform.position == targetPos)
            {
                wayPointIndex++;
            }
            if (wayPointIndex >= wayPoints.Count)
        {
            wayPointIndex = 0;
        }
    }

}   
